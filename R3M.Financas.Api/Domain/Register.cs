namespace R3M.Financas.Api.Domain;

public abstract class Register
{
    public Guid Id { get; set; }

    public DateTime? InsertDate { get; set; }
    public DateTime? UpdatenDate { get; set; }
}
