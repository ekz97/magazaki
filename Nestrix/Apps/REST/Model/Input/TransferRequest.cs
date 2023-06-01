namespace RESTLayer.Model.Input;

public class TransferRequest
{
    public Guid From { get; set; }
    public Guid To { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public TransferRequest(Guid from, Guid to, decimal amount, string? description = null)
    {
        From = from;
        To = to;
        Amount = amount;
        Description = description;
    }
}