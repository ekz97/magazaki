using LogicLayer.Exceptions;

namespace LogicLayer.Model;

public class Adres
{
    public Guid? Id { get; set; }
    public string Straat { get; set; }
    public string Huisnummer { get; set; }
    public string Postcode { get; set; }
    public string Gemeente { get; set; }
    public string Land { get; set; }
    
    public Adres(string straat, string huisnummer, string postcode, string gemeente, string land)
    {
        Id = Guid.NewGuid();
        ZetStraat(straat);
        ZetHuisnummer(huisnummer);
        ZetPostcode(postcode);
        ZetGemeente(gemeente);
        ZetLand(land);
    }

    // DTO constructor
    public Adres(Guid id, string straat, string huisnummer, string postcode, string gemeente, string land)
    {
        Id = id;
        ZetStraat(straat);
        ZetHuisnummer(huisnummer);
        ZetPostcode(postcode);
        ZetGemeente(gemeente);
        ZetLand(land);
    }
    
    // Test constructor
    public Adres()
    {
    }
    
    public void ZetStraat(string straat)
    {
        if (string.IsNullOrWhiteSpace(straat))
        {
            throw new AdresException("Straat mag niet leeg zijn.");
        }
        
        Straat = straat;
    }
    
    public void ZetHuisnummer(string huisnummer)
    {
        if (string.IsNullOrWhiteSpace(huisnummer))
        {
            throw new AdresException("Huisnummer mag niet leeg zijn.");
        }
        
        Huisnummer = huisnummer;
    }
    
    public void ZetPostcode(string postcode)
    {
        if (string.IsNullOrWhiteSpace(postcode))
        {
            throw new AdresException("Postcode mag niet leeg zijn.");
        }
        
        Postcode = postcode;
    }
    
    public void ZetGemeente(string gemeente)
    {
        if (string.IsNullOrWhiteSpace(gemeente))
        {
            throw new AdresException("Gemeente mag niet leeg zijn.");
        }
        
        Gemeente = gemeente;
    }
    
    public void ZetLand(string land)
    {
        if (string.IsNullOrWhiteSpace(land))
        {
            throw new AdresException("Land mag niet leeg zijn.");
        }
        
        Land = land;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Adres)obj;

        return Straat == other.Straat &&
               Huisnummer == other.Huisnummer &&
               Postcode == other.Postcode &&
               Gemeente == other.Gemeente &&
               Land == other.Land;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Straat, Huisnummer, Postcode, Gemeente, Land);
    }
}