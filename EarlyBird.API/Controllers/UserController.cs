
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;         // Your entity classes (User, etc.)
using EarlyBird.Model.Repositories;         // Your repository classes

namespace EarlyBirdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Use dependency injection to get your repository
        protected UserRepository Repository { get; }

        public UserController(UserRepository repository)
        {
            Repository = repository;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser([FromRoute] int id)
        {
            User user = Repository.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found");
            }
            return Ok(user);
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Repository.GetUsers());
        }

        // POST: api/User
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User info not provided");
            }

            bool status = Repository.InsertUser(user);
            if (status)
            {
                return Ok("User created successfully");
            }
            return BadRequest("Failed to create user");
        }

        // PUT: api/User
        [HttpPut]
        public ActionResult UpdateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User info not provided");
            }

            // Check if the user exists
            User existingUser = Repository.GetUserById(user.UserId);
            if (existingUser == null)
            {
                return NotFound($"User with id {user.UserId} not found");
            }

            bool status = Repository.UpdateUser(user);
            if (status)
            {
                return Ok("User updated successfully");
            }
            return BadRequest("Failed to update user");
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            User existingUser = Repository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound($"User with id {id} not found");
            }
            bool status = Repository.DeleteUser(id);
            if (status)
            {
                return NoContent(); // Successfully deleted, no content to return.
            }
            return BadRequest($"Unable to delete user with id {id}");
        }
    }
}