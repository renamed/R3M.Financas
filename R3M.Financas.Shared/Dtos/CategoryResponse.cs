namespace R3M.Financas.Shared.Dtos;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string ParentName { get; set; }
}
