using LogicLayer.Exceptions;
using LogicLayer.Managers;
using LogicLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RESTLayer.Mappers;
using RESTLayer.Model.Input;
using RESTLayer.Model.Output;

namespace RESTLayer.Controllers;

// [Authorize]
[Route("[controller]")]
[ApiController]
public class RekeningController : ControllerBase
{
    private readonly GebruikerManager _gebruikerManager;
    private readonly RekeningManager _rekeningManager;

    public RekeningController(GebruikerManager gM, RekeningManager rM)
    {
        _gebruikerManager = gM;
        _rekeningManager = rM;
    }

    [HttpGet("{rekeningId}", Name = nameof(GetRekening))]
    public async Task<ActionResult<RekeningRESTOutputDTO>> GetRekening(Guid rekeningId, int depth = 0)
    {
        if (rekeningId == Guid.Empty) return BadRequest("RekeningId is leeg");
        try
        {
            // depth is by default 0 but can be set higher to get more data
            var rekening = await _rekeningManager.RekeningOphalenAsync(rekeningId, depth);
            if (rekening == null) return NotFound("Rekening niet gevonden");
            var rekeningDto = MapFromDomain.MapFromDomainRekening(rekening);
            return Ok(rekeningDto);
        }
        catch (RekeningManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost(Name = nameof(PostRekeningAsync))]
    public async Task<ActionResult<RekeningRESTOutputDTO>> PostRekeningAsync([FromBody] RekeningRESTinputDTO rekening)
    {
        if (rekening == null) return BadRequest("Rekening is leeg");
        try
        {
            var gebruiker = await _gebruikerManager.GebruikerOphalenAsync(rekening.GebruikerId);
            if (gebruiker == null) return NotFound("Gebruiker niet gevonden");
            var newRekening = MapToDomain.MapToDomainRekening(rekening, gebruiker);
            await _rekeningManager.RekeningToevoegenAsync(newRekening);
            var newRekeningDto = MapFromDomain.MapFromDomainRekening(newRekening);
            return CreatedAtAction(nameof(GetRekening), new { rekeningId = newRekening.Rekeningnummer },
                newRekeningDto);
        }
        catch (RekeningManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("/{rekeningId:guid}", Name = nameof(PutRekeningAsync))]
    public async Task<IActionResult> PutRekeningAsync(Guid rekeningId, [FromBody] RekeningRESTinputDTO rekening)
    {
        if (rekeningId == Guid.Empty) return BadRequest("RekeningId is leeg");
        if (rekening == null) return BadRequest("Rekening is leeg");
        try
        {
            var rekeningDb = await _rekeningManager.RekeningOphalenAsync(rekeningId, 0);
            if (rekeningDb == null) return NotFound("Rekening niet gevonden");
            var newRekening = MapToDomain.MapToDomainRekening(rekeningId, rekening);
            await _rekeningManager.RekeningWijzigenAsync(rekeningId, newRekening);
            return CreatedAtAction(nameof(GetRekening), new { rekeningId = newRekening.Rekeningnummer },
                newRekening);
        }
        catch (RekeningManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("/{rekeningId:guid}", Name = nameof(DeleteRekeningAsync))]
    public async Task<IActionResult> DeleteRekeningAsync(Guid rekeningId)
    {
        if (rekeningId == Guid.Empty) return BadRequest("RekeningId is leeg");
        try
        {
            var rekeningDb = await _rekeningManager.RekeningOphalenAsync(rekeningId, 0);
            if (rekeningDb == null) return NotFound("Rekening niet gevonden");
            await _rekeningManager.RekeningVerwijderenAsync(rekeningId);
            return NoContent();
        }
        catch (RekeningManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("transactie", Name = nameof(TransferAsync))]
    // [Authorize(Policy = "CanTransferFromRekening")]
    public async Task<IActionResult> TransferAsync([FromBody] TransferRequest request)
    {
        if (request.From == Guid.Empty) return BadRequest("RekeningId is leeg");
        if (request.To == Guid.Empty) return BadRequest("RekeningId is leeg");
        if (request.Amount <= 0) return BadRequest("Bedrag moet groter zijn dan 0");
        if (request.From == request.To)
            return BadRequest("RekeningnummerFrom en RekeningnummerTo mogen niet hetzelfde zijn");
        try
        {
            // var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value;
            // if (userId == null) return Unauthorized();

            var from = await _rekeningManager.RekeningOphalenAsync(request.From, 0);
            if (from == null) return NotFound("Rekening niet gevonden");
            // if (from.Gebruiker.Id != Guid.Parse(userId)) return Unauthorized();
            var to = await _rekeningManager.RekeningOphalenAsync(request.To, 0);
            if (to == null) return NotFound("Rekening niet gevonden");
            if (request.Description != null)
                await _rekeningManager.TransferMoneyAsync(from, to, request.Amount, request.Description);
            else await _rekeningManager.TransferMoneyAsync(from, to, request.Amount);
            return Ok();
        }
        catch (RekeningManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}