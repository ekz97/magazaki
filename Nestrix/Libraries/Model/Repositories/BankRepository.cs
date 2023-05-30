using DataLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;
using MySqlConnector;

namespace DataLayer.Repositories;

public class BankRepository : IBankRepository
{
    private readonly string _connectionString;

    public BankRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task BankToevoegenAsync(Bank bank)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "INSERT INTO Bank (Id, Naam, Telefoonnummer, AdresId, is_visible) VALUES (@id, @naam, @telefoonnummer, @adresId, @is_visible)";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", bank.Id);
            command.Parameters.AddWithValue("@naam", bank.Naam);
            command.Parameters.AddWithValue("@telefoonnummer", bank.Telefoonnummer);
            command.Parameters.AddWithValue("@adresId", bank.Adres.Id);
            command.Parameters.AddWithValue("@is_visible", 1);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new BankRepositoryException("Er is een fout opgetreden bij het aanmaken van de bank.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task BankVerwijderenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "UPDATE Bank SET is_visible = @is_visible WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@is_visible", 0);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new BankRepositoryException("Er is een fout opgetreden bij het verwijderen van de bank.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task BankWijzigenAsync(Guid id, Bank bank)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "UPDATE Bank SET Naam = @naam, Telefoonnummer = @telefoonnummer, AdresId = @adresId WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@naam", bank.Naam);
            command.Parameters.AddWithValue("@telefoonnummer", bank.Telefoonnummer);
            command.Parameters.AddWithValue("@adresId", bank.Adres.Id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new BankRepositoryException("Er is een fout opgetreden bij het wijzigen van de bank.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Bank?> BankOphalenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT b.Id, b.Naam, b.Telefoonnummer, a.Id as AdresId, a.Straat, a.Huisnummer,  a.Postcode, a.Gemeente, a.Land FROM Bank b JOIN Adres a ON b.AdresId = a.Id WHERE b.Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var bank = new Bank
                {
                    Id = reader.GetGuid(0),
                    Naam = reader.GetString(1),
                    Telefoonnummer = reader.GetString(2),
                    Adres = new Adres
                    {
                        Id = reader.GetGuid(3),
                        Straat = reader.GetString(4),
                        Huisnummer = reader.GetString(5),
                        Postcode = reader.GetString(6),
                        Gemeente = reader.GetString(7),
                        Land = reader.GetString(8)
                    }
                };
                return bank;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new BankRepositoryException("Er is een fout opgetreden bij het ophalen van de bank.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Bank?> BankOphalenAsync(string naam)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT b.Id, b.Naam, b.Telefoonnummer, a.Id as AdresId, a.Straat, a.Huisnummer,  a.Postcode, a.Gemeente, a.Land FROM Bank b JOIN Adres a ON b.AdresId = a.Id WHERE b.Naam = @naam";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@naam", naam);
            connection.Open();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var bank = new Bank
                {
                    Id = reader.GetGuid(0),
                    Naam = reader.GetString(1),
                    Telefoonnummer = reader.GetString(2),
                    Adres = new Adres
                    {
                        Id = reader.GetGuid(3),
                        Straat = reader.GetString(4),
                        Huisnummer = reader.GetString(5),
                        Postcode = reader.GetString(6),
                        Gemeente = reader.GetString(7),
                        Land = reader.GetString(8)
                    }
                };
                return bank;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            throw new BankRepositoryException("Er is een fout opgetreden bij het ophalen van de bank.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}