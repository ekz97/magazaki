using LogicLayer.Exceptions;
using LogicLayer.Model;

namespace TestLayer.ModelTests;

public class TransactieTests
{
    [Theory]
    [InlineData(-200.00)]
    [InlineData(-1.00)]
    public void ZetBedrag_NegatiefBedrag_TransactieException(decimal bedrag)
    {
        var transactie = new Transactie();
        Assert.Throws<TransactieException>(() => transactie.ZetBedrag(bedrag));
    }

    [Theory]
    [InlineData(0.00)]
    [InlineData(200.00)]
    public void ZetBedrag_ValidBedrag_NoException(decimal bedrag)
    {
        var transactie = new Transactie();
        transactie.ZetBedrag(bedrag);
        Assert.Equal(bedrag, transactie.Bedrag);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Overschrijving")]
    public void ZetMededeling_Valid_NoException(string mededeling)
    {
        var transactie = new Transactie();
        transactie.ZetMededeling(mededeling);
        Assert.Equal(mededeling, transactie.Mededeling);
    }
    
    [Fact]
    public void ZetRekening_Null_TransactieException()
    {
        var transactie = new Transactie();
        Assert.Throws<TransactieException>(() => transactie.ZetRekening(null));
    }

    [Fact]
    public void ZetRekening_Valid_NoException()
    {
        var transactie = new Transactie();
        var rekening = new Rekening();
        transactie.ZetRekening(rekening);
        Assert.Equal(rekening, transactie.Rekening);
    }
    
    // [Theory]
    // [InlineData(TransactieType.Betalen)]
    // [InlineData(TransactieType.Ontvangen)]
    // public void ZetTransactieType_Valid_NoException(TransactieType transactieType)
    // {
    //     var transactie = new Transactie();
    //     transactie.ZetTransactieType(transactieType);
    //     Assert.Equal(transactieType, transactie.TransactieType);
    // }
    //
    //
    // [Fact]
    // public void Transactie_Constructor_NoException()
    // {
    //     var bedrag = 100.00m;
    //     var mededeling = "Overschrijving";
    //     var rekening = new Rekening();
    //     var transactieType = TransactieType.Betalen;
    //     var transactie = new Transactie(bedrag, mededeling, rekening, transactieType);
    //     Assert.NotEqual(Guid.Empty, transactie.Id);
    //     Assert.Equal(bedrag, transactie.Bedrag);
    //     Assert.Equal(mededeling, transactie.Mededeling);
    //     Assert.Equal(rekening, transactie.Rekening);
    //     Assert.Equal(transactieType, transactie.TransactieType);
    // }
}