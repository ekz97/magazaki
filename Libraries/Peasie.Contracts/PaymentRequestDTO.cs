using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{
    public class PaymentRequestDTO : IToHtmlTable
    {
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string? BeneficiaryPublicKey { get; set; }
        public SessionDetailsDTO? SessionDetails { get; set; }
    }
}