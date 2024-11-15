namespace R3M.Financas.Api.Domain;

public abstract class Registry
{
    public int Id { get; set; }

    public DateTime? InsertDate { get; set; }
    public DateTime? UpdatenDate { get; set; }
}
