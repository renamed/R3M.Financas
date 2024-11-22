using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Extensions;
using R3M.Financas.Api.Repository;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeriodController : ControllerBase
{
    private readonly IPeriodRepository periodRepository;
    private readonly IMovimentationRepository movimentationRepository;
    private readonly IValidator<PeriodRequest> validator;

    public PeriodController(IPeriodRepository periodRepository, IMovimentationRepository movimentationRepository, IValidator<PeriodRequest> validator)
    {
        this.periodRepository = periodRepository;
        this.movimentationRepository = movimentationRepository;
        this.validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] int page = 1, [FromQuery] int count = 12)
    {
        if (page <= 0 || count <= 0)
        {
            return BadRequest(new ServerResponse()
            {
                ErrorMessage = "Page and count must be positive greater than zero"
            });
        }

        var response = 
            await CreatesBuildResponseAsync(default, default, page, count);

        return Ok(response);
    }

    [HttpGet("{startDate}/{endDate}")]
    public async Task<IActionResult> ListAsync([FromRoute] DateOnly startDate, 
        [FromRoute] DateOnly endDate,
        [FromQuery] int page = 1, 
        [FromQuery] int count = 12)
    {
        if (page <= 0 || count <= 0)
        {
            return BadRequest(new ServerResponse()
            {
                ErrorMessage = "Page and count must be positive greater than zero"
            });
        }

        if (endDate < startDate)
        {
            return BadRequest(new ServerResponse()
            {
                ErrorMessage = "Start date may be after end date"
            });
        }

        PagingServerResponse<IEnumerable<PeriodResponse>> response = 
            await CreatesBuildResponseAsync(startDate, endDate, page, count);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] PeriodRequest request)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ServerResponse()
            {
                ErrorMessage = string.Join(", ", validationResult.Errors)
            });
        }

        var periodByDateRange = await periodRepository.GetAsync(request.InitialDate, request.FinalDate);
        if (periodByDateRange != null)
        {
            return BadRequest(new ServerResponse()
            {
                ErrorMessage = $"There is already a period between {request.InitialDate} and {request.FinalDate}"
            });
        }

        var periodByDescription = await periodRepository.GetAsync(request.Description);
        if (periodByDescription != null)
        {
            return BadRequest(new ServerResponse()
            {
                ErrorMessage = $"There is already a period with such description"
            });
        }

        var newPeriod = new Period
        {
            Description = request.Description,
            Start = request.InitialDate,
            End = request.FinalDate
        };

        await periodRepository.AddAsync(newPeriod);
        return Created(Request?.Path, new ServerResponse<PeriodResponse>()
        {
            Result = new PeriodResponse
            {
                Description = newPeriod.Description,
                InitialDate = request.InitialDate,
                FinalDate = request.FinalDate,
                Id = newPeriod.Id
            }
        });
    }

    private async Task<PagingServerResponse<IEnumerable<PeriodResponse>>> CreatesBuildResponseAsync(DateOnly startDate, DateOnly endDate, int page, int count)
    {        
        List<Period> periods = 
            startDate != default && endDate != default
            ? (await periodRepository.ListAsync(startDate, endDate, page, count)).ToList()
            : (await periodRepository.ListAsync(page, count)).ToList();

        int periodCount = await periodRepository.CountAsync();

        PagingServerResponse<IEnumerable<PeriodResponse>> response = new();
        response.Count = periods.Count;
        response.CurrentPage = page;
        response.TotalPages = Convert.ToInt32(Math.Ceiling(1.0 * periodCount / count));
        response.Result = periods.Select(x => new PeriodResponse
        {
            Description = x.Description,
            FinalDate = x.End,
            InitialDate = x.Start,
            Id = x.Id
        });
        return response;
    }
}
