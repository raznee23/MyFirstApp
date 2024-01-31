using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using Models.Domain.Entites;
using Models.DTO.User;
using Services.User;
using System.Globalization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthenticatedUserDTO _loggedInUser;
        public UsersController(IUserService userService, IHttpContextAccessor accessor)
        {
            _userService = userService;
            _loggedInUser = (AuthenticatedUserDTO)accessor?.HttpContext?.Items["User"];
        }

        [HttpPost("signup")]
        public async Task<APIResponse> Signup([FromBody] SignupRequestDTO request)
        {
            var result = await _userService.SignupAsync(request);

            // Return 200 OK if signup is successful
            if (result) return new APIResponse(true, StatusCodes.Status200OK, "Success", "Success");

            return new APIResponse(false, StatusCodes.Status400BadRequest, "BadRequest", null);
        }

        [HttpPost("authenticate")]
        public async Task<APIResponse> Authenticate([FromBody] AuthenticateRequestDTO request)
        {
            // Call the appropriate service method to authenticate the user
            var user = await _userService.AuthenticateAsync(request);

            // Return 401 Unauthorized if authentication fails
            if (user == null)
                return new APIResponse(false, StatusCodes.Status401Unauthorized, "Unauthorized", null);

            // Generate a JWT token
            return new APIResponse(true, StatusCodes.Status201Created, "Success", user);
        }


        [Helpers.FilterHandlers.Authorize]
        [HttpGet("auth/balance")]
        public async Task<APIResponse> GetBalance()
        {
            decimal balance = await _userService.GetUserBalanceAsync(_loggedInUser);

            return new APIResponse(true, StatusCodes.Status201Created, "Success", balance);
        }
        [AllowAnonymous]
        [HttpGet("ping")]
        [Produces("application/json")]
        public async Task<IActionResult> Ping()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessTokenExpiration = await HttpContext.GetTokenAsync("expires_at");
                DateTimeOffset dtExpires = DateTimeOffset.Parse(accessTokenExpiration, CultureInfo.InvariantCulture);
                int accessTokenExpiresIn = (int)dtExpires.Subtract(DateTimeOffset.UtcNow).TotalSeconds;
                if (accessTokenExpiresIn <=90)
                {
                    return Ok("refresh_required");
                }
                return Ok("session_alive");
            }

            return Ok("login_required");
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            if (User.Identity.IsAuthenticated)
            {
                string accessTokenExpiration = await HttpContext.GetTokenAsync("expires_at");
                DateTimeOffset dtExpires = DateTimeOffset.Parse(accessTokenExpiration, CultureInfo.InvariantCulture);
                int accessTokenExpiresIn = (int)dtExpires.Subtract(DateTimeOffset.UtcNow).TotalSeconds;
                if (accessTokenExpiresIn <= 90)
                {
                    return Ok("refresh_required");
                }
                return Ok("session_alive");
            }

            return Ok("login_required");

           // string? accessToken = tokenModel.AccessToken;
           // string? refreshToken = tokenModel.RefreshToken;

           // //var principal = GetPrincipalFromExpiredToken(accessToken);
           // //if (principal == null)
           // //{
           // //    return BadRequest("Invalid access token or refresh token");
           // //}

           // //string username = principal.Identity.Name;

           // //var user = await _userManager.FindByNameAsync(username);

           // //if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
           // //{
           // //    return BadRequest("Invalid access token or refresh token");
           // //}

           // //var newAccessToken = CreateToken(principal.Claims.ToList());
           // var newRefreshToken = string.Empty;// GenerateRefreshToken();

           // //user.RefreshToken = newRefreshToken;
           //// await _userService.UpdateAsync(user);
            
           // return new ObjectResult(new
           // {
           //    // accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
           //     refreshToken = newRefreshToken
           // });
        }
    }
}
