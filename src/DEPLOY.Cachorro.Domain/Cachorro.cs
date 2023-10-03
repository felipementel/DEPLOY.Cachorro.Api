namespace DEPLOY.Cachorro.Domain
{
    public class Cachorro : BaseEntidade<int>
    {
        public DateTime Nascimento { get; init; }

        public bool Adotado { get; set; }

        public PELAGEM Pelagem { get; init; }

        public float Peso { get; init; }
    }

    [Flags]
    public enum VACINA
    {
        None = 0,
        Vacina1 = 1,
        Vacina2 = 2,
        Vacina3 = 3
    }

    [Flags]
    public enum PELAGEM
    {
        None = 0,
        Curto = 1,
        Medio = 2,
        Longo = 3
    }
}
