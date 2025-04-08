using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;
using EarlyBird.Model.Repositories;
using System.Collections.Generic;

namespace EarlyBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedJobController : ControllerBase
    {
        protected SavedJobRepository Repository { get; }

        public SavedJobController(SavedJobRepository repository)
        {
            Repository = repository;
        }

        // GET: api/SavedJob/{id}
        [HttpGet("{id}")]
        public ActionResult<SavedJob> GetSavedJob([FromRoute] int id)
        {
            SavedJob savedJob = Repository.GetSavedJobById(id);
            if (savedJob == null)
            {
                return NotFound($"Saved job with id {id} not found");
            }
            return Ok(savedJob);
        }

        // GET: api/SavedJob/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<SavedJob>> GetSavedJobsByUser([FromRoute] int userId)
        {
            List<SavedJob> savedJobs = Repository.GetSavedJobsByUserId(userId);
            if (savedJobs == null || savedJobs.Count == 0)
            {
                return NotFound($"No saved jobs found for user with id {userId}");
            }
            return Ok(savedJobs);
        }

        // POST: api/SavedJob
        [HttpPost]
        public ActionResult PostSavedJob([FromBody] SavedJob savedJob)
        {
            if (savedJob == null)
            {
                return BadRequest("Saved job information not provided");
            }

            bool status = Repository.InsertSavedJob(savedJob);
            if (status)
            {
                return Ok("Saved job added successfully");
            }
            return BadRequest("Failed to add saved job");
        }

        // DELETE: api/SavedJob/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteSavedJob([FromRoute] int id)
        {
            SavedJob? savedJob = Repository.GetSavedJobById(id);
            if (savedJob == null)
            {
                return NotFound($"Saved job with id {id} not found");
            }
            bool status = Repository.DeleteSavedJob(id);
            if (status)
            {
                return NoContent(); // Successfully deleted.
            }
            return BadRequest($"Unable to delete saved job with id {id}");
        }
    }
}