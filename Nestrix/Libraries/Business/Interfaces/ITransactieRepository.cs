using LogicLayer.Model;

namespace LogicLayer.Interfaces;

public interface ITransactieRepository
{
    Task TransactieToevoegenAsync(Transactie transactie);
    Task TransactieVerwijderenAsync(Guid id);
    Task TransactieWijzigenAsync(Guid id, Transactie transactie);
    Task<Transactie?> TransactieOphalenAsync(Guid id);
}