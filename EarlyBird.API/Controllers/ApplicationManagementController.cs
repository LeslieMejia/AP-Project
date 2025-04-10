using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;
using EarlyBirdAPI.Model.Repositories;

namespace EarlyBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationManagementController : ControllerBase
    {
        protected ApplicationManagementRepository Repository { get; }

        public ApplicationManagementController(ApplicationManagementRepository repository)
        {
            Repository = repository;
        }

        // GET: api/ApplicationManagement/5
        [HttpGet("{id}")]
        public ActionResult<ApplicationManagement> GetApplicationManagement([FromRoute] int id)
        {
            ApplicationManagement result = Repository.GetManagementById(id);
            if (result == null)
            {
                return NotFound($"Application management entry with ID {id} not found.");
            }
            return Ok(result);
        }

        // GET: api/ApplicationManagement
        [HttpGet]
        public ActionResult<IEnumerable<ApplicationManagement>> GetAllApplicationManagements()
        {
            return Ok(Repository.GetAllManagements());
        }

        // POST: api/ApplicationManagement
        [HttpPost]
        public ActionResult Post([FromBody] ApplicationManagement item)
        {
            if (item == null)
            {
                return BadRequest("Application management data not provided.");
            }

            bool status = Repository.InsertManagement(item);
            return status
                ? Ok("Application management entry created successfully.")
                : BadRequest("Failed to create application management entry.");
        }

        // PUT: api/ApplicationManagement
        [HttpPut]
        public ActionResult UpdateApplicationManagement([FromBody] ApplicationManagement item)
        {
            if (item == null)
            {
                return BadRequest("Application management data not provided.");
            }

            ApplicationManagement existing = Repository.GetManagementById(item.ManagementId);
            if (existing == null)
            {
                return NotFound($"Application management entry with ID {item.ManagementId} not found.");
            }

            bool status = Repository.UpdateManagement(item);
            return status
                ? Ok("Application management entry updated successfully.")
                : BadRequest("Failed to update application management entry.");
        }

        // DELETE: api/ApplicationManagement/5
        [HttpDelete("{id}")]
        public ActionResult DeleteApplicationManagement([FromRoute] int id)
        {
            ApplicationManagement existing = Repository.GetManagementById(id);
            if (existing == null)
            {
                return NotFound($"Application management entry with ID {id} not found.");
            }

            bool status = Repository.DeleteManagement(id);
            return status
                ? NoContent()
                : BadRequest($"Unable to delete application management entry with ID {id}.");
        }
    }
}
