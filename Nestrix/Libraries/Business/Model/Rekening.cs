using LogicLayer.Exceptions;

namespace LogicLayer.Model;

public class Rekening
{
    public Guid Rekeningnummer { get; set; }
    public string Iban { get; set; }
    public RekeningType RekeningType { get; set; }
    public decimal KredietLimiet { get; set; }
    public decimal Saldo { get; set; }
    public List<Transactie> Transacties { get; set; }
    public Gebruiker Gebruiker { get; set; }
    
    // Nieuwe rekening aanmaken
    public Rekening(RekeningType rekeningType, string iban, decimal kredietLimiet, Gebruiker? gebruiker = null)
    {
        Rekeningnummer = Guid.NewGuid();
        Iban = iban;
        ZetRekeningType(rekeningType);
        ZetKredietLimiet(kredietLimiet);
        if(gebruiker != null)
            ZetGebruiker(gebruiker);
        Saldo = 0;
        Transacties = new List<Transactie>();
    }
    
    // Rekening wijzigen
    public Rekening(Guid id, string iban, decimal kredietLimiet)
    {
        Rekeningnummer = id;
        Iban = iban;
        // RekeningType wijzigen toelaten?
        // ZetRekeningType(rekeningType);
        ZetKredietLimiet(kredietLimiet);
        // Gebruiker wijzigen toelaten?
    }

    // Rekening ophalen uit database// Controles zijn niet nodig
    public Rekening(Guid rekeningnummer, string iban, RekeningType rekeningType, decimal kredietLimiet, decimal saldo,
        List<Transactie> transacties, Gebruiker gebruiker)
    {
        Rekeningnummer = rekeningnummer;
        Iban = iban;
        RekeningType = rekeningType;
        KredietLimiet = kredietLimiet;
        Saldo = saldo;
        Transacties = transacties;
        Gebruiker = gebruiker;
    }
    
    // Test constructor
    public Rekening()
    {
    }
    
    public void ZetRekeningType(RekeningType rekeningType)
    {
        RekeningType = rekeningType;
    }
    
    public void ZetKredietLimiet(decimal kredietLimiet)
    {
        if (kredietLimiet < 0)
        {
            throw new RekeningException($"KredietLimiet {Gebruiker.Email} {Iban} ({RekeningType}) mag niet negatief zijn: {KredietLimiet} (saldo: {Saldo})");
        }
        
        KredietLimiet = kredietLimiet;
    }
    
    public void ZetGebruiker(Gebruiker gebruiker)
    {
        if (gebruiker == null)
        {
            throw new RekeningException("Gebruiker mag niet leeg zijn.");
        }
        
        Gebruiker = gebruiker;
    }
}