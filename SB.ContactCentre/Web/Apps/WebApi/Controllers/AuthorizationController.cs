using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using SB.SharedModels.Authentication;
using SB.WebShared.DynamicsAuthentication;
using SB.WebShared.Dynamics;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthorizationController: ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationController(IAuthenticationService authentication)
        {
            _authenticationService = authentication;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginDataModel login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login.UserName))
                {
                    return BadRequest($"{nameof(login.UserName)} is null, empty or white-space");
                }
                if (string.IsNullOrWhiteSpace(login.UserPassword))
                {
                    return BadRequest($"{nameof(login.UserPassword)} is null, empty or white-space");
                }

                var connectionString = _authenticationService.GetConnectionString();

                using var serviceClient = new ServiceClient(connectionString);

                var query = new QueryExpression(SBCustomSettingsModel.LogicalName)
                {
                    ColumnSet = new ColumnSet(SBCustomSettingsModel.Fields.UserName, SBCustomSettingsModel.Fields.UserPassword, SBCustomSettingsModel.Fields.TokenLifetime),
                    TopCount = 10
                };

                var settings =  serviceClient.RetrieveMultipleAsync(query).Result.Entities.FirstOrDefault();

                if (settings == null)
                {
                    return BadRequest("SB Custom settings not found. Please configure system or contact the system administrator for support");
                }

                string userName, userPassword;

                using (var sha256 = SHA256.Create())
                {
                    var userNameHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(settings.GetAttributeValue<string>(SBCustomSettingsModel.Fields.UserName)));
                    var userPasswordHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(settings.GetAttributeValue<string>(SBCustomSettingsModel.Fields.UserPassword)));

                    userName = BitConverter.ToString(userNameHashedBytes).Replace("-", "");
                    userPassword = BitConverter.ToString(userPasswordHashedBytes).Replace("-", "");

                }

                if (login.UserName != userName || login.UserPassword != userPassword)
                {
                    return BadRequest("Invalid username or password");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login.UserName)
                };

                var claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);

                var now = DateTime.UtcNow;

                var lifeTime = settings.GetAttributeValue<int>(SBCustomSettingsModel.Fields.TokenLifetime);

                if (lifeTime <= 0)
                {
                    return BadRequest("Invalid TokenLifetime");
                }

                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    expires: now.AddMinutes(lifeTime),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var cookieOptions = new CookieOptions
                {
                    Expires = now.AddMinutes(lifeTime), 
                    Domain = Request.Host.Value, 
                    Path = "/"
                };

                Response.Cookies.Append(AuthOptions.KEY, encodedJwt, cookieOptions);

                var response = new TokenModel
                {
                    Token = encodedJwt
                };

                return Ok(response);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}