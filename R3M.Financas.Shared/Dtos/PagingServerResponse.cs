namespace R3M.Financas.Shared.Dtos;

public class PagingServerResponse<T> : ServerResponse<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int Count { get; set; }
}
