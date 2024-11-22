namespace R3M.Financas.Shared.Dtos;

public class ServerResponse
{
    public bool IsError => ErrorMessage != null;
    public string ErrorMessage { get; set; }
}

public class ServerResponse<T> : ServerResponse
{    
    public T Result { get; set; }
}
