using Microsoft.AspNetCore.Mvc;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Services;

namespace ProductManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly IJwtTokenService _jwtTokenService;

    //public UsersController(
    //    IUserService userService)
    //{
    //    _userService = userService;
    //}

    public UsersController(
    IUserService userService,
    IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(
     UserRegistrationDto dto)
    {
        try
        {
            var user =
                await _userService.RegisterAsync(dto);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    //[HttpPost("login")]
    //public async Task<ActionResult<LoginResponseDto>>
    //    Login(LoginRequestDto dto)
    //{
    //    var isValid =
    //        await _userService.ValidateUserAsync(
    //            dto.Username,
    //            dto.Password);

    //    var response =
    //        new LoginResponseDto
    //        {
    //            IsSuccess = isValid,
    //            Message = isValid
    //                ? "Login successful"
    //                : "Invalid username or password"
    //        };

    //    return Ok(response);
    //}


    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>>
    Login(LoginRequestDto dto)
    {
        var user =
            await _userService.ValidateUserAsync(
                dto.Username,
                dto.Password);

        if (user == null)
        {
            return Ok(new LoginResponseDto
            {
                IsSuccess = false,
                Message = "Invalid username or password"
            });
        }


        var token =_jwtTokenService.GenerateToken( user);


        return Ok(new LoginResponseDto
        {
            IsSuccess = true,
            Message = "Login successful",
            Name = user.Name,
            Username = user.Username,
            UserId=user.Id,
            Token = token
        });
    }

}