using Flurl.Http;
using Peasie.Contracts;
using PeasieAPI.Services.Interfaces;
using PeasieLib.Interfaces;
using PeasieLib.Services;
using System.Text;

namespace PeasieLib.Handlers;

public class PeasieEndpointHandler
{
    public void RegisterAPIs(WebApplication app,
        SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
    {
        var applicationContextService = app.Services.GetService(typeof(IPeasieApplicationContextService)) as IPeasieApplicationContextService;
        IDataManagerService? dataManagerService = app.Services.GetService(typeof(IDataManagerService)) as IDataManagerService;

        // GROUPS OF ENDPOINTS ==================================================
        var tokenHandler = app.MapGroup("/token").WithTags("Token Service API");
        var sessionHandler = app.MapGroup("/session").WithTags("Session Service API");
        var paymentHandler = app.MapGroup("/payment").WithTags("Payment Service API");
        var hookHandler = app.MapGroup("/hook").WithTags("WebHook API");
        var hangfireHandler = app.MapGroup("/hangfire").WithTags("Hangfire Service API");
        var adminHandler = app.MapGroup("/admin").WithTags("Admin View");

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
                ValidityTimeSpan = ((applicationContextService == null) ? new TimeSpan(0, 0, 30) : applicationContextService.Lifetime),
                PublicKey = peasiePublicKey
            };

            var json = JsonSerializer.Serialize<SessionResponseDTO>(sessionResponseDTO);
            var encrypted = EncryptionService.EncryptUsingPublicKey(json, sessionRequest.PublicKey);
            var reply = new PeasieReplyDTO
            {
                Payload = encrypted
            };

            // remember
            dataManagerService.Sessions[sessionGuid.ToString()] = new SessionWrapper() { SessionRequest = sessionRequest, SessionResponse = sessionResponseDTO, PrivateKey = peasiePrivateKey };

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::SessionRequest");
            return Results.Ok(reply);
        });

        _ = sessionHandler.MapPost("/details", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::SessionDetails");
            var session = dataManagerService.Sessions[peasieRequestDTO.Id];
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
            dataManagerService.Sessions[peasieRequestDTO.Id].SessionDetails = sessionDetailsDTO;

            // Set ASPNET user for authentication and authorization...

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::SessionDetails");
            return Results.Ok();
        });

        _ = sessionHandler.MapPost("/assert", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::Assert");
            SessionWrapper? session;
            if (dataManagerService != null && dataManagerService.Sessions.ContainsKey(peasieRequestDTO.Id))
            {
                if (dataManagerService.Sessions.TryGetValue(peasieRequestDTO.Id, out session))
                {
                    string? decrypted = null;
                    if (session != null && !string.IsNullOrEmpty(session.PrivateKey))
                    {
                        decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, session.PrivateKey);
                        var sessionVerificationRequestDTO = JsonSerializer.Deserialize<SessionVerificationRequestDTO>(decrypted);
                        if (string.IsNullOrEmpty(decrypted) || sessionVerificationRequestDTO == null)
                            return Results.BadRequest();
                    }
                }
                else return Results.Problem();
            }
            else
            {
                return Results.NotFound();
            }

            applicationContextService?.Logger.LogDebug($"Session {session?.SessionDetails?.User?.Type} verified");
            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::Assert");
            return Results.Ok();
        }).RequireAuthorization("IsAuthorized");

        // HOOK =================================================================

        _ = hookHandler.MapPost("/PaymentTrxUpdate", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::PaymentTrxUpdate");

            // Decrypt using bank session public key
            var session = dataManagerService.Sessions[peasieRequestDTO.Id];
            string? decrypted = null;

            if (session == null)
            {
                return Results.BadRequest();
            }

            // Encrypt beneficiary session public key en send to beneficiary hook

            applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrxUpdate");
            return Results.Ok();
        }).RequireAuthorization("IsAuthorized");

        // PAYMENT ==============================================================

        _ = paymentHandler.MapPost("/request", (PeasieRequestDTO peasieRequestDTO) =>
        {
            applicationContextService?.Logger.LogDebug("-> PeasieEndpointHandler::PaymentRequest");
            PaymentRequestDTO? paymentRequest = null;

            var session = dataManagerService.Sessions[peasieRequestDTO.Id];
            string? decrypted = null;
            if (session != null && !string.IsNullOrEmpty(session.PrivateKey))
            {
                decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, session.PrivateKey);
                paymentRequest = JsonSerializer.Deserialize<PaymentRequestDTO>(decrypted);
            }

            if (string.IsNullOrEmpty(decrypted) || paymentRequest == null || string.IsNullOrEmpty(paymentRequest.BeneficiaryPublicKey))
            {
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest (decryption problem)");
                return Results.BadRequest();
            }

            if(paymentRequest.SessionDetails == null || paymentRequest.SessionDetails.User == null || string.IsNullOrEmpty(paymentRequest.SessionDetails.User.Email))
            {
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest (session details not avail)");
                return Results.BadRequest();
            }

            // we encrypt for bank
            // search bank session: email
            applicationContextService?.Logger.LogDebug($"Searching email: {paymentRequest.SessionDetails.User.Email}");
            foreach(var s in dataManagerService.Sessions.Values)
            {
                applicationContextService?.Logger.LogDebug($"{s.SessionDetails?.User?.Email}");
            }
            var bankSession = dataManagerService.Sessions.Values.Where(s => s.SessionResponse != null && s.SessionDetails.User.Email == paymentRequest.SessionDetails.User.Email && s.SessionDetails.User.Type.ToLowerInvariant() == "bank").FirstOrDefault();

            if (bankSession == null)
            {
                applicationContextService?.Logger.LogError("Matching financial institute not found");
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest");
                return Results.BadRequest();
            }

            PeasieRequestDTO bankPeasieRequestDTO = new()
            {
                Id = bankSession.SessionResponse.SessionGuid.ToString(),
                Payload = EncryptionService.EncryptUsingPublicKey(decrypted, bankSession.SessionRequest.PublicKey)
            };

            var url = applicationContextService?.FinancialInstituteUrl + "/payment/request";
            applicationContextService?.Logger.LogDebug($"Constructed url: {url}");
            var reference = url.WithOAuthBearerToken(bankSession.SessionDetails?.JWTAuthorizationToken).PostJsonAsync(bankPeasieRequestDTO).Result;

            var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;

            if (string.IsNullOrEmpty(reply.Payload))
            {
                applicationContextService?.Logger.LogError("No reply from BANK");
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentRequest");
                return Results.BadRequest();
            }

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

            var session = dataManagerService.Sessions[peasieRequestDTO.Id];
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
            var bankSession = dataManagerService.Sessions.Values.Where(s => s.SessionResponse != null && s.SessionDetails.User.Email == session.SessionDetails.User.Email && s.SessionDetails.User.Type.ToLowerInvariant() == "bank").FirstOrDefault();

            if (bankSession == null)
            {
                applicationContextService?.Logger.LogError("Matching bank not found");
                applicationContextService?.Logger.LogDebug("<- PeasieEndpointHandler::PaymentTrx");
                return Results.BadRequest();
            }

            var newPeasieRequestDTO = new PeasieRequestDTO()
            {
                Id = bankSession.SessionResponse.SessionGuid.ToString(),
                Payload = EncryptionService.EncryptUsingPublicKey(decrypted, bankSession.SessionRequest.PublicKey)
            };

            //_logger.LogDebug(sessionSymmetricPwdEnc.HexDump());

            var url = applicationContextService?.FinancialInstituteUrl + "/payment/trx";
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

        // ADMIN =================================================================

        _ = adminHandler.MapGet("/", () =>
        {
            applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::AdminIndex");
            var html = System.IO.File.ReadAllText(@"./Assets/admin.html");
            StringBuilder htmlContentBuilder = new();
            htmlContentBuilder.Append(applicationContextService?.ToHtml());
            // htmlContentBuilder.Append(_paymentTransactions.IEnumerableToHtmlTable());
            html = html.Replace("{{placeholder}}", htmlContentBuilder.ToString());
            applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::AdminIndex");
            return Results.Extensions.Html(html);
        });

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
