namespace R3M.Financas.Api.Domain;

public class Category : Register
{
    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
}
