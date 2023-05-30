using LogicLayer.Exceptions;
using LogicLayer.Managers;
using LogicLayer.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RESTLayer.Mappers;
using RESTLayer.Model.Input;

namespace RESTLayer.Controllers;

[Route("[controller]")]
[ApiController]
public class AdresController : ControllerBase
{
    private readonly GebruikerManager _gebruikerManager;
    private readonly BankManager _bankManager;
    private readonly TransactieManager _transactieManager;
    private readonly RekeningManager _rekeningManager;
    private readonly AdresManager _adresManager;

    public AdresController(GebruikerManager gM, BankManager bM, TransactieManager tM, RekeningManager rM,
        AdresManager aM)
    {
        _gebruikerManager = gM;
        _bankManager = bM;
        _transactieManager = tM;
        _rekeningManager = rM;
        _adresManager = aM;
    }

    // WERKT
    [HttpPost(Name = nameof(PostAdresAsync))]
    public async Task<ActionResult<Adres>> PostAdresAsync([FromBody] AdresRESTinputDTO adres)
    {
        if (adres == null) return BadRequest("Adres is leeg");
        try
        {
            var newAdres = MapToDomain.MapToDomainAdres(adres);
            await _adresManager.AdresToevoegenAsync(newAdres);
            return CreatedAtAction(nameof(GetAdres), new { adresId = newAdres.Id }, newAdres);
        }
        catch (AdresManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

// WERKT
    [HttpGet("{adresId}", Name = nameof(GetAdres))]
    public async Task<ActionResult<Adres>> GetAdres(Guid adresId)
    {
        if (adresId == Guid.Empty) return BadRequest("AdresId is leeg");
        try
        {
            var adres = await _adresManager.AdresOphalenAsync(adresId);
            return Ok(adres);
        }
        catch (AdresManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

// WERKT
    [HttpPut("{adresId}", Name = nameof(PutAdresAsync))]
    public async Task<IActionResult> PutAdresAsync(Guid adresId, [FromBody] AdresRESTinputDTO adres)
    {
        if (adresId == Guid.Empty) return BadRequest("AdresId is leeg");
        if (adres == null) return BadRequest("Adres is leeg");
        try
        {
            var adresDb = await _adresManager.AdresOphalenAsync(adresId);
            if (adresDb == null) return NotFound("Adres niet gevonden");
            var newAdres = MapToDomain.MapToDomainAdres(adresId, adres);
            await _adresManager.AdresWijzigenAsync(adresId, newAdres);
            return CreatedAtAction(nameof(GetAdres), new { adresId = newAdres.Id }, newAdres);
        }
        catch (AdresManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

// WERKT
    [HttpDelete("{adresId}", Name = nameof(DeleteAdresAsync))]
    public async Task<IActionResult> DeleteAdresAsync(Guid adresId)
    {
        if (adresId == Guid.Empty) return BadRequest("AdresId is leeg");
        try
        {
            var adresDb = await _adresManager.AdresOphalenAsync(adresId);
            if (adresDb == null) return NotFound("Adres niet gevonden");
            await _adresManager.AdresVerwijderenAsync(adresId);
            return NoContent();
        }
        catch (AdresManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}