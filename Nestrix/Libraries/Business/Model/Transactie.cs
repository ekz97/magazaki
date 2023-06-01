using LogicLayer.Exceptions;

namespace LogicLayer.Model;

public class Transactie
{
    public Guid Id { get; set; }
    public DateTime Datum { get; set; }
    public decimal Bedrag { get; set; }
    public string? Mededeling { get; set; }
    public Rekening Rekening { get; set; }
    public TransactieType TransactieType { get; set; }
    
    // Nieuwe transactie aanmaken
    public Transactie(decimal bedrag, string? mededeling, Rekening rekening, TransactieType transactieType)
    {
        Id = Guid.NewGuid();
        ZetBedrag(bedrag);
        ZetMededeling(mededeling);
        ZetRekening(rekening);
        ZetTransactieType(transactieType);
    }

    // Transactie uit database halen// Controles zijn niet nodig
    public Transactie(Guid id, DateTime datum, decimal bedrag, string? mededeling, Rekening rekening,
        TransactieType transactieType)
    {
        Id = id;
        Datum = datum;
        Bedrag = bedrag;
        Mededeling = mededeling;
        Rekening = rekening;
        TransactieType = transactieType;
    }
    
    // Test constructor
    public Transactie()
    {
    }
    
    public void ZetBedrag(decimal bedrag)
    {
        // Wat met transactieType?
        if (bedrag < 0)
        {
            throw new TransactieException("Bedrag mag niet negatief zijn.");
        }
        
        Bedrag = bedrag;
    }
    
    public void ZetMededeling(string mededeling)
    {
        Mededeling = mededeling;
    }
    
    public void ZetRekening(Rekening rekening)
    {
        if (rekening == null)
        {
            throw new TransactieException("Rekening mag niet leeg zijn.");
        }
        
        Rekening = rekening;
    }
    
    public void ZetTransactieType(TransactieType transactieType)
    {
        TransactieType = transactieType;
    }
}