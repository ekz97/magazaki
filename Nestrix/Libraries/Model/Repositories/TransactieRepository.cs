using DataLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;
using MySqlConnector;

namespace DataLayer.Repositories;

public class TransactieRepository : ITransactieRepository
{
    private string _connectionString;

    public TransactieRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task TransactieToevoegenAsync(Transactie transactie)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "INSERT INTO Transactie (Id, Datum, Bedrag, Mededeling, RekeningnummerBegunstigde, TransactieType, is_visible) VALUES (@id, @datum, @bedrag, @mededeling, @rekeningId, @transactieType, @is_visible)";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", transactie.Id);
            command.Parameters.AddWithValue("@datum", transactie.Datum);
            command.Parameters.AddWithValue("@bedrag", transactie.Bedrag);
            command.Parameters.AddWithValue("@mededeling", transactie.Mededeling);
            command.Parameters.AddWithValue("@rekeningId", transactie.Rekening.Rekeningnummer);
            command.Parameters.AddWithValue("@transactieType", transactie.TransactieType.ToString());
            command.Parameters.AddWithValue("@is_visible", 1);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new TransactieRepositoryException("Er is een fout opgetreden bij het toevoegen van de transactie.",
                e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task TransactieVerwijderenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query = "UPDATE Transactie SET is_visible = @is_visible WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@is_visible", 0);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new TransactieRepositoryException("Er is een fout opgetreden bij het verwijderen van de transactie.",
                e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task TransactieWijzigenAsync(Guid id, Transactie transactie)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "UPDATE Transactie SET Datum = @datum, Bedrag = @bedrag, Mededeling = @mededeling, RekeningnummerBegunstigde = @rekeningId, TransactieType = @transactieType WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@datum", transactie.Datum);
            command.Parameters.AddWithValue("@bedrag", transactie.Bedrag);
            command.Parameters.AddWithValue("@mededeling", transactie.Mededeling);
            command.Parameters.AddWithValue("@rekeningId", transactie.Rekening.Rekeningnummer);
            command.Parameters.AddWithValue("@transactieType", transactie.TransactieType.ToString());
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new TransactieRepositoryException("Er is een fout opgetreden bij het wijzigen van de transactie.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Transactie?> TransactieOphalenAsync(Guid id)
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            const string query =
                "SELECT * FROM Transactie WHERE Id = @id";
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            await connection.OpenAsync();
            await using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                var tId = reader.GetGuid("Id");
                var datum = reader.GetDateTime("Datum");
                var bedrag = reader.GetDecimal("Bedrag");
                var mededeling = reader.GetString("Mededeling");
                var rekeningId = reader.GetGuid("RekeningnummerBegunstigde");
                // Convert transactieType to TransactieType
                var transactieType = Enum.Parse<TransactieType>(reader.GetString("TransactieType"));
                var rekening = await new RekeningRepository(_connectionString).RekeningOphalenAsync(rekeningId, 0);
                var transactie = new Transactie(tId, datum, bedrag, mededeling, rekening, transactieType);
                return transactie;
            }
            else
            {
                throw new TransactieRepositoryException("Er is geen transactie gevonden met het opgegeven id.");
            }
        }
        catch (Exception e)
        {
            throw new TransactieRepositoryException("Er is een fout opgetreden bij het ophalen van de transactie.", e);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}