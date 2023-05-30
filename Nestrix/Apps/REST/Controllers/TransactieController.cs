using LogicLayer.Exceptions;
using LogicLayer.Managers;
using LogicLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace RESTLayer.Controllers;

[Route("[controller]")]
[ApiController]
public class TransactieController : ControllerBase
{
    private readonly TransactieManager _transactieManager;

    public TransactieController(TransactieManager tM)
    {
        _transactieManager = tM;
    }

    [HttpGet("{transactieId}", Name = nameof(GetTransactie))]
    public async Task<ActionResult<Transactie>> GetTransactie(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("TransactieId is leeg");
        try
        {
            var transactie = await _transactieManager.TransactieOphalenAsync(id);
            if (transactie == null) return NotFound("Transactie niet gevonden");
            return transactie;
        }
        catch (TransactieManagerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}