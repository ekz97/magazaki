﻿using Flurl.Http;
using Peasie.Contracts;
using PeasieAPI.Interfaces;
using PeasieLib.Services;

namespace PeasieLib.Handlers;

public class PeasieEndpointHandler
{
    private readonly Dictionary<Guid, SessionWrapper> _sessions = new();
    private readonly string _basicFinancialInstituteUrl = "https://localhost:7126";

    public void RegisterAPIs(WebApplication app,
        SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
    {
        var applicationContextService = app.Services.GetService(typeof(IApplicationContextService)) as IApplicationContextService;

        // GROUPS OF ENDPOINTS ==================================================
        var tokenHandler = app.MapGroup("/token").WithTags("Token Service API");
        var sessionHandler = app.MapGroup("/session").WithTags("Session Service API");
        var paymentHandler = app.MapGroup("/payment").WithTags("Payment Service API");
        var hookHandler = app.MapGroup("/hook").WithTags("WebHook API");
        var hangfireHandler = app.MapGroup("/hangfire").WithTags("Hangfire Service API");

        /*
        var nlogHandler = app.MapGroup("/nlog").WithTags("NLog, EF and Dapper Services API");
        var compressingHandler = app.MapGroup("/compressing").WithTags("Compressing Service API");
        var httpClientHandler = app.MapGroup("/httpClient").WithTags("HTTP client to call another REST API");
        */

        // ======================================================================

        // TOKEN ================================================================

        /*
        _ = tokenHandler.MapGet("/generateToken", async () =>
        {
            var token = await TokenService.GenerateToken();
            return token;
        });

        _ = tokenHandler.MapGet("/generateSignedToken", async () =>
        {
            var token = await TokenService.GenerateSignedToken(issuer, audience, key);
            return token;
        });

        _ = tokenHandler.MapGet("/generateSignedTokenFromCertificate", async () =>
        {
            var token = await TokenService.GenerateSignedTokenFromCertificate(issuer, audience, signingCertificateKey);
            return token;
        });
        */

        _ = tokenHandler.MapGet("/generateEncryptedToken", async () =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::GenerateEncryptedToken");
            var token = await TokenService.GenerateEncryptedToken(applicationContextService.Issuer, applicationContextService.Audience, key);
            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::GenerateEncryptedToken");
            return Results.Ok(token);
        });

        /*
        _ = tokenHandler.MapGet("/generateEncryptedTokenFromCertificate", async () =>
        {
            var token = await TokenService.GenerateEncryptedTokenFromCertificate(issuer, audience, key, signingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateJOSEFromCertificate", async () =>
        {
            var token = await TokenService.GenerateJOSEFromCertificate(issuer, audience, signingCertificateKey, encryptingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateEncryptedTokenNotSigned", async () =>
        {
            var token = await TokenService.GenerateEncryptedTokenNotSigned(issuer, audience, key, signingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateJOSERandomlySigned", async () =>
        {
            var token = await TokenService.GenerateJOSERandomlySigned();
            return token;
        });

        _ = tokenHandler.MapGet("/tryToken", () => 
        { 
            Results.Ok(); 
        }).RequireAuthorization("IsAuthorized"); // if we want to check ourselves ... specify own policy!
        */

        // SESSION ==============================================================

        _ = sessionHandler.MapPost("/request", (SessionRequestDTO sessionRequest) => 
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::SessionRequest");
            if (string.IsNullOrEmpty(sessionRequest.PublicKey))
                return Results.BadRequest();

            var sessionGuid = Guid.NewGuid();
            TokenService.GeneratePPKRandomly(out string peasiePrivateKey, out string peasiePublicKey);
            var sessionResponseDTO = new SessionResponseDTO
            {
                SessionGuid = sessionGuid,
                ReplyTimeUtc = DateTime.UtcNow,
                ValidityTimeSpan = new TimeSpan(0, 30, 0), // TODO: configure
                PublicKey = peasiePublicKey
            };

            var json = JsonSerializer.Serialize<SessionResponseDTO>(sessionResponseDTO);
            var encrypted = EncryptionService.EncryptUsingPublicKey(json, sessionRequest.PublicKey);
            var reply = new PeasieReplyDTO
            {
                Payload = encrypted
            };

            // remember
            _sessions[sessionGuid] = new SessionWrapper() { SessionRequest = sessionRequest, SessionResponse = sessionResponseDTO, PrivateKey = peasiePrivateKey };

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::SessionRequest");
            return Results.Ok(reply);
        }).RequireAuthorization("IsAuthorized");

        _ = sessionHandler.MapPost("/details", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::SessionDetails");
            var session = _sessions[peasieRequestDTO.Id];
            if(session == null)
                return Results.BadRequest();

            string? decrypted = null;
            SessionDetailsDTO? sessionDetailsDTO = null;
            if (session != null && !string.IsNullOrEmpty(session.PrivateKey))
            {
                decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, session.PrivateKey);
                sessionDetailsDTO = JsonSerializer.Deserialize<SessionDetailsDTO>(decrypted);
            }

            // remember
            _sessions[peasieRequestDTO.Id].SessionDetails = sessionDetailsDTO;

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::SessionDetails");
            return Results.Ok();
        }).RequireAuthorization("IsAuthorized");

        _ = sessionHandler.MapPost("/assert", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::Assert");
            var session = _sessions[peasieRequestDTO.Id];
            string? decrypted = null;
            if (session != null && !string.IsNullOrEmpty(session.PrivateKey))
            {
                decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, session.PrivateKey);
                var sessionVerificationRequestDTO = JsonSerializer.Deserialize<SessionVerificationRequestDTO>(decrypted);
                if (string.IsNullOrEmpty(decrypted) || sessionVerificationRequestDTO == null)
                    return Results.BadRequest();            
            }

            applicationContextService?.Logger.LogDebug($"Session {session.SessionDetails.User.Type} verified");
            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::Assert");
            return Results.Ok();
        }).RequireAuthorization("IsAuthorized");

        // HOOK =================================================================

        _ = hookHandler.MapPost("/PaymentTrxUpdate", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::PaymentTrxUpdate");

            // Decrypt using bank session public key
            var session = _sessions[peasieRequestDTO.Id];
            string? decrypted = null;

            if (session == null)
            {
                return Results.BadRequest();
            }

            // Encrypt beneficiary session public key en send to beneficiary hook

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrxUpdate");
            return Results.Ok();
        });

        // PAYMENT ==============================================================

        _ = paymentHandler.MapPost("/request", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::PaymentRequest");
            PaymentRequestDTO? paymentRequest = null;

            var session = _sessions[peasieRequestDTO.Id];
            string? decrypted = null;
            if (session != null && !string.IsNullOrEmpty(session.PrivateKey))
            {
                decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, session.PrivateKey);
                paymentRequest = JsonSerializer.Deserialize<PaymentRequestDTO>(decrypted);
            }

            if (string.IsNullOrEmpty(decrypted) || paymentRequest == null || string.IsNullOrEmpty(paymentRequest.BeneficiaryPublicKey))
            {
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest");
                return Results.BadRequest();
            }

            // we encrypt for bank
            // search bank session: email
            var bankSession = _sessions.Values.Where(s => s.SessionResponse != null && s.SessionDetails.User.Email == paymentRequest.SessionDetails.User.Email && s.SessionDetails.User.Type.ToLowerInvariant() == "bank").FirstOrDefault();

            if (bankSession == null)
            {
                applicationContextService?.Logger.LogError("Matching financial institute not found");
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest");
                return Results.BadRequest();
            }

            PeasieRequestDTO bankPeasieRequestDTO = new()
            {
                Id = bankSession.SessionResponse.SessionGuid,
                Payload = EncryptionService.EncryptUsingPublicKey(decrypted, bankSession.SessionRequest.PublicKey)
            };

            var url = _basicFinancialInstituteUrl + "/payment/request";
            var reference = url.WithOAuthBearerToken(bankSession.SessionDetails.JWTAuthorizationToken).PostJsonAsync(bankPeasieRequestDTO).Result;

            var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;

            if (string.IsNullOrEmpty(reply.Payload))
                return Results.BadRequest();

            // Unpack and pack
            var bankReplyDecrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, bankSession.PrivateKey);

            var bankReply = JsonSerializer.Deserialize<PeasieReplyDTO>(bankReplyDecrypted);

            var bankReplyToBeneficiary = new PeasieReplyDTO()
            {
                Payload = EncryptionService.EncryptUsingPublicKey(bankReplyDecrypted, session.SessionRequest.PublicKey)
            };

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest");
            return Results.Ok(bankReplyToBeneficiary);
        }).RequireAuthorization("IsAuthorized");

        _ = paymentHandler.MapPost("/trx", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrx");

            PeasieRequestDTO? bankPeasieRequestDTO = null;

            var session = _sessions[peasieRequestDTO.Id];
            string decrypted = null;
            if (session != null && !string.IsNullOrEmpty(session.PrivateKey))
            {
                decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, session.PrivateKey);
                bankPeasieRequestDTO = JsonSerializer.Deserialize<PeasieRequestDTO>(decrypted);
            }

            if (string.IsNullOrEmpty(decrypted) || bankPeasieRequestDTO == null)
            {
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrx");
                return Results.BadRequest();
            }

            // we decrypt for the bank
            var bankSession = _sessions.Values.Where(s => s.SessionResponse != null && s.SessionDetails.User.Email == session.SessionDetails.User.Email && s.SessionDetails.User.Type.ToLowerInvariant() == "bank").FirstOrDefault();

            if (bankSession == null)
            {
                applicationContextService?.Logger.LogError("Matching bank not found");
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrx");
                return Results.BadRequest();
            }

            var newPeasieRequestDTO = new PeasieRequestDTO()
            {
                Id = bankSession.SessionResponse.SessionGuid,
                Payload = EncryptionService.EncryptUsingPublicKey(decrypted, bankSession.SessionRequest.PublicKey)
            };

            //_logger.LogDebug(sessionSymmetricPwdEnc.HexDump());

            var url = _basicFinancialInstituteUrl + "/payment/trx";
            var reference = url.WithOAuthBearerToken(bankSession.SessionDetails.JWTAuthorizationToken).PostJsonAsync(newPeasieRequestDTO).Result;

            var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;

            if (string.IsNullOrEmpty(reply.Payload))
            {
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrx");
                return Results.BadRequest();
            }

            // Unpack and pack
            var bankReplyDecrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, bankSession.PrivateKey);

            var bankReply = JsonSerializer.Deserialize<PeasieReplyDTO>(bankReplyDecrypted);

            var bankReplyToBeneficiary = new PeasieReplyDTO()
            {
                Payload = EncryptionService.EncryptUsingPublicKey(bankReplyDecrypted, session.SessionRequest.PublicKey)
            };
            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrx");
            return Results.Ok(bankReplyToBeneficiary);
        }).RequireAuthorization("IsAuthorized");


        /*
        _ = hangfireHandler.MapGet("/recurringTryToken", () =>
        {
            logger.LogInformation("Initializing RecurringJobManager");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();

        _ = hangfireHandler.MapGet("/removeRecurringJob", () =>
        {
            logger.LogInformation("Removing RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        _ = hangfireHandler.MapGet("/recurringSampleJob", () =>
        {
            logger.LogInformation("Initializing RecurringJob SampleJob");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("SampleJob", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        });

        _ = hangfireHandler.MapGet("/removeSampleJob", () =>
        {
            logger.LogInformation("Removing RecurringJob SampleJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("SampleJob");
        });

        _ = nlogHandler.MapGet("/tryNLog", () =>
        {
            logger.LogCritical("This is a critical good sample");
            return Results.Ok();
        });

        _ = compressingHandler.MapGet("/tryCompression", async () =>
        {
            var jsonToCompress = JsonSerializer.Serialize(forecast);

            var compressedData = await CompressingService.Compress(jsonToCompress);

            return compressedData;
        });

        _ = compressingHandler.MapGet("/tryDecompression", async (string compressedData) =>
        {
            var decompressedData = await CompressingService.Decompress(compressedData);

            return decompressedData;
        });

        _ = nlogHandler.MapGet("/getLogsWithEntityFrameworkAndLinq", async (int page, int pageSize) =>
        {
            var stopwatch = Stopwatch.StartNew();
            List<Nlog> logs;
            var param = 1;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 1;
            using (var context = new PeasieAPIDbContext())
            {
                using var dbContextTransaction = await context.Database.BeginTransactionAsync();
                logs = await (from l in context.Nlog
                              where param == 1
                              select l).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            return Results.Ok(new { elapsedTime, logs });
        });

        _ = nlogHandler.MapGet("/getLogsWithEntityFrameworkAndSql", () =>
        {
            var stopwatch = Stopwatch.StartNew();
            var param = 1;

            using var dbContext = new PeasieAPIDbContext();
            var logs = dbContext.Nlog.FromSqlInterpolated($"SELECT * FROM NLog WHERE 1 = {param} ").ToList();

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            return Results.Ok(new { elapsedTime, logs });
        });

        _ = nlogHandler.MapGet("/getLogsWithDapperAndSqlClient", async () =>
        {
            var stopwatch = Stopwatch.StartNew();

            using var connection = new MySqlConnection(app.Configuration.GetConnectionString("PeasieAPIDB"));
            await connection.OpenAsync();
            var logs = (await connection.QueryAsync<Nlog>("SELECT * FROM NLog WHERE 1 = @param ",
                new { param = (int?)1 })).ToList();

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            return Results.Ok(new { elapsedTime, logs });
        });

        _ = httpClientHandler.MapGet("/getAnotherEndpoint", async (HttpContext context) =>
        {
            var request = context?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}{request?.PathBase}{request?.QueryString}";
            baseUrl = baseUrl.Last().ToString() != "/" ? baseUrl.TrimEnd('/') : baseUrl;

            var client = new HttpClient();
            using var response = await client.GetAsync(new Uri(baseUrl + "/httpClient/getThisEndpoint/"), HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            return response.Content.Headers.ContentType?.MediaType == System.Net.Mime.MediaTypeNames.Application.Json
                ? await response.Content.ReadFromJsonAsync<dynamic>()
                : await response.Content.ReadAsStringAsync();
        });

        _ = httpClientHandler.MapGet("/getThisEndpoint", () => Results.Ok(new { res = "This is the response from httpClient/getThisEndpoint" }));
        */
    }
}
