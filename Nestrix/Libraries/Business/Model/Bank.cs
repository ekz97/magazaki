using LogicLayer.Exceptions;

namespace LogicLayer.Model;

public class Bank
{
    public Guid Id { get; set; }
    public string Naam { get; set; }
    public string Telefoonnummer { get; set; }
    public Adres Adres { get; set; }
    public List<Gebruiker> Gebruikers { get; set; }

    // Nieuw bankobject aanmaken
    public Bank(string naam, Adres adres, string telefoonnummer)
    {
        Id = Guid.NewGuid();
        ZetNaam(naam);
        ZetAdres(adres);
        ZetTelefoonnummer(telefoonnummer);
        Gebruikers = new List<Gebruiker>();
    }
    
    // Bank wijzigen
    public Bank(Guid id, string naam, Adres adres, string telefoonnummer)
    {
        Id = id;
        ZetNaam(naam);
        ZetAdres(adres);
        ZetTelefoonnummer(telefoonnummer);
    }
    
    // Bank ophalen uit database// Controles zijn niet nodig
    public Bank(Guid id, Adres adres, string naam, string telefoonnummer, List<Gebruiker> gebruikers)
    {
        Id = id;
        Adres = adres;
        Naam = naam;
        Telefoonnummer = telefoonnummer;
        Gebruikers = gebruikers;
    }

    // Test constructor
    public Bank()
    {
    }

    public void ZetNaam(string naam)
    {
        if (string.IsNullOrWhiteSpace(naam))
        {
            throw new BankException("Naam mag niet leeg zijn.");
        }

        Naam = naam;
    }

    public void ZetTelefoonnummer(string telefoonnummer)
    {
        if (string.IsNullOrWhiteSpace(telefoonnummer))
        {
            throw new BankException("Telefoonnummer mag niet leeg zijn.");
        }

        Telefoonnummer = telefoonnummer;
    }

    public void ZetAdres(Adres adres)
    {
        if (adres == null)
        {
            throw new BankException("Adres mag niet leeg zijn.");
        }

        Adres = adres;
    }

    public void ZetGebruikers(List<Gebruiker> gebruikers)
    {
        if (gebruikers == null)
        {
            throw new BankException("Gebruikers mag niet leeg zijn.");
        }

        Gebruikers = gebruikers;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Bank)obj;

        return Naam == other.Naam && Telefoonnummer == other.Telefoonnummer && Adres == other.Adres;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Naam, Telefoonnummer, Adres);
    }
}