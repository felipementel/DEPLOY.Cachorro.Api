using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.Cachorro.Domain
{
    [ExcludeFromCodeCoverage]
    public class Tutor : BaseEntidade<int>
    {
        public long CPF { get; set; }
    }
}
