using System.Collections.Concurrent;

namespace PeasieAPI.Services.Interfaces;

public interface IDataManagerService
{
    public ILogger? Logger { get; set; }
    public ConcurrentDictionary<string, SessionWrapper> Sessions { get; set; }

    public void Reset();
    public void Recycle();
}
