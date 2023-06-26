using AutoMapper;
using HotelApi.Data;
using HotelApi.DTOS.WriteDtos;
using HotelApi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HotelApi.Controllers
{
    [Route("api/[controller")]
    [ApiController]
    public class AccountController :ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;
        public AccountController(
            ILogger<AccountController> logger,
            IMapper mapper,
            SignInManager<ApiUser> signInManager,
            UserManager<ApiUser> userManager,
            IAuthManager authManager)
        {
            _logger = logger;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _authManager = authManager;
        }

        [HttpPost("Login", Name = "Register")]
        public async Task<IActionResult> Register([FromBody] UserWriteDto userWriteDto)
        {
            var user = _mapper.Map<ApiUser>(userWriteDto);
            if (ModelState.IsValid is not true)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userManager.CreateAsync(user, userWriteDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, userWriteDto.Roles);
                    return StatusCode(201, "User Created Successfully");

                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, $"something went wrong in {nameof(Register)}");
                return StatusCode(500, "An error occured. Please try again later");
            }
        }

        [HttpPost("register", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginWriteDto loginInfo)
        {


            if (ModelState.IsValid is not true)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isUserValidated = await _authManager.AuthenticateUserAsync(loginInfo);
                if (isUserValidated)
                {
                    var token = await _authManager.GenerateTokenAsync();
                    return Accepted(new {Token = token});

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, $"something went wrong in {nameof(Register)}");
                return StatusCode(500, "An error occured. Please try again later");
            }
        }
    }
}
