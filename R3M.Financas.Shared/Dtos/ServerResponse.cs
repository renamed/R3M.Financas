namespace R3M.Financas.Shared.Dtos;

public class ServerResponse<T>
{
    public bool IsError => ErrorMessage != null;
    public string ErrorMessage { get; set; }
    public T Result { get; set; }
}
