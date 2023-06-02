using System.Transactions;
using LogicLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace LogicLayer.Managers;

public class RekeningManager
{
    private readonly IRekeningRepository _rekeningRepository;
    private readonly ITransactieRepository _transactieRepository;

    public RekeningManager(IRekeningRepository rekeningRepository, ITransactieRepository transactieRepository)
    {
        _rekeningRepository = rekeningRepository;
        _transactieRepository = transactieRepository;
    }

    public async Task RekeningToevoegenAsync(Rekening rekening)
    {
        try
        {
            if (rekening == null)
            {
                throw new RekeningManagerException("Rekening is leeg");
            }

            var rekeningDb = await _rekeningRepository.RekeningOphalenAsync(rekening.Rekeningnummer, 0);
            if (rekeningDb != null)
            {
                throw new RekeningManagerException("Rekening bestaat al");
            }

            await _rekeningRepository.RekeningToevoegenAsync(rekening);
        }
        catch (Exception e)
        {
            throw new RekeningManagerException("Er is een fout opgetreden bij het toevoegen van de rekening.", e);
        }
    }

    public async Task RekeningVerwijderenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new RekeningManagerException("Id is leeg");
            }

            var rekeningDb = await _rekeningRepository.RekeningOphalenAsync(id, 0);
            if (rekeningDb == null)
            {
                throw new RekeningManagerException("Rekening bestaat niet");
            }

            await _rekeningRepository.RekeningVerwijderenAsync(id);
        }
        catch (Exception e)
        {
            throw new RekeningManagerException("Er is een fout opgetreden bij het verwijderen van de rekening.", e);
        }
    }

    public async Task RekeningWijzigenAsync(Guid id, Rekening rekening)
    {
        if (id == Guid.Empty)
        {
            throw new RekeningManagerException("Id is leeg");
        }

        if (rekening == null)
        {
            throw new RekeningManagerException("Rekening is leeg");
        }
        try
        {
            var rekeningDb = await _rekeningRepository.RekeningOphalenAsync(rekening.Rekeningnummer, 0) ?? throw new RekeningManagerException("Rekening bestaat niet");
            if (rekeningDb.Equals(rekening))
            {
                Console.WriteLine("Rekening is niet gewijzigd.");
            }

            await _rekeningRepository.RekeningWijzigenAsync(id, rekening);
        }
        catch (Exception e)
        {
            throw new RekeningManagerException("Er is een fout opgetreden bij het wijzigen van de rekening.", e);
        }
    }

    public async Task<Rekening?> RekeningOphalenAsync(Guid id, int depth = 0)
    {
        if (id == Guid.Empty)
        {
            throw new RekeningManagerException("Id is leeg");
        }
        try
        {
            var rekeningDb = await _rekeningRepository.RekeningOphalenAsync(id, 0) ?? throw new RekeningManagerException("Rekening bestaat niet");
            return await _rekeningRepository.RekeningOphalenAsync(id, depth);
        }
        catch (Exception e)
        {
            throw new RekeningManagerException("Er is een fout opgetreden bij het ophalen van de rekening.", e);
        }
    }

    public async Task<Rekening?> RekeningOphalenViaEmailAsync(string email, int depth = 0)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new RekeningManagerException("Email is niet opgegeven");
        }
        try
        {
            var rekeningDb = await _rekeningRepository.RekeningOphalenViaEmailAsync(email, 0) ?? throw new RekeningManagerException("Rekening bestaat niet");
            return await _rekeningRepository.RekeningOphalenViaEmailAsync(email, depth);
        }
        catch (Exception e)
        {
            throw new RekeningManagerException($"Er is een fout opgetreden bij het ophalen van de rekening voor {email}", e);
        }
    }

    public async Task<bool> TransferMoneyAsync(Rekening from, Rekening to, decimal amount, string? mededeling = null)
    {
        // Je moet iets kunnen storten op een vreemd rekeningnummer...
        var date = DateTime.Now;
        if (from == null /*|| to == null*/)
        {
            throw new TransactieManagerException("Rekening is onbekend");
        }

        if (amount <= 0)
        {
            throw new TransactieManagerException("Bedrag is niet zinvol");
        }
        if (from.Saldo - amount < -from.KredietLimiet)
        {
            throw new TransactieManagerException("Saldo is te laag");
        }
        if (from.RekeningType == RekeningType.Spaarrekening && to.Gebruiker.Id != from.Gebruiker.Id)
        {
            throw new TransactieManagerException("Spaarrekening kan alleen naar eigen rekening overmaken");
        }

        var transactie = new Transactie(amount, mededeling, from, TransactieType.Uitgaand)
        {
            Datum = date
        };

        var transactieOntvangen = new Transactie(amount, mededeling, to, TransactieType.Inkomend)
        {
            Datum = date
        };

        using var transaction = new TransactionScope();
        try
        {
            PerformTransfer(from, to, amount, transactie, transactieOntvangen);
            await _transactieRepository.TransactieToevoegenAsync(transactie);
            await _transactieRepository.TransactieToevoegenAsync(transactieOntvangen);
            await RekeningWijzigenAsync(from.Rekeningnummer, from);
            await RekeningWijzigenAsync(to.Rekeningnummer, to);
            transaction.Complete();
            return true;
        }
        catch (Exception e)
        {
            transaction.Dispose();
            throw new TransactieManagerException("Er ging iets fout bij het uitvoeren van de transactie", e);
        }
        //return false;
    }

    private static void PerformTransfer(Rekening from, Rekening to, decimal amount, Transactie transactie,
        Transactie transactieOntvangen)
    {
        try
        {
            from.Saldo -= amount;
            to.Saldo += amount;
            from.Transacties.Add(transactie);
            to.Transacties.Add(transactieOntvangen);
        }
        catch (Exception e)
        {
            throw new TransactieManagerException("Er ging iets fout bij het uitvoeren van de transactie", e);
        }
    }
}

// TODO: saldo!?