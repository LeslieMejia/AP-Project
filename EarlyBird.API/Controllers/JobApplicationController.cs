using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;
using EarlyBird.Model.Repositories;
using System.Collections.Generic;

namespace EarlyBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        protected JobApplicationRepository Repository { get; }

        public JobApplicationController(JobApplicationRepository repository)
        {
            Repository = repository;
        }

        // GET: api/JobApplication/{id}
        [HttpGet("{id}")]
        public ActionResult<JobApplication> GetJobApplication([FromRoute] int id)
        {
            JobApplication? app = Repository.GetJobApplicationById(id);
            if (app == null)
            {
                return NotFound($"Job application with id {id} not found");
            }
            return Ok(app);
        }

        // GET: api/JobApplication
        [HttpGet]
        public ActionResult<IEnumerable<JobApplication>> GetJobApplications()
        {
            List<JobApplication> apps = Repository.GetJobApplications();
            if (apps == null || apps.Count == 0)
            {
                return NotFound("No job applications found");
            }
            return Ok(apps);
        }

        // POST: api/JobApplication
        [HttpPost]
        public ActionResult PostJobApplication([FromBody] JobApplication jobApp)
        {
            if (jobApp == null)
            {
                return BadRequest("Job application data not provided");
            }

            bool status = Repository.InsertJobApplication(jobApp);
            if (status)
            {
                return Ok("Job application created successfully");
            }
            return BadRequest("Failed to create job application");
        }

        // PUT: api/JobApplication
        [HttpPut]
        public ActionResult UpdateJobApplication([FromBody] JobApplication jobApp)
        {
            if (jobApp == null)
            {
                return BadRequest("Job application data not provided");
            }

            JobApplication? existingApp = Repository.GetJobApplicationById(jobApp.ApplicationId);
            if (existingApp == null)
            {
                return NotFound($"Job application with id {jobApp.ApplicationId} not found");
            }

            bool status = Repository.UpdateJobApplication(jobApp);
            if (status)
            {
                return Ok("Job application updated successfully");
            }
            return BadRequest("Failed to update job application");
        }

        // DELETE: api/JobApplication/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteJobApplication([FromRoute] int id)
        {
            JobApplication? existingApp = Repository.GetJobApplicationById(id);
            if (existingApp == null)
            {
                return NotFound($"Job application with id {id} not found");
            }

            bool status = Repository.DeleteJobApplication(id);
            if (status)
            {
                return NoContent(); // Successfully deleted, no content to return
            }
            return BadRequest($"Unable to delete job application with id {id}");
        }
    }
}