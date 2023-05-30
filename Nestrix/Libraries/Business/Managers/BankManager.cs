using LogicLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace LogicLayer.Managers;

public class BankManager
{
    private readonly IBankRepository _bankRepository;
    private readonly IAdresRepository _adresRepository;

    public BankManager(IBankRepository bankRepository, IAdresRepository adresRepository)
    {
        _bankRepository = bankRepository;
        _adresRepository = adresRepository;
    }

    public async Task BankToevoegenAsync(Bank bank)
    {
        try
        {
            if (bank == null)
            {
                throw new BankManagerException("Bank is leeg");
            }

            var bankDb = await _bankRepository.BankOphalenAsync(bank.Naam);
            if (bankDb != null)
            {
                throw new BankManagerException("Bank bestaat al");
            }

            var adres = await _adresRepository.AdresOphalenAsync(bank.Adres.Straat, bank.Adres.Huisnummer, bank.Adres.Postcode, bank.Adres.Gemeente, bank.Adres.Land);
            if (adres== null)
            {
                await _adresRepository.AdresToevoegenAsync(new Adres(bank.Adres.Straat, bank.Adres.Huisnummer, bank.Adres.Postcode, bank.Adres.Gemeente, bank.Adres.Land));
            }

            await _bankRepository.BankToevoegenAsync(bank);
        }
        catch (Exception e)
        {
            throw new BankManagerException("Er is een fout opgetreden bij het toevoegen van de bank.", e);
        }
    }

    public async Task BankWijzigenAsync(Guid id, Bank bank)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new BankManagerException("Id is leeg");
            }
            if (bank == null)
            {
                throw new BankManagerException("Bank is leeg");
            }

            var bankDb = await _bankRepository.BankOphalenAsync(id);
            if (bankDb == null)
            {
                throw new BankManagerException("Bank bestaat niet");
            }

            if (bankDb.Equals(bank))
            {
                throw new BankManagerException("Bank is niet gewijzigd.");
            }

            await _bankRepository.BankWijzigenAsync(id, bank);
        }
        catch (Exception e)
        {
            throw new BankManagerException("Er is een fout opgetreden bij het wijzigen van de bank.", e);
        }
    }

    public async Task BankVerwijderenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new BankManagerException("Id is leeg");
            }

            var bankDb = await _bankRepository.BankOphalenAsync(id);
            if (bankDb == null)
            {
                throw new BankManagerException("Bank bestaat niet");
            }

            await _bankRepository.BankVerwijderenAsync(id);
        }
        catch (Exception e)
        {
            throw new BankManagerException("Er is een fout opgetreden bij het verwijderen van de bank.", e);
        }
    }

    public async Task<Bank?> BankOphalenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new BankManagerException("Id is leeg");
            }

            var bank = await _bankRepository.BankOphalenAsync(id);
            if (bank == null)
            {
                throw new BankManagerException("Bank niet gevonden");
            }

            return bank;
        }
        catch (Exception e)
        {
            throw new BankManagerException("Er is een fout opgetreden bij het ophalen van de bank.", e);
        }
    }
}