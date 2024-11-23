using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository categoryRepository;
    private readonly IMovimentationRepository movimentationRepository;
    private readonly IValidator<CategoryRequest> categoryValidator;

    public CategoryController(ICategoryRepository categoryRepository, IValidator<CategoryRequest> categoryValidator, IMovimentationRepository movimentationRepository)
    {
        this.categoryRepository = categoryRepository;
        this.categoryValidator = categoryValidator;
        this.movimentationRepository = movimentationRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CategoryRequest request)
    {
        Category? parent = request.ParentId.HasValue
            ? await categoryRepository.GetAsync(request.ParentId.Value)
            : null;

        var errorMsg = await GetErrorMsgForCreationAsync(request, parent);
        if (errorMsg != null)
        {
            return errorMsg;
        }

        var newCategory = new Category
        {
            Name = request.Name,
            ParentId = parent?.ParentId
        };

        await categoryRepository.AddAsync(newCategory);
        return Created(Request?.Path, new ServerResponse<CategoryResponse>
        {
            Result = new CategoryResponse
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                ParentId = parent?.Id,
                ParentName = parent?.Name
            }
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CategoryRequest request)
    {
        Category? parent = request.ParentId.HasValue
            ? await categoryRepository.GetAsync(request.ParentId.Value)
            : null;

        var errorMsg = await GetErrorMsgForCreationAsync(request, parent);
        if (errorMsg != null)
        {
            return errorMsg;
        }

        var category = await categoryRepository.GetAsync(id);
        if (category == null)
        {
            return NotFound(new ServerResponse
            {
                ErrorMessage = "Category not found"
            });
        }

        category.Name = request.Name;
        category.ParentId = request.ParentId;

        await categoryRepository.UpdateAsync(category);
        return Ok(new ServerResponse<CategoryResponse>
        {
            Result = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = parent?.Id,
                ParentName = parent?.Name
            }
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var category = await categoryRepository.GetAsync(id);
        if (category == null)
        {
            return NotFound(new ServerResponse
            {
                ErrorMessage = "Category not found"
            });
        }

        var childrenCount = await categoryRepository.GetChildrenCountAsync(category.Id);
        if (childrenCount > 0)
        {
            return BadRequest(new ServerResponse
            {
                ErrorMessage = "Cannot delete category with children"
            });
        }

        var movimentationsCount = await movimentationRepository.GetCategoryCountAsync(category.Id);
        if (movimentationsCount > 0)
        {
            return BadRequest(new ServerResponse
            {
                ErrorMessage = "Cannot delete category with movimentations attached to it"
            });
        }

        await categoryRepository.DeleteAsync(category);
        return Ok(new ServerResponse());
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var parents = await categoryRepository.ListAsync(null);
        return Ok(new ServerResponse<IEnumerable<CategoryResponse>>
        {
            Result = parents.Select(x => new CategoryResponse
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = null,
                ParentName = null
            })
        });
    }

    [HttpGet("{parentId}")]
    public async Task<IActionResult> ListChildrenAsync(Guid parentId)
    {
        var category = await categoryRepository.GetAsync(parentId);
        if (category == null)
        {
            return NotFound(new ServerResponse
            {
                ErrorMessage = "Category not found"
            });
        }
        var children = await categoryRepository.ListAsync(parentId);
        return Ok(new ServerResponse<IEnumerable<CategoryResponse>>
        {
            Result = children.Select(x => new CategoryResponse
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = category.Id,
                ParentName = category.Name
            })
        });
    }



    private async Task<IActionResult?> GetErrorMsgForCreationAsync(CategoryRequest request, Category? parent)
    {
        var validationResult = categoryValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ServerResponse
            {
                ErrorMessage = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))
            });
        }

        parent = null;
        if (request.ParentId.HasValue && parent == null)
        {
            return NotFound(new ServerResponse
            {
                ErrorMessage = "Parent not found"
            });
        }

        var catSameName = await categoryRepository.GetAsync(request.Name);
        if (catSameName != null)
        {
            return BadRequest(new ServerResponse
            {
                ErrorMessage = "Category with the same name already exists"
            });
        }

        return null;
    }
}
