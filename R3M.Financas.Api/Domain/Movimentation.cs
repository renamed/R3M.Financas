namespace R3M.Financas.Api.Domain;

public class Movimentation : Register
{
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }

    public Category Category { get; set; }
    public Guid CategoryId { get; set; }
    public Period Period { get; set; }
    public Guid PeriodId { get; set; }
    public Institution Institution { get; set; }
    public Guid InstitutionId { get; set; }
}
