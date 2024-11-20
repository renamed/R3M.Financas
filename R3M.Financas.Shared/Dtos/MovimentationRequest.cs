namespace R3M.Financas.Shared.Dtos;

public class MovimentationRequest
{
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }

    public int CategoryId { get; set; }
    public int PeriodId { get; set; }
    public int InstitutionId { get; set; }
}
