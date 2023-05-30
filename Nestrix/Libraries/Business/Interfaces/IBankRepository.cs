using LogicLayer.Model;

namespace LogicLayer.Interfaces;

public interface IBankRepository
{
    Task BankToevoegenAsync(Bank bank);
    Task BankVerwijderenAsync(Guid id);
    Task BankWijzigenAsync(Guid id, Bank bank);
    Task<Bank?> BankOphalenAsync(Guid id);
    Task<Bank?> BankOphalenAsync(string naam);
}