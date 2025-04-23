using System.Collections.Generic;
using EarlyBirdAPI.Model.Entities;
using EarlyBirdAPI.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EarlyBird.API.Controllers
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

        // GET: api/Resume/5
        [HttpGet("{id}")]
        public ActionResult<Resume> GetResume([FromRoute] int id)
        {
            Resume resume = Repository.GetResumeById(id);
            if (resume == null)
            {
                return NotFound();
            }
            return Ok(resume);
        }

        // GET: api/Resume
        [HttpGet]
        public ActionResult<IEnumerable<Resume>> GetResumes()
        {
            return Ok(Repository.GetResumes());
        }

        // POST: api/Resume
        [HttpPost]
        public ActionResult Post([FromBody] Resume resume)
        {
            if (resume == null)
            {
                return BadRequest("Resume info not provided");
            }

            bool status = Repository.InsertResume(resume);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Failed to create resume");
        }

        // PUT: api/Resume
        [HttpPut]
        public ActionResult UpdateResume([FromBody] Resume resume)
        {
            if (resume == null)
            {
                return BadRequest("Resume info not provided");
            }

            Resume existing = Repository.GetResumeById(resume.Id);
            if (existing == null)
            {
                return NotFound($"Resume with id {resume.Id} not found");
            }

            bool status = Repository.UpdateResume(resume);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong updating resume");
        }

        // DELETE: api/Resume/5
        [HttpDelete("{id}")]
        public ActionResult DeleteResume([FromRoute] int id)
        {
            Resume existing = Repository.GetResumeById(id);
            if (existing == null)
            {
                return NotFound($"Resume with id {id} not found");
            }

            bool status = Repository.DeleteResume(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete resume with id {id}");
        }
    }
}
