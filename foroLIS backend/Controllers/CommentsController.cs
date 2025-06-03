using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommentDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace foroLIS_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
        {
            var comments = await _context.Comments
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    ArchitectureOS = c.ArchitectureOS,
                    FamilyOS = c.FamilyOS,
                    VersionOS = c.VersionOS,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    MediaFileId = c.MediaFileId,
                    CreateAt = c.CreateAt,
                    UpdateAt = c.UpdateAt
                }).ToListAsync();

            return Ok(comments);
        }

        // GET: api/Comments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(Guid id)
        {
            var comment = await _context.Comments
                .Where(c => c.Id == id)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    ArchitectureOS = c.ArchitectureOS,
                    FamilyOS = c.FamilyOS,
                    VersionOS = c.VersionOS,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    MediaFileId = c.MediaFileId,
                    CreateAt = c.CreateAt,
                    UpdateAt = c.UpdateAt
                }).FirstOrDefaultAsync();

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        // POST: api/Comments
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            if (!await _context.Posts.AnyAsync(p => p.Id == dto.PostId))
                return BadRequest("El PostId no existe.");

            if (dto.MediaFileId.HasValue && !await _context.MediaFiles.AnyAsync(m => m.Id == dto.MediaFileId.Value))
                return BadRequest("El MediaFileId no existe.");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                ArchitectureOS = dto.ArchitectureOS,
                FamilyOS = dto.FamilyOS,
                VersionOS = dto.VersionOS,
                PostId = dto.PostId,
                UserId = userId,
                MediaFileId = dto.MediaFileId,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var result = new CommentDto
            {
                Id = comment.Id,
                ArchitectureOS = comment.ArchitectureOS,
                FamilyOS = comment.FamilyOS,
                VersionOS = comment.VersionOS,
                PostId = comment.PostId,
                UserId = comment.UserId,
                MediaFileId = comment.MediaFileId,
                CreateAt = comment.CreateAt,
                UpdateAt = comment.UpdateAt
            };

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, result);
        }

        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, UpdateCommentDto dto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            comment.ArchitectureOS = dto.ArchitectureOS;
            comment.FamilyOS = dto.FamilyOS;
            comment.VersionOS = dto.VersionOS;
            comment.PostId = dto.PostId;
            comment.MediaFileId = dto.MediaFileId;
            comment.UpdateAt = DateTime.UtcNow;

            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Comments/ByPost/{postId}
        [HttpGet("ByPost/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPostId(Guid postId)
        {
            
            if (!await _context.Posts.AnyAsync(p => p.Id == postId))
            {
                return NotFound(new { Message = $"No existe un post con ID {postId}" });
            }

            
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreateAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    ArchitectureOS = c.ArchitectureOS,
                    FamilyOS = c.FamilyOS,
                    VersionOS = c.VersionOS,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    MediaFileId = c.MediaFileId,
                    CreateAt = c.CreateAt,
                    UpdateAt = c.UpdateAt
                })
                .ToListAsync();

            return Ok(comments);
        }
        // GET /api/Posts/Debug/ListIds
        [HttpGet("Debug/ListIds")]
        public async Task<IActionResult> GetPostIds()
        {
            var posts = await _context.Posts
                .Select(p => new {
                    p.Id,
                    p.Title,
                    Created = p.CreateAt.ToString("yyyy-MM-dd")
                })
                .OrderByDescending(p => p.Created)
                .ToListAsync();

            return Ok(posts);
        }

    }
}

