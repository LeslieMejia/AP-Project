using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;         // Your entity classes (Job, etc.)
using EarlyBirdAPI.Model.Repositories;     // Your repository classes

namespace EarlyBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        protected JobRepository Repository { get; }

        public JobController(JobRepository repository)
        {
            Repository = repository;
        }
    
        // GET - Retrieve a specific job by ID
        [HttpGet("{id}")]
        public ActionResult<Job> GetJob([FromRoute] int id)
        {
            Job job = Repository.GetJobById(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }
    
         // GET - Retrieve all jobs
        [HttpGet]
        public ActionResult<IEnumerable<Job>> GetJobs()
        {
            return Ok(Repository.GetJobs());
        }

         // POST - Insert a new job
        [HttpPost]
        public ActionResult Post([FromBody] Job job)
        {
            if (job == null)
            {
                return BadRequest("Job data is not correct");
            }
            bool status = Repository.InsertJob(job);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Failed to insert job");
        }
        
        // PUT - Update an existing job
        [HttpPut]
        public ActionResult UpdateJob([FromBody] Job job)
        {
            if (job == null)
            {
                return BadRequest("Job data is not correct");
            }

            Job existingJob = Repository.GetJobById(job.Id);
            if (existingJob == null)
            {
                return NotFound($"Job with id {job.Id} not found");
            }

            bool status = Repository.UpdateJob(job);
            if (status)
            {
                return Ok();
            }

            return BadRequest("Something went wrong while updating");
        }

        // DELETE - Delete a job by ID
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
                return NoContent();
            }

            return BadRequest($"Unable to delete job with id {id}");
        }
    }
}
