using PeasieAPI.Services.Interfaces;
using System.Collections.Concurrent;

namespace PeasieAPI.Services;

public class DataManagerService: IDataManagerService
{
    public ILogger? Logger { get; set; }
    public ConcurrentDictionary<string, SessionWrapper> Sessions { get; set; } = new();

    public void Reset()
    {
        Sessions.Clear();
    }

    public void Recycle()
    {
        // We keep history around one more cycle

        List<string> keysToRemove = new();

        foreach(var kv in Sessions)
        {
            if(kv.Value.Valid == false)
                keysToRemove.Add(kv.Key);
        }
        foreach (var key in keysToRemove)
        {
            Logger?.LogInformation($"Removing session {key}");
            if (Sessions.TryRemove(key, out SessionWrapper? item))
            {
                Logger?.LogInformation($"Session {key} removed");
            }
            else
            {
                Logger?.LogInformation($"Session {key} could not be removed");
            }
        }

        foreach(var key in Sessions.Keys)
        {
            if (Sessions.TryGetValue(key, out SessionWrapper? item))
            {
                if (item != null && item.SessionResponse?.ReplyTimeUtc.Add(item.SessionResponse.ValidityTimeSpan) < DateTime.UtcNow)
                {
                    Logger?.LogInformation($"Marking session {key}");
                    item.Valid = false;
                }
                else
                {
                    Logger?.LogInformation($"Session {key} not marked");
                }
            }
        }
    }
}
