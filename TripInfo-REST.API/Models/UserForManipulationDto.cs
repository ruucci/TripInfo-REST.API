using System.ComponentModel.DataAnnotations;

namespace TripInfoREST.API.Models
{
    public class UserForManipulationDto
    {
        [Required(ErrorMessage = "First Name is a required field.")]
        [MaxLength(50, ErrorMessage = "The First Name shouldn't have more than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is a required field.")]
        [MaxLength(50, ErrorMessage = "The Last Name shouldn't have more than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is a required field.")]
        [MaxLength(50, ErrorMessage = "The Username shouldn't have more than 100 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        [MaxLength(50, ErrorMessage = "The Password shouldn't have more than 100 characters.")]
        public string Password { get; set; }

    }
}