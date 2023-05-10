using PeasieAPI.Services.Interfaces;
using PeasieLib.Interfaces;

namespace PeasieLib.Handlers;

public class PeasieAuthorizationRequirement : IAuthorizationRequirement 
{
    private readonly IPeasieApplicationContextService? _applicationContextService;
    private readonly IDataManagerService? _dataManagerService;

    public PeasieAuthorizationRequirement(IPeasieApplicationContextService? applicationContextService, IDataManagerService? dataManagerService)
    {
        _applicationContextService = applicationContextService;
        _dataManagerService = dataManagerService;
    }
}
