namespace R3M.Financas.Api.Domain;

public class Movimentation : Registry
{
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }

    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public Period Period { get; set; }
    public int PeriodId { get; set; }
    public Institution Institution { get; set; }
    public int InstitutionId { get; set; }
}
