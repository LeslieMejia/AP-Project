using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;
using EarlyBird.Model.Repositories;
using System.Collections.Generic;

namespace EarlyBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        protected ResumeRepository Repository { get; }

        public ResumeController(ResumeRepository repository)
        {
            Repository = repository;
        }

        // GET: api/Resume/{id}
        [HttpGet("{id}")]
        public ActionResult<Resume> GetResume([FromRoute] int id)
        {
            Resume resume = Repository.GetResumeById(id);
            if (resume == null)
            {
                return NotFound($"Resume with id {id} not found");
            }
            return Ok(resume);
        }

        // GET: api/Resume/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Resume>> GetResumesByUser([FromRoute] int userId)
        {
            List<Resume> resumes = Repository.GetResumesByUserId(userId);
            if (resumes == null || resumes.Count == 0)
            {
                return NotFound($"No resumes found for user with id {userId}");
            }
            return Ok(resumes);
        }

        // POST: api/Resume
        [HttpPost]
        public ActionResult PostResume([FromBody] Resume resume)
        {
            if (resume == null)
            {
                return BadRequest("Resume data not provided");
            }

            bool status = Repository.InsertResume(resume);
            if (status)
            {
                return Ok("Resume added successfully");
            }
            return BadRequest("Failed to add resume");
        }

        // PUT: api/Resume
        [HttpPut]
        public ActionResult UpdateResume([FromBody] Resume resume)
        {
            if (resume == null)
            {
                return BadRequest("Resume data not provided");
            }

            Resume existingResume = Repository.GetResumeById(resume.ResumeId);
            if (existingResume == null)
            {
                return NotFound($"Resume with id {resume.ResumeId} not found");
            }

            bool status = Repository.UpdateResume(resume);
            if (status)
            {
                return Ok("Resume updated successfully");
            }
            return BadRequest("Failed to update resume");
        }

        // DELETE: api/Resume/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteResume([FromRoute] int id)
        {
            Resume existingResume = Repository.GetResumeById(id);
            if (existingResume == null)
            {
                return NotFound($"Resume with id {id} not found");
            }

            bool status = Repository.DeleteResume(id);
            if (status)
            {
                return NoContent(); // Successfully deleted
            }
            return BadRequest($"Unable to delete resume with id {id}");
        }
    }
}