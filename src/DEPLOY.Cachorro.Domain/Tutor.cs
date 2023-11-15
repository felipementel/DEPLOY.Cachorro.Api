using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain
{
    [ExcludeFromCodeCoverage]
    public class Tutor : BaseEntity<long>
    {
        public long CPF { get; set; }
    }
}
