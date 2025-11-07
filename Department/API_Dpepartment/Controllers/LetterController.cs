using Application.Intrfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Dpepartment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LetterController : ControllerBase
    {
        private readonly IletterRepozitory _repo;

        public LetterController(IletterRepozitory repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var letters = _repo.GetAll();
            return Ok(letters);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var letter = _repo.GetFullById(id);
            if (letter == null) return NotFound();
            return Ok(letter);
        }

        [HttpGet("sent/{senderId}")]
        public IActionResult GetBySender(int senderId)
        {
            var letters = _repo.GetBySender(senderId);
            return Ok(letters);
        }

        [HttpGet("received/{receiverId}")]
        public IActionResult GetByReceiver(int receiverId)
        {
            var letters = _repo.GetByReceiver(receiverId);
            return Ok(letters);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Letter letter)
        {
            letter.CreatedAt = DateTime.UtcNow;
            _repo.Add(letter);
            _repo.Save();
            return Ok(letter);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Letter letter)
        {
            var existing = _repo.Get(id);
            if (existing == null) return NotFound();

            existing.Subject = letter.Subject;
            existing.Body = letter.Body;
            existing.ReceiverId = letter.ReceiverId;
            existing.LetterTypeId = letter.LetterTypeId;
            existing.Status = letter.Status;
            _repo.Update(existing);
            _repo.Save();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _repo.Get(id);
            if (existing == null) return NotFound();

            _repo.Delete(existing);
            _repo.Save();
            return NoContent();
        }
    }
}
