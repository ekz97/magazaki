
// TODO
// Header protection
// https://flerka.github.io/personal-blog/2022-06-21-ahead-of-time-compilation/#lets-try-native-aot-in-console AOT!!

using PeasieLib.Models.DTO;
using PeasieAPI.Interfaces;

namespace PeasieAPI
{
    public class ApplicationContextService : IApplicationContextService
        {
            public ILogger? Logger { get; set; }
            public string? PeasieUrl { get; set; }
            public SessionRequestDTOWrapper? Session { get; set; }
            public string? AuthenticationToken { get; set; }
            public string? Issuer { get; set; }
            public string? Audience { get; set; }
            public string? WebHook { get; set; }
            public bool? DemoMode { get; set; }
        }
}