using foroLIS_backend.DTOs.CommunityMessagesCommentsDtos;

namespace foroLIS_backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using foroLIS_backend.Models;
using foroLIS_backend.Infrastructure.Context;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CommunityMessageCommentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommunityMessageCommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommunityMessageCommentDto>>> GetAll()
    {
        var comments = await _context.CommunityMessageComments
            .Select(c => new CommunityMessageCommentDto
            {
                Id = c.Id,
                Title = c.Title,
                CreateAt = c.CreateAt,
                CommunityMessageId = c.CommunityMessageId,
                UserId = c.UserId
            }).ToListAsync();

        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityMessageCommentDto>> GetById(Guid id)
    {
        var comment = await _context.CommunityMessageComments.FindAsync(id);
        if (comment == null) return NotFound();

        return new CommunityMessageCommentDto
        {
            Id = comment.Id,
            Title = comment.Title,
            CreateAt = comment.CreateAt,
            CommunityMessageId = comment.CommunityMessageId,
            UserId = comment.UserId
        };
    }

    [HttpPost]
    public async Task<ActionResult<CommunityMessageCommentDto>> Create(CommunityMessageCommentCreateDto dto)
    {
        var comment = new CommunityMessageComment
        {
            Title = dto.Title,
            CommunityMessageId = dto.CommunityMessageId,
            UserId = dto.UserId
        };

        _context.CommunityMessageComments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, new CommunityMessageCommentDto
        {
            Id = comment.Id,
            Title = comment.Title,
            CreateAt = comment.CreateAt,
            CommunityMessageId = comment.CommunityMessageId,
            UserId = comment.UserId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CommunityMessageCommentCreateDto dto)
    {
        var comment = await _context.CommunityMessageComments.FindAsync(id);
        if (comment == null) return NotFound();

        comment.Title = dto.Title;
        comment.CommunityMessageId = dto.CommunityMessageId;
        comment.UserId = dto.UserId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var comment = await _context.CommunityMessageComments.FindAsync(id);
        if (comment == null) return NotFound();

        _context.CommunityMessageComments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}