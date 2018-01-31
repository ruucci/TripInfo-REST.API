using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;
using TripInfoREST.API.Models;
using TripInfoREST.API.Services;

namespace TripInfoREST.API.Controllers
{
    [Authorize]
    [Route("users")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;
        private IMailService _mailService;
        private ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            IOptions<AppSettings> appSettings,
            IMailService mailService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
            _mailService = mailService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserForAuthenticationDto userDto)
        {
            if(userDto == null)
            {
                return BadRequest();
            }

            var user = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]UserForCreationDto userDto)
        {
            if(userDto==null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

            try
            {
                // map dto to entity
                var user = Mapper.Map<User>(userDto);

                // save 
                _userService.Create(user, userDto.Password);

                var userToReturn = Mapper.Map<UserDto>(user);
                return CreatedAtRoute("GetUserById", new { id = userToReturn.Id }, userToReturn);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                _logger.LogInformation(100, ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = Mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}",Name = "GetUserById")]
        public IActionResult GetUserById(Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = _userService.GetById(id);
            var userDto = Mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]UserForUpdateDto userDto)
        {
            if(userDto == null || id == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

            try
            {
                // map dto to entity and set id
                var user = Mapper.Map<User>(userDto);
                user.Id = id;

                // save 
                _userService.Update(user, userDto.Password);
                return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                _logger.LogInformation(100, ex.ToString());
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            var user = _userService.GetById(id);
            var userDto = Mapper.Map<UserDto>(user);

            if(userDto == null)
            {
                return NotFound();
            }

            _userService.Delete(userDto.Id);
            _mailService.Send("User has been deleted.", $"User {userDto.FirstName} with Id {userDto.Id} was deleted");

            return NoContent();
        }
    }
}
