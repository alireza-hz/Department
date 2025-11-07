using Application.Intrfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Dpepartment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoteController : Controller
    {
        private readonly INoteRepozitory _noteRepo;

        public NoteController(INoteRepozitory noteRepo)
        {
            _noteRepo = noteRepo;
        }

        [HttpGet("by-letter/{letterId}")]
        public IActionResult GetNotesByLetter(int letterId)
        {
            var notes = _noteRepo.GetNotesByLetter(letterId);
            return Ok(notes);
        }

        [HttpGet("by-user/{userId}")]
        public IActionResult GetNotesByUser(int userId)
        {
            var notes = _noteRepo.GetNotesByUser(userId);
            return Ok(notes);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Note note)
        {
            note.CreatedAt = DateTime.UtcNow;
            _noteRepo.Add(note);
            _noteRepo.Save();
            return Ok(note);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Note note)
        {
            var existing = _noteRepo.Get(id);
            if (existing == null)
                return NotFound();

            existing.Text = note.Text;
            _noteRepo.Update(existing);
            _noteRepo.Save();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var note = _noteRepo.Get(id);
            if (note == null)
                return NotFound();

            _noteRepo.Delete(note);
            _noteRepo.Save();
            return NoContent();
        }
    }
}
