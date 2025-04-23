using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;         // Your entity classes (User, etc.)
using EarlyBird.Model.Repositories;
using EarlyBirdAPI.Model.Repositories;

namespace EarlyBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        // Dependency injection of the repository
        protected JobApplicationRepository Repository { get; }

        public JobApplicationController(JobApplicationRepository repository)
        {
            Repository = repository;
        }

        // GET: api/JobApplication/{id}
        // Returns a specific job application by ID
        [HttpGet("{id}")]
        public ActionResult<JobApplication> GetJobApplication([FromRoute] int id)
        {
            JobApplication application = Repository.GetJobApplicationById(id);
            if (application == null)
            {
                return NotFound(); // Returns 404 if not found
            }
            return Ok(application); // Returns 200 with application
        }

        // GET: api/JobApplication
        // Returns all job applications
        [HttpGet]
        public ActionResult<IEnumerable<JobApplication>> GetJobApplications()
        {
            return Ok(Repository.GetJobApplications()); // Returns 200 with list
        }

        // POST: api/JobApplication
        // Creates a new job application
        [HttpPost]
        public ActionResult Post([FromBody] JobApplication application)
        {
            if (application == null)
            {
                return BadRequest("Job application data is not correct"); // 400 if body is null
            }

            bool status = Repository.InsertJobApplication(application);
            if (status)
            {
                return Ok(); // 200 if insert successful
            }

            return BadRequest("Failed to insert job application"); // 400 on failure
        }

        // PUT: api/JobApplication
        // Updates an existing job application
        [HttpPut]
        public ActionResult UpdateJobApplication([FromBody] JobApplication application)
        {
            if (application == null)
            {
                return BadRequest("Job application data is not correct"); // 400 if input is null
            }

            JobApplication existing = Repository.GetJobApplicationById(application.Id);
            if (existing == null)
            {
                return NotFound($"Job application with id {application.Id} not found"); // 404 if not found
            }

            bool status = Repository.UpdateJobApplication(application);
            if (status)
            {
                return Ok(); // 200 if update successful
            }

            return BadRequest("Something went wrong while updating"); // 400 if update fails
        }

        // DELETE: api/JobApplication/{id}
        // Deletes a specific job application by ID
        [HttpDelete("{id}")]
        public ActionResult DeleteJobApplication([FromRoute] int id)
        {
            JobApplication existing = Repository.GetJobApplicationById(id);
            if (existing == null)
            {
                return NotFound($"Job application with id {id} not found"); // 404 if not found
            }

            bool status = Repository.DeleteJobApplication(id);
            if (status)
            {
                return NoContent(); // 204 if deletion successful
            }

            return BadRequest($"Unable to delete job application with id {id}"); // 400 if deletion fails
        }
    }
}
