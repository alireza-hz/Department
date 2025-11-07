using Application.Intrfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace API_Dpepartment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _repo;
        private readonly IDistributedCache _cache;
        private readonly IWebHostEnvironment _env;
        private const int MaxRedisSize = 5 * 1024 * 1024; // 5MB

        public AttachmentController(IAttachmentRepository repo, IDistributedCache cache, IWebHostEnvironment env)
        {
            _repo = repo;
            _cache = cache;
            _env = env;
        }

        // 📤 Upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] int letterId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var attachment = new Attachment
            {
                LetterId = letterId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow
            };

            // فایل‌های کوچک → Redis
            if (file.Length <= MaxRedisSize)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                var redisKey = $"attachment:{Guid.NewGuid()}";
                await _cache.SetAsync(redisKey, fileBytes, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                });

                attachment.RedisKey = redisKey;
            }
            else
            {
                // فایل‌های بزرگ → File System
                var uploadPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadPath, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                attachment.FilePath = $"/uploads/{uniqueName}";
            }

            _repo.Add(attachment);
            _repo.Save();

            return Ok(new
            {
                attachment.Id,
                attachment.FileName,
                attachment.FileSize,
                attachment.RedisKey,
                attachment.FilePath
            });
        }

        // 📜 لیست پیوست‌ها برای یک نامه
        [HttpGet("by-letter/{letterId}")]
        public IActionResult GetByLetter(int letterId)
        {
            var attachments = _repo.GetByLetterId(letterId);
            return Ok(attachments);
        }

        // 📥 دانلود فایل
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var attachment = _repo.Get(id);
            if (attachment == null)
                return NotFound();

            byte[]? fileBytes = null;

            if (!string.IsNullOrEmpty(attachment.RedisKey))
            {
                // از Redis
                fileBytes = await _cache.GetAsync(attachment.RedisKey);
                if (fileBytes == null)
                    return NotFound("File expired from cache.");
            }
            else if (!string.IsNullOrEmpty(attachment.FilePath))
            {
                // از دیسک
                var fullPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), attachment.FilePath.TrimStart('/'));
                if (!System.IO.File.Exists(fullPath))
                    return NotFound();

                fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            }

            if (fileBytes == null)
                return NotFound();

            return File(fileBytes, attachment.ContentType, attachment.FileName);
        }

        // ❌ حذف فایل
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var attachment = _repo.Get(id);
            if (attachment == null)
                return NotFound();

            if (!string.IsNullOrEmpty(attachment.RedisKey))
                await _cache.RemoveAsync(attachment.RedisKey);

            if (!string.IsNullOrEmpty(attachment.FilePath))
            {
                var fullPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), attachment.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            _repo.Delete(attachment);
            _repo.Save();

            return NoContent();
        }
    }
}