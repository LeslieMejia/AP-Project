using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;         // Your entity classes (Job, etc.)
using EarlyBirdAPI.Model.Repositories;     // Your repository classes

namespace EarlyBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        // Use dependency injection to get your repository
        protected JobRepository Repository { get; }

        public JobController(JobRepository repository)
        {
            Repository = repository;
        }

        // GET: api/Job/5
        [HttpGet("{id}")]
        public ActionResult<Job> GetJob([FromRoute] int id)
        {
            Job job = Repository.GetJobById(id);
            if (job == null)
            {
                return NotFound($"Job with id {id} not found");
            }
            return Ok(job);
        }

        // GET: api/Job
        [HttpGet]
        public ActionResult<IEnumerable<Job>> GetJobs()
        {
            return Ok(Repository.GetJobs());
        }

        // POST: api/Job
        [HttpPost]
        public ActionResult Post([FromBody] Job job)
        {
            if (job == null)
            {
                return BadRequest("Job info not provided");
            }

            bool status = Repository.InsertJob(job);
            if (status)
            {
                return Ok("Job created successfully");
            }
            return BadRequest("Failed to create job");
        }

        // PUT: api/Job
        [HttpPut]
        public ActionResult UpdateJob([FromBody] Job job)
        {
            if (job == null)
            {
                return BadRequest("Job info not provided");
            }

            // Check if the job exists
            Job existingJob = Repository.GetJobById(job.JobId);
            if (existingJob == null)
            {
                return NotFound($"Job with id {job.JobId} not found");
            }

            bool status = Repository.UpdateJob(job);
            if (status)
            {
                return Ok("Job updated successfully");
            }
            return BadRequest("Failed to update job");
        }

        // DELETE: api/Job/5
        [HttpDelete("{id}")]
        public ActionResult DeleteJob([FromRoute] int id)
        {
            Job existingJob = Repository.GetJobById(id);
            if (existingJob == null)
            {
                return NotFound($"Job with id {id} not found");
            }
            bool status = Repository.DeleteJob(id);
            if (status)
            {
                return NoContent(); // Successfully deleted, no content to return.
            }
            return BadRequest($"Unable to delete job with id {id}");
        }
    }
}