using LogicLayer.Exceptions;
using LogicLayer.Managers;
using LogicLayer.Model;
using Microsoft.AspNetCore.Mvc;
using RESTLayer.Mappers;
using RESTLayer.Model.Input;

namespace RESTLayer.Controllers;

[Route("[controller]")]
[ApiController]
public class BankController : ControllerBase
{
    private readonly BankManager _bankManager;
    private readonly AdresManager _adresManager;

    public BankController(BankManager bM, AdresManager aM)
    {
        _bankManager = bM;
        _adresManager = aM;
    }

    // WERKT
    [HttpGet("{bankId}", Name = nameof(GetBank))]
    public async Task<ActionResult<Bank>> GetBank(Guid bankId)
    {
        if (bankId == Guid.Empty) return BadRequest("BankId is leeg");
        try
        {
            var bank = await _bankManager.BankOphalenAsync(bankId);
            return Ok(bank);
        }
        catch (BankManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // WERKT
    [HttpPost(Name = nameof(PostBankAsync))]
    public async Task<ActionResult<Bank>> PostBankAsync([FromBody] BankRESTinputDTO bank)
    {
        if (bank == null) return BadRequest("Bank is leeg");
        try
        {
            var adres = await _adresManager.AdresOphalenAsync(bank.Adres.Straat, bank.Adres.Huisnummer,
                bank.Adres.Postcode, bank.Adres.Gemeente, bank.Adres.Land);
            if (adres == null)
            {
                adres = MapToDomain.MapToDomainAdres(bank.Adres);
                await _adresManager.AdresToevoegenAsync(adres);
            }

            var newBank = MapToDomain.MapToDomainBank(bank, (Guid)adres.Id);
            await _bankManager.BankToevoegenAsync(newBank);
            return CreatedAtAction(nameof(GetBank), new { bankId = newBank.Id }, newBank);
        }
        catch (BankManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // WERKT MAAR ADRES IS NIET AANPASBAAR
    [HttpPut("{bankId:guid}", Name = nameof(PutBankAsync))]
    public async Task<ActionResult<Bank>> PutBankAsync(Guid bankId, [FromBody] BankRESTinputDTO bank)
    {
        if (bankId == Guid.Empty) return BadRequest("BankId is leeg");
        if (bank == null) return BadRequest("Bank is leeg");
        try
        {
            var bankDb = await _bankManager.BankOphalenAsync(bankId);
            if (bankDb == null) return NotFound("Bank niet gevonden");
            var newBank = MapToDomain.MapToDomainBank(bankId, bank, (Guid)bankDb.Adres.Id);
            await _bankManager.BankWijzigenAsync(bankId, newBank);
            return CreatedAtAction(nameof(GetBank), new { bankId = newBank.Id }, newBank);
        }
        catch (BankManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // WERKT MAAR HARD DELETE, VERWIJDERT BANK => CHECK COMMENT IN METHOD
    [HttpDelete("{bankId:guid}", Name = nameof(DeleteBankAsync))]
    public async Task<IActionResult> DeleteBankAsync(Guid bankId)
    {
        // Bank verwijderen == ALLES verwijderen wat met die bank te maken heeft?
        if (bankId == Guid.Empty) return BadRequest("BankId is leeg");
        try
        {
            var bankDb = await _bankManager.BankOphalenAsync(bankId);
            if (bankDb == null) return NotFound("Bank niet gevonden");
            await _bankManager.BankVerwijderenAsync(bankDb.Id);
            return NoContent();
        }
        catch (BankManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}