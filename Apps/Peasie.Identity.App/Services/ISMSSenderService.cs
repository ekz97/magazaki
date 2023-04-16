namespace Peasie.Web.Services
{
    public interface ISMSSenderService
    {
        Task SendSmsAsync(string number, string message);
    }
}
