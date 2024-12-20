﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstitutionController : ControllerBase
{
    private readonly IInstitutionRepository institutionRepository;
    private readonly IMovimentationRepository movimentationRepository;
    private readonly IValidator<InstitutionRequest> validator;
    private readonly IValidator<InstitutionUpdateRequest> updateValidator;

    public InstitutionController(IInstitutionRepository institutionRepository, IValidator<InstitutionRequest> validator, IValidator<InstitutionUpdateRequest> updateValidator, IMovimentationRepository movimentationRepository)
    {
        this.institutionRepository = institutionRepository;
        this.validator = validator;
        this.updateValidator = updateValidator;
        this.movimentationRepository = movimentationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var institutions = await institutionRepository.ListAsync();
        return Ok(new ServerResponse<IEnumerable<InstitutionResponse>>()
        {
            Result = institutions.Select(s => new InstitutionResponse
            {
                Id = s.Id,
                Balance = s.Balance,
                Name = s.Name,
            })
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
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

        bool nameExists = await institutionRepository.ExistsAsync(request.Name);
        if (nameExists)
        {
            response.ErrorMessage = "Name already taken";
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
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var response = new ServerResponse<InstitutionResponse>();
        var institution = await institutionRepository.GetAsync(id);
        if (institution == null)
        {
            response.ErrorMessage = $"{id} not found";
            return NotFound(response);
        }

        var movimentationsInPeriod = await movimentationRepository.ListAsync(id);
        if (movimentationsInPeriod.Any())
        {
            response.ErrorMessage = $"Period has movimentations attached to it";
            return BadRequest(response);
        }

        await institutionRepository.DeleteAsync(institution);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] InstitutionUpdateRequest request)
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

        bool nameExists = await institutionRepository.ExistsAsync(request.Name);
        if (nameExists)
        {
            response.ErrorMessage = "Name already taken";
            return BadRequest(response);
        }

        institution.Name = request.Name;
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