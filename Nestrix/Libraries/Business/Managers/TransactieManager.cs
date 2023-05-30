using System.Runtime.InteropServices.JavaScript;
using System.Transactions;
using LogicLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace LogicLayer.Managers;

public class TransactieManager
{
    private ITransactieRepository _transactieRepository;
    private IRekeningRepository _rekeningRepository;

    public TransactieManager(ITransactieRepository transactieRepository, IRekeningRepository rekeningRepository)
    {
        _transactieRepository = transactieRepository;
        _rekeningRepository = rekeningRepository;
    }

    public async Task TransactieToevoegenAsync(Transactie transactie)
    {
        try
        {
            if (transactie == null)
            {
                throw new TransactieManagerException("Transactie is leeg");
            }

            var transactieDb = await _transactieRepository.TransactieOphalenAsync(transactie.Id);
            if (transactieDb != null)
            {
                throw new TransactieManagerException("Transactie bestaat al");
            }

            await _transactieRepository.TransactieToevoegenAsync(transactie);
        }
        catch (Exception e)
        {
            throw new TransactieManagerException("Er ging iets fout bij het toevoegen van de transactie.", e);
        }
    }

    public async Task TransactieVerwijderenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new TransactieManagerException("Id is leeg");
            }

            var transactieDb = await _transactieRepository.TransactieOphalenAsync(id);
            if (transactieDb == null)
            {
                throw new TransactieManagerException("Transactie bestaat niet");
            }

            await _transactieRepository.TransactieVerwijderenAsync(id);
        }
        catch (Exception e)
        {
            throw new TransactieManagerException("Er ging iets fout bij het verwijderen van de transactie", e);
        }
    }

    public async Task TransactieWijzigenAsync(Guid id, Transactie transactie)
    {
        try
        {
          if (id == Guid.Empty)
            {
                throw new TransactieManagerException("Id is leeg");
            }
            if (transactie == null)
            {
                throw new TransactieManagerException("Transactie is leeg");
            }

            var transactieDb = await _transactieRepository.TransactieOphalenAsync(id);
            if (transactieDb == null)
            {
                throw new TransactieManagerException("Transactie bestaat niet");
            }

            if (transactie.Equals(transactieDb))
            {
                throw new TransactieManagerException("Transactie is niet gewijzigd");
            }

            await _transactieRepository.TransactieWijzigenAsync(id, transactie);
        }
        catch (Exception e)
        {
            throw new TransactieManagerException("Er ging iets fout bij het wijzigen van de transactie", e);
        }
    }

    public async Task<Transactie?> TransactieOphalenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new TransactieManagerException("Id is leeg");
            }

            var transactieDb = await _transactieRepository.TransactieOphalenAsync(id);
            if (transactieDb == null)
            {
                throw new TransactieManagerException("Transactie bestaat niet");
            }

            return transactieDb;
        }
        catch (Exception e)
        {
            throw new TransactieManagerException("Er ging iets fout bij het ophalen van de transactie", e);
        }
    }
}