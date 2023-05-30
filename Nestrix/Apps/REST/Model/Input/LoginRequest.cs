namespace RESTLayer.Model.Input;

public class LoginRequest
{
    public Guid GebruikerId { get; set; }
    public string Code { get; set; }

    public LoginRequest(Guid gebruikerId, string code)
    {
        GebruikerId = gebruikerId;
        Code = code;
    }
}