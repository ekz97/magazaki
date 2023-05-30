using LogicLayer.Model;

namespace LogicLayer.Interfaces;

public interface IAdresRepository
{
    Task AdresToevoegenAsync(Adres adres);
    Task AdresVerwijderenAsync(Guid id);
    Task AdresWijzigenAsync(Guid id, Adres adres);
    Task<Adres?> AdresOphalenAsync(Guid id);
    Task<Adres?> AdresOphalenAsync(string straat, string huisnummer, string postcode, string gemeente, string land);
}