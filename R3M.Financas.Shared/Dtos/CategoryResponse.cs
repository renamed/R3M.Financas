namespace R3M.Financas.Shared.Dtos;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryResponse Parent { get; set; }
}
