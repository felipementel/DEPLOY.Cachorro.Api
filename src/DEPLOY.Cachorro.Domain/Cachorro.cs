namespace DEPLOY.Cachorro.Domain
{
    public class Cachorro : BaseEntidade<int>
    {
        public DateTime Nascimento { get; init; }

        public bool Adotado { get; set; }

        public List<Vacina> Vacinas { get; }

        public string Pelagem { get; init; }

        public float Peso { get; init; }

        public void Vacinar(Vacina vacinas)
        {
            Vacinas.Add(vacinas);
        }
    }

    [Flags]
    public enum Vacina
    {
        Vacina1,
        Vacina2,
        Vacina3
    }

    [Flags]
    public enum Pelagem
    {
        Curto,
        Medio,
        Longo
    }
}
