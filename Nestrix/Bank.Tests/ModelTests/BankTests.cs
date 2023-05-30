using DataLayer.Repositories;
using LogicLayer.Exceptions;
using LogicLayer.Managers;
using LogicLayer.Model;

namespace TestLayer.ModelTests;

public class BankTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetNaam_NullOrWhiteSpace_BankException(string naam)
    {
        var bank = new Bank();
        Assert.Throws<BankException>(() => bank.ZetNaam(naam));
    }
    
    [Theory]
    [InlineData("Bank")]
    [InlineData("Bank 1")]
    public void ZetNaam_ValidNaam_NoException(string naam)
    {
        var bank = new Bank();
        bank.ZetNaam(naam);
        Assert.Equal(naam, bank.Naam);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ZetTelefoonnummer_NullOrWhiteSpace_BankException(string telefoonnummer)
    {
        var bank = new Bank();
        Assert.Throws<BankException>(() => bank.ZetTelefoonnummer(telefoonnummer));
    }
    
    [Theory]
    [InlineData("123456789")]
    [InlineData("1234567890")]
    public void ZetTelefoonnummer_ValidTelefoonnummer_NoException(string telefoonnummer)
    {
        var bank = new Bank();
        bank.ZetTelefoonnummer(telefoonnummer);
        Assert.Equal(telefoonnummer, bank.Telefoonnummer);
    }
    
    [Fact]
    public void ZetAdres_Null_BankException()
    {
        var bank = new Bank();
        Assert.Throws<BankException>(() => bank.ZetAdres(null));
    }
    
    [Fact]
    public void ZetAdres_ValidAdres_NoException()
    {
        var bank = new Bank();
        var adres = new Adres("Straat", "1", "1000", "Gent", "België");
        bank.ZetAdres(adres);
        Assert.Equal(adres, bank.Adres);
    }
    
    [Fact]
    public void ZetGebruikers_Null_BankException()
    {
        var bank = new Bank();
        Assert.Throws<BankException>(() => bank.ZetGebruikers(null));
    }
    
    [Fact]
    public void ZetGebruikers_ValidGebruikers_NoException()
    {
        var bank = new Bank();
        var gebruikers = new List<Gebruiker>();
        bank.ZetGebruikers(gebruikers);
        Assert.Equal(gebruikers, bank.Gebruikers);
    }
    
    // Constructor tests
    // [Fact]
    // public void Bank_Constructor_NoException()
    // {
    //     // Arrange
    //     var naam = "Test Bank";
    //     var adres = new Adres("Hillarestraat", "12", "9160", "Lokeren", "België");
    //     var telefoonnummer = "1234567890";
    //     var gebruikers = new List<Gebruiker>();
    //
    //     // Act
    //     var bank = new Bank(naam, adres, telefoonnummer, gebruikers);
    //
    //     // Assert
    //     Assert.NotEqual(Guid.Empty, bank.Id);
    //     Assert.Equal(naam, bank.Naam);
    //     Assert.Equal(adres, bank.Adres);
    //     Assert.Equal(telefoonnummer, bank.Telefoonnummer);
    //     Assert.Equal(gebruikers, bank.Gebruikers);
    // }
}