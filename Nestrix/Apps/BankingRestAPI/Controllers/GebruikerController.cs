using LogicLayer.Exceptions;
using LogicLayer.Managers;
using LogicLayer.Model;
using Microsoft.AspNetCore.Mvc;
using RESTLayer.Mappers;
using RESTLayer.Model.Input;
using RESTLayer.Model.Output;
using SecurityLayer;

namespace RESTLayer.Controllers;

[Route("[controller]")]
[ApiController]

public class GebruikerController : ControllerBase
{
    private readonly GebruikerManager _gebruikerManager;
    private readonly AdresManager _adresManager;
    private readonly UserService _userService;
    private readonly JwtAuthenticationService _jwtAuthenticationService;

    public GebruikerController(GebruikerManager gM, AdresManager aM, UserService uS, JwtAuthenticationService jAS)
    {
        _gebruikerManager = gM;
        _adresManager = aM;
        _userService = uS;
        _jwtAuthenticationService = jAS;
    }

    // WERKT MAAR OUTPUT DTO ZOU BETER ZIJN
    [HttpGet("{gebruikerId}", Name = nameof(GetGebruiker))]
    public async Task<ActionResult<GebruikerRESTOutputDTO>> GetGebruiker(Guid gebruikerId)
    {
        if (gebruikerId == Guid.Empty) return BadRequest("GebruikerId is leeg");
        try
        {
            var gebruiker = await _gebruikerManager.GebruikerOphalenAsync(gebruikerId);
            if (gebruiker == null) return NotFound("Gebruiker niet gevonden");
            var gebruikerDTO = MapFromDomain.MapFromDomainGebruiker(gebruiker);
            return Ok(gebruikerDTO);
        }
        catch (GebruikerManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // WERKT EN SLAAT GEHASHTE CODE OP IN DB
    [HttpPost(Name = nameof(PostGebruikerAsync))]
    public async Task<ActionResult<GebruikerRESTOutputDTO>> PostGebruikerAsync([FromBody] GebruikerRESTinputDTO gebruiker)
    {
        if (gebruiker == null) return BadRequest("Gebruiker is leeg");
        try
        {
            var adres = await _adresManager.AdresOphalenAsync(gebruiker.Adres.Straat, gebruiker.Adres.Huisnummer, gebruiker.Adres.Postcode, gebruiker.Adres.Gemeente, gebruiker.Adres.Land);
            if (adres == null)
            {
                adres = MapToDomain.MapToDomainAdres(gebruiker.Adres);
                await _adresManager.AdresToevoegenAsync(adres);
            }
            var newGebruiker = MapToDomain.MapToDomainGebruiker(gebruiker, (Guid)adres.Id);
            await _gebruikerManager.GebruikerToevoegenAsync(newGebruiker);
            var newGebruikerDto = MapFromDomain.MapFromDomainGebruiker(newGebruiker);
            return CreatedAtAction(nameof(GetGebruiker), new { gebruikerId = newGebruikerDto.Id }, newGebruikerDto);
        }
        catch (GebruikerManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // WERKT MAAR ADRES IS NIET EDITEERBAAR -- NAKIJKEN AANPASBAARHEID "CODE"
    [HttpPut("{gebruikerId}", Name = nameof(PutGebruikerAsync))]
    public async Task<ActionResult<GebruikerRESTOutputDTO>> PutGebruikerAsync(Guid gebruikerId, [FromBody] GebruikerRESTinputDTO gebruiker)
    {
        if (gebruikerId == Guid.Empty) return BadRequest("GebruikerId is leeg");
        if (gebruiker == null) return BadRequest("Gebruiker is leeg");
        try
        {
            var g = await _gebruikerManager.GebruikerOphalenAsync(gebruikerId);
            if (g == null) return NotFound("Gebruiker niet gevonden");
            var newGebruiker = MapToDomain.MapToDomainGebruiker(gebruikerId, gebruiker, (Guid)g.Adres.Id);
            await _gebruikerManager.GebruikerWijzigenAsync(gebruikerId, newGebruiker);
            var newGebruikerDto = MapFromDomain.MapFromDomainGebruiker(newGebruiker);
            return CreatedAtAction(nameof(GetGebruiker), new { gebruikerId = newGebruikerDto.Id }, newGebruikerDto);
        }
        catch (GebruikerManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{gebruikerId}", Name = nameof(DeleteGebruikerAsync))]
    public async Task<IActionResult> DeleteGebruikerAsync(Guid gebruikerId)
    {
        if (gebruikerId == Guid.Empty) return BadRequest("GebruikerId is leeg");
        try
        {
            var g = await _gebruikerManager.GebruikerOphalenAsync(gebruikerId);
            if (g == null) return NotFound("Gebruiker niet gevonden");
            await _gebruikerManager.GebruikerVerwijderenAsync(gebruikerId);
            return NoContent();
        }
        catch (GebruikerManagerException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login", Name = nameof(LoginAsync))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        if (request == null) return BadRequest("LoginRequest is leeg");
        try
        {
            var gebruiker = await _userService.AuthenticateAsync(request.GebruikerId, request.Code);
            if (gebruiker == null) return Unauthorized();
            var token = await _jwtAuthenticationService.GenerateJwtTokenAsync(gebruiker);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true, // make sure to use HTTPS
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
            return Ok(new { message = "Logged in", token });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}