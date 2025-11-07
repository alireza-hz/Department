using Application.Intrfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Dpepartment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LetterFlowController : ControllerBase
    {
        private readonly IletterFlowRepozitory _flowRepo;
        private readonly IletterRepozitory _letterRepo;

        public LetterFlowController(IletterFlowRepozitory flowRepo, IletterRepozitory letterRepo)
        {
            _flowRepo = flowRepo;
            _letterRepo = letterRepo;
        }

        [HttpGet("by-letter/{letterId}")]
        public IActionResult GetByLetter(int letterId)
        {
            var flows = _flowRepo.GetFlowsByLetter(letterId);
            return Ok(flows);
        }

        [HttpGet("by-user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            var flows = _flowRepo.GetFlowsByUser(userId);
            return Ok(flows);
        }

        [HttpPost("send")]
        public IActionResult SendLetter([FromBody] LetterFlow flow)
        {
            var letter = _letterRepo.Get(flow.LetterId);
            if (letter == null) return NotFound("Letter not found");

            flow.SentAt = DateTime.UtcNow;
            flow.Status = "Sent";

            _flowRepo.Add(flow);
            _flowRepo.Save();
            letter.Status = "Sent";
            _letterRepo.Update(letter);
            _letterRepo.Save();

            return Ok(flow);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] string newStatus)
        {
            var flow = _flowRepo.Get(id);
            if (flow == null) return NotFound();

            flow.Status = newStatus;
            _flowRepo.Update(flow);
            _flowRepo.Save();

            return Ok(flow);
        }
    }
}
