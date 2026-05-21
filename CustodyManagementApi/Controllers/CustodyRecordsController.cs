using CustodyManagementApi.Data;
using CustodyManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustodyManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustodyRecordsController(CustodyDbContext dbContext, ILogger<CustodyRecordsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustodyRecord>>> GetAll(CancellationToken cancellationToken)
    {
        var records = await dbContext.CustodyRecords
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(records);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustodyRecord>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var record = await dbContext.CustodyRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return record is null ? NotFound() : Ok(record);
    }

    [HttpPost]
    public async Task<ActionResult<CustodyRecord>> Create([FromBody] CustodyRecord request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var entity = new CustodyRecord
        {
            Id = Guid.NewGuid(),
            PersonName = request.PersonName,
            CaseNumber = request.CaseNumber,
            ArrestedAtUtc = request.ArrestedAtUtc,
            Facility = request.Facility,
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        dbContext.CustodyRecords.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created custody record {RecordId}", entity.Id);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustodyRecord>> Update(Guid id, [FromBody] CustodyRecord request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CustodyRecords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        entity.PersonName = request.PersonName;
        entity.CaseNumber = request.CaseNumber;
        entity.ArrestedAtUtc = request.ArrestedAtUtc;
        entity.Facility = request.Facility;
        entity.Status = request.Status;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Updated custody record {RecordId}", entity.Id);

        return Ok(entity);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CustodyRecords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.CustodyRecords.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Deleted custody record {RecordId}", entity.Id);

        return NoContent();
    }
}
