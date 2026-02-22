using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>();
        private static int nextId = 1;


        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            try
            {
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            try
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            try
            {
                var validationError = ValidateUser(user);
                if (validationError != null)
                    return BadRequest(validationError);
                user.Id = nextId++;
                users.Add(user);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser)
        {
            try
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");
                var validationError = ValidateUser(updatedUser);
                if (validationError != null)
                    return BadRequest(validationError);
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");
                users.Remove(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private string? ValidateUser(User user)
        {
            if (user == null)
                return "User data is required.";
            if (string.IsNullOrWhiteSpace(user.FirstName))
                return "First name is required.";
            if (string.IsNullOrWhiteSpace(user.LastName))
                return "Last name is required.";
            if (string.IsNullOrWhiteSpace(user.Email))
                return "Email is required.";
            if (!IsValidEmail(user.Email))
                return "Email format is invalid.";
            return null;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}