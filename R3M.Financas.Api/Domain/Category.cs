namespace R3M.Financas.Api.Domain;

public class Category : Registry
{
    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
}
