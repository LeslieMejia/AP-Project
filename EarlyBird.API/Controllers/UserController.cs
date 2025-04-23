using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model.Entities;         // Your entity classes (User, etc.)
using EarlyBird.Model.Repositories;         // Your repository classes

namespace EarlyBird.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/user
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            var success = _userRepository.InsertUser(user);
            if (success)
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            return BadRequest("Could not insert user.");
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("ID mismatch.");

            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
                return NotFound();

            var success = _userRepository.UpdateUser(user);
            return success ? NoContent() : StatusCode(500, "Update failed.");
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
                return NotFound();

            var success = _userRepository.DeleteUser(id);
            return success ? NoContent() : StatusCode(500, "Delete failed.");
        }
    }
}
