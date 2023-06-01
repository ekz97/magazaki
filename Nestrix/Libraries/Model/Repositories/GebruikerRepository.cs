using DataLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;
using MySqlConnector;

namespace DataLayer.Repositories;

public class GebruikerRepository : IGebruikerRepository
{
    private string _connectionString;

    public GebruikerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task GebruikerToevoegenAsync(Gebruiker gebruiker)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "INSERT INTO Gebruiker (Id, Familienaam, Voornaam,  Email, Geboortedatum, AdresId, Code, Telefoonnummer, is_visible) VALUES (@id, @familienaam, @voornaam, @email, @geboortedatum, @adresId, @code, @telefoonnummer, @is_visible)";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", gebruiker.Id);
            command.Parameters.AddWithValue("@voornaam", gebruiker.Voornaam);
            command.Parameters.AddWithValue("@familienaam", gebruiker.Familienaam);
            command.Parameters.AddWithValue("@email", gebruiker.Email);
            command.Parameters.AddWithValue("@geboortedatum", gebruiker.Geboortedatum);
            command.Parameters.AddWithValue("@adresId", gebruiker.Adres.Id);
            command.Parameters.AddWithValue("@telefoonnummer", gebruiker.Telefoonnummer);
            //var hashedCode = BCrypt.Net.BCrypt.HashPassword(gebruiker.Code);
            var hashedCode = PeasieLib.Services.EncryptionService.ToSHA256(gebruiker.Code);
            command.Parameters.AddWithValue("@code", hashedCode);
            command.Parameters.AddWithValue("@is_visible", 1);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new GebruikerRepositoryException("Er is een fout opgetreden bij het aanmaken van de gebruiker.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task GebruikerVerwijderenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "UPDATE Gebruiker SET is_visible = @is_visible WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@is_visible", 0);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new GebruikerRepositoryException("Er is een fout opgetreden bij het verwijderen van de gebruiker.",
                e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task GebruikerWijzigenAsync(Guid id, Gebruiker gebruiker)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "UPDATE Gebruiker SET Familienaam = @familienaam, Voornaam = @voornaam, Email = @email, Geboortedatum = @geboortedatum, AdresId = @adresId WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@voornaam", gebruiker.Voornaam);
            command.Parameters.AddWithValue("@familienaam", gebruiker.Familienaam);
            command.Parameters.AddWithValue("@email", gebruiker.Email);
            command.Parameters.AddWithValue("@geboortedatum", gebruiker.Geboortedatum);
            command.Parameters.AddWithValue("@adresId", gebruiker.Adres.Id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new GebruikerRepositoryException("Er is een fout opgetreden bij het wijzigen van de gebruiker.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Gebruiker?> GebruikerOphalenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT g.Id, g.Voornaam, g.Familienaam, g.Email, g.Geboortedatum, g.Code, g.Telefoonnummer, a.Straat, a.Huisnummer, a.Gemeente, a.Postcode, a.Land FROM Gebruiker g join Adres a on a.Id = g.AdresId WHERE g.Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var gebruiker = new Gebruiker
                {
                    Id = reader.GetGuid("Id"),
                    Voornaam = reader.GetString("Voornaam"),
                    Familienaam = reader.GetString("Familienaam"),
                    Email = reader.GetString("Email"),
                    Geboortedatum = reader.GetDateTime("Geboortedatum"),
                    HashedCode = reader.GetString("Code"),
                    Telefoonnummer = reader.GetString("Telefoonnummer"),
                    Adres = new Adres
                    {
                        Id = reader.GetGuid("AdresId"),
                        Straat = reader.GetString("Straat"),
                        Huisnummer = reader.GetString("Huisnummer"),
                        Gemeente = reader.GetString("Gemeente"),
                        Postcode = reader.GetString("Postcode"),
                        Land = reader.GetString("Land")
                    }
                };
                return gebruiker;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new GebruikerRepositoryException("Er is een fout opgetreden bij het ophalen van de gebruiker.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Gebruiker?> GebruikerOphalenAsync(string voornaam, string familienaam, string email)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT g.Id, g.Voornaam, g.Familienaam, g.Email, g.Geboortedatum, g.AdresId, a.Straat, a.Huisnummer, a.Gemeente, a.Postcode, a.Land FROM Gebruiker g join Adres a on a.Id = g.AdresId WHERE g.Voornaam = @voornaam AND g.Familienaam = @familienaam AND g.Email = @email";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@voornaam", voornaam);
            command.Parameters.AddWithValue("@familienaam", familienaam);
            command.Parameters.AddWithValue("@email", email);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var gebruiker = new Gebruiker
                {
                    Id = reader.GetGuid("Id"),
                    Voornaam = reader.GetString("Voornaam"),
                    Familienaam = reader.GetString("Familienaam"),
                    Email = reader.GetString("Email"),
                    Geboortedatum = reader.GetDateTime("Geboortedatum"),
                    Adres = new Adres
                    {
                        Id = reader.GetGuid("AdresId"),
                        Straat = reader.GetString("Straat"),
                        Huisnummer = reader.GetString("Huisnummer"),
                        Gemeente = reader.GetString("Gemeente"),
                        Postcode = reader.GetString("Postcode"),
                        Land = reader.GetString("Land")
                    }
                };
                return gebruiker;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new GebruikerRepositoryException("Er is een fout opgetreden bij het ophalen van de gebruiker.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Gebruiker?> GebruikerOphalenAsync(string email)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT g.Id, g.Voornaam, g.Familienaam, g.Email, g.Geboortedatum, g.AdresId, a.Straat, a.Huisnummer, a.Gemeente, a.Postcode, a.Land FROM Gebruiker g join Adres a on a.Id = g.AdresId WHERE g.Email = @email";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@email", email);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var gebruiker = new Gebruiker
                {
                    Id = reader.GetGuid("Id"),
                    Voornaam = reader.GetString("Voornaam"),
                    Familienaam = reader.GetString("Familienaam"),
                    Email = reader.GetString("Email"),
                    Geboortedatum = reader.GetDateTime("Geboortedatum"),
                    Adres = new Adres
                    {
                        Id = reader.GetGuid("AdresId"),
                        Straat = reader.GetString("Straat"),
                        Huisnummer = reader.GetString("Huisnummer"),
                        Gemeente = reader.GetString("Gemeente"),
                        Postcode = reader.GetString("Postcode"),
                        Land = reader.GetString("Land")
                    }
                };
                return gebruiker;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new GebruikerRepositoryException("Er is een fout opgetreden bij het ophalen van de gebruiker.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}