using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Extensions;
using R3M.Financas.Api.Repository;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstitutionController : ControllerBase
{
    private readonly IInstitutionRepository institutionRepository;
    private readonly IValidator<InstitutionRequest> validator;
    private readonly IValidator<InstitutionUpdateRequest> updateValidator;

    public InstitutionController(IInstitutionRepository institutionRepository, IValidator<InstitutionRequest> validator, IValidator<InstitutionUpdateRequest> updateValidator)
    {
        this.institutionRepository = institutionRepository;
        this.validator = validator;
        this.updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var institutions = await institutionRepository.ListAsync().ToListAsync();
        return Ok(institutions.Select(s => new InstitutionResponse
        {
            Id = s.Id,
            Balance = s.Balance,
            Name = s.Name,
        }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = new ServerResponse<InstitutionResponse>();
        var institution = await institutionRepository.GetAsync(id);
        if (institution == null)
        {
            response.ErrorMessage = $"{id} not found";
            return NotFound(response);
        }

        response.Result = new InstitutionResponse
        {
            Id = institution.Id,
            Balance = institution.Balance,
            Name = institution.Name,
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] InstitutionRequest request)
    {
        var response = new ServerResponse<InstitutionResponse>();
        if (request == null)
        {
            response.ErrorMessage = "No data found";
            return BadRequest(response);
        }

        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            response.ErrorMessage = string.Join(", ", validationResult.Errors);
            return BadRequest(response);
        }

        var institution = new Institution
        {
            Balance = request.InitialBalance ?? 0,
            Name = request.Name
        };

        await institutionRepository.AddAsync(institution);
        response.Result = new InstitutionResponse
        {
            Id = institution.Id,
            Name = institution.Name,
            Balance = institution.Balance,
        };

        return Created(Request.Path, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var response = new ServerResponse<InstitutionResponse>();
        var institution = await institutionRepository.GetAsync(id);
        if (institution == null)
        {
            response.ErrorMessage = $"{id} not found";
            return NotFound(response);
        }

        await institutionRepository.DeleteAsync(institution);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] InstitutionUpdateRequest request)
    {
        var response = new ServerResponse<InstitutionResponse>();
        var institution = await institutionRepository.GetAsync(id);
        if (institution == null)
        {
            response.ErrorMessage = $"{id} not found";
            return NotFound(response);
        }

        var validationResult = updateValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            response.ErrorMessage = string.Join(", ", validationResult.Errors);
            return BadRequest(response);
        }

        request.Name = institution.Name;
        await institutionRepository.UpdateAsync(institution);

        response.Result = new InstitutionResponse
        {
            Id = institution.Id,
            Name = institution.Name,
            Balance = institution.Balance
        };

        return Ok(response);
    }
}