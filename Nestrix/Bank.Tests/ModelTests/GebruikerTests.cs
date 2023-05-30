// using LogicLayer.Exceptions;
// using LogicLayer.Model;
//
// namespace TestLayer.ModelTests;
//
// public class GebruikerTests
// {
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public void ZetFamilienaam_NullOrWhiteSpace_GebruikerException(string familienaam)
//     {
//         var gebruiker = new Gebruiker();
//         Assert.Throws<GebruikerException>(() => gebruiker.ZetFamilienaam(familienaam));
//     }
//
//     [Theory]
//     [InlineData("Janssens")]
//     [InlineData("Voskes")]
//     public void ZetFamilienaam_ValidFamilienaam_NoException(string gebruikersnaam)
//     {
//         var gebruiker = new Gebruiker();
//         gebruiker.ZetFamilienaam(gebruikersnaam);
//         Assert.Equal(gebruikersnaam, gebruiker.Familienaam);
//     }
//     
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public void ZetVoornaam_NullOrWhiteSpace_GebruikerException(string voornaam)
//     {
//         var gebruiker = new Gebruiker();
//         Assert.Throws<GebruikerException>(() => gebruiker.ZetVoornaam(voornaam));
//     }
//     
//     [Theory]
//     [InlineData("Jan")]
//     [InlineData("Jef")]
//     public void ZetVoornaam_ValidVoornaam_NoException(string voornaam)
//     {
//         var gebruiker = new Gebruiker();
//         gebruiker.ZetVoornaam(voornaam);
//         Assert.Equal(voornaam, gebruiker.Voornaam);
//     }
//     
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public void ZetEmail_NullOrWhiteSpace_GebruikerException(string email)
//     {
//         var gebruiker = new Gebruiker();
//         Assert.Throws<GebruikerException>(() => gebruiker.ZetEmail(email));
//     }
//     
//     [Theory]
//     [InlineData("janjanssens@test.be")]
//     [InlineData("jefvoskens@test.be")]
//     public void ZetEmail_ValidEmail_NoException(string email)
//     {
//         var gebruiker = new Gebruiker();
//         gebruiker.ZetEmail(email);
//         Assert.Equal(email, gebruiker.Email);
//     }
//     
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public void ZetTelefoonnummer_NullOrWhiteSpace_GebruikerException(string telefoonnummer)
//     {
//         var gebruiker = new Gebruiker();
//         Assert.Throws<GebruikerException>(() => gebruiker.ZetTelefoonnummer(telefoonnummer));
//     }
//     
//     [Theory]
//     [InlineData("0123456789")]
//     [InlineData("012345678")]
//     public void ZetTelefoonnummer_ValidTelefoonnummer_NoException(string telefoonnummer)
//     {
//         var gebruiker = new Gebruiker();
//         gebruiker.ZetTelefoonnummer(telefoonnummer);
//         Assert.Equal(telefoonnummer, gebruiker.Telefoonnummer);
//     }
//     
//     [Theory]
//     [InlineData("")]
//     [InlineData(" ")]
//     public void ZetCode_NullOrWhiteSpace_GebruikerException(string code)
//     {
//         var gebruiker = new Gebruiker();
//         Assert.Throws<GebruikerException>(() => gebruiker.ZetCode(code));
//     }
//     
//     [Theory]
//     [InlineData("1234")]
//     [InlineData("12345")]
//     public void ZetCode_ValidCode_NoException(string code)
//     {
//         var gebruiker = new Gebruiker();
//         gebruiker.ZetCode(code);
//         Assert.Equal(code, gebruiker.Code);
//     }
//     
//     [Theory]
//     [InlineData("01/01/2000")]
//     [InlineData("01/01/2001")]
//     public void ZetGeboortedatum_ValidGeboortedatum_NoException(string geboortedatum)
//     {
//         var gebruiker = new Gebruiker();
//         gebruiker.ZetGeboortedatum(DateTime.Parse(geboortedatum));
//         Assert.Equal(DateTime.Parse(geboortedatum), gebruiker.Geboortedatum);
//     }
//     
//     [Fact]
//     public void ZetAdres_Null_GebruikerException()
//     {
//         var gebruiker = new Gebruiker();
//         Assert.Throws<GebruikerException>(() => gebruiker.ZetAdres(null));
//     }
//     
//     [Fact]
//     public void ZetAdres_ValidAdres_NoException()
//     {
//         var gebruiker = new Gebruiker();
//         var adres = new Adres("Hillarestraat", "12", "9160", "Lokeren", "België");
//         gebruiker.ZetAdres(adres);
//         Assert.Equal(adres, gebruiker.Adres);
//     }
//     
//     // Constructor tests
//     [Fact]
//     public void Gebruiker_Constructor_NoException()
//     {
//         string familienaam = "Janssens";
//         string voornaam = "Jan";
//         string email = "janjanssens@test.be";
//         string telefoonnummer = "0123456789";
//         string code = "1234";
//         DateTime geboortedatum = DateTime.Parse("01/01/2000");
//         var adres = new Adres("Hillarestraat", "12", "9160", "Lokeren", "België");
//         var gebruiker = new Gebruiker(familienaam, voornaam, email, telefoonnummer, code, geboortedatum, adres);
//         Assert.NotEqual(Guid.Empty, gebruiker.Id);
//         Assert.Equal(familienaam, gebruiker.Familienaam);
//         Assert.Equal(voornaam, gebruiker.Voornaam);
//         Assert.Equal(email, gebruiker.Email);
//         Assert.Equal(telefoonnummer, gebruiker.Telefoonnummer);
//         Assert.Equal(code, gebruiker.Code);
//         Assert.Equal(geboortedatum, gebruiker.Geboortedatum);
//         Assert.Equal(adres, gebruiker.Adres);
//     }
// }