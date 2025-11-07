using Application.Intrfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Dpepartment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LetterTypeController : ControllerBase
    {
        private readonly ILetterTypeRepository _repo;

        public LetterTypeController(ILetterTypeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var types = _repo.GetAll();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var type = _repo.Get(id);
            if (type == null) return NotFound();
            return Ok(type);
        }

        [HttpPost]
        public IActionResult Create([FromBody] LetterType type)
        {
            _repo.Add(type);
            _repo.Save();
            return Ok(type);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] LetterType type)
        {
            var existing = _repo.Get(id);
            if (existing == null) return NotFound();

            existing.Title = type.Title;
            _repo.Update(existing);
            _repo.Save();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var type = _repo.Get(id);
            if (type == null) return NotFound();

            _repo.Delete(type);
            _repo.Save();
            return NoContent();
        }
    }
}
