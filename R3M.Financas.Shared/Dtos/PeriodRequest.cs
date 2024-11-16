namespace R3M.Financas.Shared.Dtos;

public class PeriodRequest
{
    public DateOnly InitialDate { get; set; }
    public DateOnly FinalDate { get; set; }
    public string Description { get; set; }
}
