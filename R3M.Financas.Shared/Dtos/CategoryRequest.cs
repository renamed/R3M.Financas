namespace R3M.Financas.Shared.Dtos;

public class CategoryRequest
{
    public string Name { get; set; }

    public Guid? ParentId { get; set; }
}
