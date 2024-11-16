using MediatR;
using System.ComponentModel.DataAnnotations;

namespace R3M.Financas.Shared.Dtos;

public class CategoryRequest : IRequest<CategoryResponse>
{
    public string Name { get; set; }

    public int? ParentId { get; set; }
}
