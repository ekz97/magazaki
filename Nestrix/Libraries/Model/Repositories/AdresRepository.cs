using DataLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;
using MySqlConnector;

namespace DataLayer.Repositories;

public class AdresRepository : IAdresRepository
{
    private readonly string _connectionString;

    public AdresRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AdresToevoegenAsync(Adres adres)
    {
        await using var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "INSERT INTO Adres (Id, Straat, Huisnummer, Postcode, Gemeente, Land, is_visible) VALUES (@id, @straat, @huisnummer, @postcode, @gemeente, @land, @is_visible)";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", adres.Id);
            command.Parameters.AddWithValue("@straat", adres.Straat);
            command.Parameters.AddWithValue("@huisnummer", adres.Huisnummer);
            command.Parameters.AddWithValue("@postcode", adres.Postcode);
            command.Parameters.AddWithValue("@gemeente", adres.Gemeente);
            command.Parameters.AddWithValue("@land", adres.Land);
            command.Parameters.AddWithValue("@is_visible", 1);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new AdresRepositoryException("Er is een fout opgetreden bij het toevoegen van het adres.", e);
        }
    }

    public async Task AdresVerwijderenAsync(Guid id)
    {
        await using var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "UPDATE Adres SET is_visible = @is_visible WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@is_visible", 0);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new AdresRepositoryException("Er is een fout opgetreden bij het verwijderen van het adres.", e);
        }
    }

    public async Task AdresWijzigenAsync(Guid id, Adres adres)
    {
        await using var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "UPDATE Adres SET Straat = @straat, Huisnummer = @huisnummer, Postcode = @postcode, Gemeente = @gemeente, Land = @land WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@straat", adres.Straat);
            command.Parameters.AddWithValue("@huisnummer", adres.Huisnummer);
            command.Parameters.AddWithValue("@postcode", adres.Postcode);
            command.Parameters.AddWithValue("@gemeente", adres.Gemeente);
            command.Parameters.AddWithValue("@land", adres.Land);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new AdresRepositoryException("Er is een fout opgetreden bij het wijzigen van het adres.", e);
        }
    }

    public async Task<Adres?> AdresOphalenAsync(Guid id)
    {
        await using var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "SELECT * FROM Adres WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Adres
                {
                    Id = reader.GetGuid(0),
                    Straat = reader.GetString(1),
                    Huisnummer = reader.GetString(2),
                    Postcode = reader.GetString(3),
                    Gemeente = reader.GetString(4),
                    Land = reader.GetString(5)
                };
            }

            return null;
        }
        catch (Exception e)
        {
            throw new AdresRepositoryException("Er is een fout opgetreden bij het ophalen van het adres.", e);
        }
    }

    public async Task<Adres?> AdresOphalenAsync(string straat, string huisnummer, string postcode, string gemeente,
        string land)
    {
        await using var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT * FROM Adres WHERE Straat = @straat AND Huisnummer = @huisnummer AND Postcode = @postcode AND Gemeente = @gemeente AND Land = @land";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@straat", straat);
            command.Parameters.AddWithValue("@huisnummer", huisnummer);
            command.Parameters.AddWithValue("@postcode", postcode);
            command.Parameters.AddWithValue("@gemeente", gemeente);
            command.Parameters.AddWithValue("@land", land);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Adres
                {
                    Id = reader.GetGuid(0),
                    Straat = reader.GetString(1),
                    Huisnummer = reader.GetString(2),
                    Postcode = reader.GetString(3),
                    Gemeente = reader.GetString(4),
                    Land = reader.GetString(5)
                };
            }

            return null;
        }
        catch (Exception e)
        {
            throw new AdresRepositoryException("Er is een fout opgetreden bij het ophalen van het adres.", e);
        }
    }
}