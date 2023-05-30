using LogicLayer.Exceptions;
using LogicLayer.Model;

namespace TestLayer.ModelTests;

public class AdresTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetStraat_NullOrWhiteSpace_AdresException(string straat)
    {
        var adres = new Adres();
        Assert.Throws<AdresException>(() => adres.ZetStraat(straat));
    }
    
    [Theory]
    [InlineData("Molenstraat")]
    [InlineData("Hillarestraat")]
    public void ZetStraat_ValidStraat_NoException(string straat)
    {
        var adres = new Adres();
        adres.ZetStraat(straat);
        Assert.Equal(straat, adres.Straat);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetHuisnummer_NullOrWhiteSpace_AdresException(string huisnummer)
    {
        var adres = new Adres();
        Assert.Throws<AdresException>(() => adres.ZetHuisnummer(huisnummer));
    }
    
    [Theory]
    [InlineData("1")]
    [InlineData("1A")]
    public void ZetHuisnummer_ValidHuisnummer_NoException(string huisnummer)
    {
        var adres = new Adres();
        adres.ZetHuisnummer(huisnummer);
        Assert.Equal(huisnummer, adres.Huisnummer);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetPostcode_NullOrWhiteSpace_AdresException(string postcode)
    {
        var adres = new Adres();
        Assert.Throws<AdresException>(() => adres.ZetPostcode(postcode));
    }
    
    [Theory]
    [InlineData("1000")]
    [InlineData("1000A")]
    public void ZetPostcode_ValidPostcode_NoException(string postcode)
    {
        var adres = new Adres();
        adres.ZetPostcode(postcode);
        Assert.Equal(postcode, adres.Postcode);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetGemeente_NullOrWhiteSpace_AdresException(string gemeente)
    {
        var adres = new Adres();
        Assert.Throws<AdresException>(() => adres.ZetGemeente(gemeente));
    }
    
    [Theory]
    [InlineData("Gent")]
    [InlineData("Lokeren")]
    public void ZetGemeente_ValidGemeente_NoException(string gemeente)
    {
        var adres = new Adres();
        adres.ZetGemeente(gemeente);
        Assert.Equal(gemeente, adres.Gemeente);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetLand_NullOrWhiteSpace_AdresException(string land)
    {
        var adres = new Adres();
        Assert.Throws<AdresException>(() => adres.ZetLand(land));
    }
    
    [Theory]
    [InlineData("België")]
    [InlineData("Nederland")]
    public void ZetLand_ValidLand_NoException(string land)
    {
        var adres = new Adres();
        adres.ZetLand(land);
        Assert.Equal(land, adres.Land);
    }
    
    // Constructor tests
    
    [Fact]
    public void Adres_ValidAdres_NoException()
    {
        var adres = new Adres("Straat", "1", "1000", "Gent", "België");
        Assert.NotEqual(Guid.Empty, adres.Id);
        Assert.Equal("Straat", adres.Straat);
        Assert.Equal("1", adres.Huisnummer);
        Assert.Equal("1000", adres.Postcode);
        Assert.Equal("Gent", adres.Gemeente);
        Assert.Equal("België", adres.Land);
    }
}