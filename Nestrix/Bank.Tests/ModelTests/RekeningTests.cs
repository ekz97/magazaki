// using System.Runtime.InteropServices;
// using LogicLayer.Exceptions;
// using LogicLayer.Model;
//
// namespace TestLayer.ModelTests;
//
// public class RekeningTests
// {
//     [Theory]
//     [InlineData(RekeningType.Spaarrekening)]
//     [InlineData(RekeningType.Zichtrekening)]
//     public void ZetRekeningType_ValidRekeningType_NoException(RekeningType rekeningType)
//     {
//         var rekening = new Rekening();
//         rekening.ZetRekeningType(rekeningType);
//         Assert.Equal(rekeningType, rekening.RekeningType);
//     }
//
//     [Theory]
//     [InlineData(-1.00)]
//     [InlineData(-200.00)]
//     public void ZetKredietLimiet_NegativeKredietLimiet_RekeningException(decimal kredietLimiet)
//     {
//         var rekening = new Rekening();
//         Assert.Throws<RekeningException>(() => rekening.ZetKredietLimiet(kredietLimiet));
//     }
//
//     [Theory]
//     [InlineData(0.00)]
//     [InlineData(200.00)]
//     public void ZetKredietLimiet_ValidKredietLimiet_NoException(decimal kredietLimiet)
//     {
//         var rekening = new Rekening();
//         rekening.ZetKredietLimiet(kredietLimiet);
//         Assert.Equal(kredietLimiet, rekening.KredietLimiet);
//     }
//
//     [Fact]
//     public void ZetGebruiker_NullGebruiker_RekeningException()
//     {
//         var rekening = new Rekening();
//         Assert.Throws<RekeningException>(() => rekening.ZetGebruiker(null));
//     }
//
//     [Fact]
//     public void ZetGebruiker_ValidGebruiker_NoException()
//     {
//         var rekening = new Rekening();
//         var gebruiker = new Gebruiker("Janssens", "Jan", "janjanssens@test.be", "012345678", "1234", DateTime.Now,
//             new Adres());
//         rekening.ZetGebruiker(gebruiker);
//         Assert.Equal(gebruiker, rekening.Gebruiker);
//     }
//
//     // Constructor tests
//     
//     [Fact]
//     public void Rekening_Constructor_NoException()
//     {
//         var rekeningType = RekeningType.Spaarrekening;
//         var kredietLimiet = 0.00m;  
//         var gebruiker = new Gebruiker("Janssens", "Jan", "janjanssens@test.be", "012345678", "1234", DateTime.Now,
//             new Adres());
//         var rekening = new Rekening(rekeningType, kredietLimiet, gebruiker);
//         Assert.NotEqual(Guid.Empty, rekening.Rekeningnummer);
//         Assert.Equal(rekeningType, rekening.RekeningType);
//         Assert.Equal(kredietLimiet, rekening.KredietLimiet);
//         Assert.Equal(gebruiker, rekening.Gebruiker);
//         Assert.Equal(0.00m, rekening.Saldo);
//     }
// }