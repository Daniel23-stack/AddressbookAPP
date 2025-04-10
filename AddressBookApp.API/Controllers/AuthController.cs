using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var (success, token, user) = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            
            if (!success)
                return Unauthorized(new AuthResponseDto { Success = false, Message = "Invalid email or password" });
                
            return Ok(new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = _mapper.Map<UserDto>(user)
            });
        }
        
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if (registerDto.Password != registerDto.ConfirmPassword)
                return BadRequest(new AuthResponseDto { Success = false, Message = "Passwords do not match" });
                
            var (success, message) = await _authService.RegisterAsync(
                registerDto.Email, 
                registerDto.Password, 
                registerDto.FirstName, 
                registerDto.LastName);
                
            if (!success)
                return BadRequest(new AuthResponseDto { Success = false, Message = message });
                
            return StatusCode(StatusCodes.Status201Created, new AuthResponseDto
            {
                Success = true,
                Message = message
            });
        }
    }
}
