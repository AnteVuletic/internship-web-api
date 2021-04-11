using System.Text;

namespace StudentMentor.Domain.Models.Configurations
{
    public class JwtConfiguration
    {
        public string Issuer { get; set; }
        public string AudienceId { get; set; }
        public string AudienceSecret { get; set; }
        public int ExpiryMinutes { get; set; }

        public byte[] GetAudienceSecretBytes() => Encoding.UTF8.GetBytes(AudienceSecret);
    }
}
