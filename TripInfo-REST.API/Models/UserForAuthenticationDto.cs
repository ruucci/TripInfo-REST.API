using System;
namespace TripInfoREST.API.Models
{
    public class UserForAuthenticationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
