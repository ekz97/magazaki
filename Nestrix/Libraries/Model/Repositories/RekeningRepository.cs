using System.Data;
using DataLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;
using MySqlConnector;

namespace DataLayer.Repositories;

public class RekeningRepository : IRekeningRepository
{
    private readonly string _connectionString;
    private readonly TransactieRepository _transactieRepository;
    //private readonly GebruikerRepository _gebruikerRepository;

    public RekeningRepository(string connectionString)
    {
        _connectionString = connectionString;
        _transactieRepository = new(_connectionString);
        //_gebruikerRepository = new(_connectionString);
    }

    public async Task RekeningToevoegenAsync(Rekening rekening)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "INSERT INTO Rekening (Rekeningnummer, IBAN, GebruikerId, RekeningType, Krediet, Saldo, is_visible) VALUES (@rekeningnummer, @iban, @gebruikerId, @rekeningtype, @krediet, @saldo, @is_visible)";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@rekeningnummer", rekening.Rekeningnummer);
            command.Parameters.AddWithValue("@iban", rekening.Iban);
            command.Parameters.AddWithValue("@gebruikerId", rekening.Gebruiker.Id);
            command.Parameters.AddWithValue("@rekeningtype", rekening.RekeningType.ToString());
            command.Parameters.AddWithValue("@krediet", rekening.KredietLimiet);
            command.Parameters.AddWithValue("@saldo", rekening.Saldo);
            command.Parameters.AddWithValue("@is_visible", 1);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException("Er is een fout opgetreden bij het toevoegen van de rekening.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task RekeningVerwijderenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "UPDATE Rekening SET is_visible = @is_visible WHERE Rekeningnummer = @rekeningnummer";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@rekeningnummer", id);
            command.Parameters.AddWithValue("@is_visible", 0);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException("Er is een fout opgetreden bij het verwijderen van de rekening.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task RekeningWijzigenAsync(Guid id, Rekening rekening)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "UPDATE Rekening SET Rekeningnummer = @rekeningnummer, IBAN = @iban, GebruikerId = @gebruikerId, RekeningType = @rekeningtype, Krediet = @krediet, Saldo = @saldo WHERE Rekeningnummer = @rekeningnummer";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@rekeningnummer", id);
            command.Parameters.AddWithValue("@iban", rekening.Iban);
            command.Parameters.AddWithValue("@gebruikerId", rekening.Gebruiker.Id);
            command.Parameters.AddWithValue("@rekeningtype", rekening.RekeningType.ToString());
            command.Parameters.AddWithValue("@krediet", rekening.KredietLimiet);
            command.Parameters.AddWithValue("@saldo", rekening.Saldo);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException("Er is een fout opgetreden bij het wijzigen van de rekening.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Rekening?> RekeningOphalenAsync(Guid id, int depth = 0)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "SELECT r.Rekeningnummer, r.IBAN, r.GebruikerId, r.RekeningType, r.Krediet, r.Saldo, r.TransactieId, r.is_visible, g.Familienaam, g.Voornaam, g.Email, g.Geboortedatum, g.Telefoonnummer, a.Straat, a.Huisnummer, a.Gemeente, a.Postcode, a.Land FROM Rekening r JOIN Gebruiker g on g.Id = r.GebruikerId JOIN Adres a on a.Id = g.AdresId WHERE Rekeningnummer = @rekeningnummer";
            // , t.Bedrag, t.Datum, t.RekeningnummerBegunstigde, t.TransactieType, t.Mededeling
            // LEFT JOIN Transactie t on t.Id = r.TransactieId
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@rekeningnummer", id);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var rekeningnummer = reader.GetGuid("Rekeningnummer");
                var iban = reader.GetString("Iban");
                var gebruikerId = reader.GetGuid("GebruikerId");
                var rekeningType = Enum.Parse<RekeningType>(reader.GetString("RekeningType"));
                var krediet = reader.GetDecimal("Krediet");
                var saldo = reader.GetDecimal("Saldo");
                var familienaam = reader.GetString("Familienaam");
                var voornaam = reader.GetString("Voornaam");
                var email = reader.GetString("Email");
                var geboortedatum = reader.GetDateTime("Geboortedatum");
                var telefoonnummer = reader.GetString("Telefoonnummer");
                var straat = reader.GetString("Straat");
                var huisnummer = reader.GetString("Huisnummer");
                var gemeente = reader.GetString("Gemeente");
                var postcode = reader.GetString("Postcode");
                var land = reader.GetString("Land");
                var adres = new Adres(straat, huisnummer, postcode, postcode, land);
                var gebruiker = new Gebruiker(familienaam, voornaam, email, telefoonnummer, geboortedatum, adres,
                    gebruikerId);
                var transacties = new List<Transactie>();
                if (depth > 0)
                {
                    transacties = await RekeningTransactiesOphalenAsync(rekeningnummer, depth);
                }
                var rekening = new Rekening(rekeningnummer, iban, rekeningType, krediet, saldo, transacties, gebruiker);
                return rekening;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException($"Er is een fout opgetreden bij het ophalen van de rekening voor {id.ToString()}", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Rekening?> RekeningOphalenViaEmailAsync(string email, int depth = 0)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "SELECT r.Rekeningnummer, r.IBAN, r.GebruikerId, r.RekeningType, r.Krediet, r.Saldo, r.TransactieId, r.is_visible, g.Familienaam, g.Voornaam, g.Email, g.Geboortedatum, g.Telefoonnummer, a.Straat, a.Huisnummer, a.Gemeente, a.Postcode, a.Land FROM Rekening r JOIN Gebruiker g on g.Id = r.GebruikerId JOIN Adres a on a.Id = g.AdresId WHERE g.Email = @email";
            // , t.Bedrag, t.Datum, t.RekeningnummerBegunstigde, t.TransactieType, t.Mededeling
            // LEFT JOIN Transactie t on t.Id = r.TransactieId
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@email", email);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var rekeningnummer = reader.GetGuid("Rekeningnummer");
                var iban = reader.GetString("Iban");
                var gebruikerId = reader.GetGuid("GebruikerId");
                var rekeningType = Enum.Parse<RekeningType>(reader.GetString("RekeningType"));
                var krediet = reader.GetDecimal("Krediet");
                var saldo = reader.GetDecimal("Saldo");
                var familienaam = reader.GetString("Familienaam");
                var voornaam = reader.GetString("Voornaam");
                var mail = reader.GetString("Email");
                var geboortedatum = reader.GetDateTime("Geboortedatum");
                var telefoonnummer = reader.GetString("Telefoonnummer");
                var straat = reader.GetString("Straat");
                var huisnummer = reader.GetString("Huisnummer");
                var gemeente = reader.GetString("Gemeente");
                var postcode = reader.GetString("Postcode");
                var land = reader.GetString("Land");
                var adres = new Adres(straat, huisnummer, postcode, postcode, land);
                var gebruiker = new Gebruiker(familienaam, voornaam, mail, telefoonnummer, geboortedatum, adres,
                    gebruikerId);
                var transacties = new List<Transactie>();
                if (depth > 0)
                {
                    transacties = await RekeningTransactiesOphalenAsync(rekeningnummer, depth);
                }
                var rekening = new Rekening(rekeningnummer, iban, rekeningType, krediet, saldo, transacties, gebruiker);
                return rekening;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException($"Er is een fout opgetreden bij het ophalen van de rekening voor {email}", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private async Task<List<Transactie>?> RekeningTransactiesOphalenAsync(Guid id, int depth)
    {
        var connection = new MySqlConnection(_connectionString);
        var rekeningnummer = Guid.Empty;
        var gebruikerId = Guid.Empty;
        var transactieIdOld = Guid.Empty;
        Transactie? t;
        Gebruiker g;
        var transacties = new List<Transactie>();
        try
        {
            // TODO: * kan problemen geven want volgorde kolommen niet zeker volgens standaard en want als er kolommen bijkomen of afvallen
            const string query = "SELECT * FROM transactie WHERE Rekeningnummerbegunstigde = @rekeningnummer ORDER BY Datum DESC LIMIT @depth";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@rekeningnummer", id);
            command.Parameters.AddWithValue("@depth", depth);
            await connection.OpenAsync();
            var adapter = new MySqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                var transactieId = Guid.Parse((string)row["Id"]);
                t = await _transactieRepository.TransactieOphalenAsync(transactieId);
                transacties.Add(t);
            }
            return transacties;
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException("Er is een fout opgetreden bij het ophalen van de rekening.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Rekening?> RekeningOphalenViaIBANAsync(string iban, int depth = 0)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "SELECT r.Rekeningnummer, r.IBAN, r.GebruikerId, r.RekeningType, r.Krediet, r.Saldo, r.TransactieId, r.is_visible, g.Familienaam, g.Voornaam, g.Email, g.Geboortedatum, g.Telefoonnummer, a.Straat, a.Huisnummer, a.Gemeente, a.Postcode, a.Land FROM Rekening r JOIN Gebruiker g on g.Id = r.GebruikerId JOIN Adres a on a.Id = g.AdresId WHERE g.IBAN = @iban";
            // , t.Bedrag, t.Datum, t.RekeningnummerBegunstigde, t.TransactieType, t.Mededeling
            // LEFT JOIN Transactie t on t.Id = r.TransactieId
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@iban", iban);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var rekeningnummer = reader.GetGuid("Rekeningnummer");
                var email = reader.GetString("Email");
                var gebruikerId = reader.GetGuid("GebruikerId");
                var rekeningType = Enum.Parse<RekeningType>(reader.GetString("RekeningType"));
                var krediet = reader.GetDecimal("Krediet");
                var saldo = reader.GetDecimal("Saldo");
                var familienaam = reader.GetString("Familienaam");
                var voornaam = reader.GetString("Voornaam");
                var mail = reader.GetString("Email");
                var geboortedatum = reader.GetDateTime("Geboortedatum");
                var telefoonnummer = reader.GetString("Telefoonnummer");
                var straat = reader.GetString("Straat");
                var huisnummer = reader.GetString("Huisnummer");
                var gemeente = reader.GetString("Gemeente");
                var postcode = reader.GetString("Postcode");
                var land = reader.GetString("Land");
                var adres = new Adres(straat, huisnummer, postcode, postcode, land);
                var gebruiker = new Gebruiker(familienaam, voornaam, mail, telefoonnummer, geboortedatum, adres,
                    gebruikerId);
                var transacties = new List<Transactie>();
                if (depth > 0)
                {
                    transacties = await RekeningTransactiesOphalenAsync(rekeningnummer, depth);
                }
                var rekening = new Rekening(rekeningnummer, iban, rekeningType, krediet, saldo, transacties, gebruiker);
                return rekening;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new RekeningRepositoryException($"Er is een fout opgetreden bij het ophalen van de rekening voor {iban}", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}