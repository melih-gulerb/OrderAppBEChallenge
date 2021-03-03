using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Controllers
{

    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        //This method is basically if correct user and pass
        //strings are given, gives a token to user
        [HttpGet]
        public IActionResult InsertCoin(string user, string pass)
        {
            if (user == "melih" && pass == "pass") //if user is melih  and pass is pass, return true 
            {
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };
                var regKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("B6S4B5avsySsHKdmdasoCsOsz9KdIVCfOs6kvF==")); // Assign secret key as regKey
                var tokenValidationParameters = new TokenValidationParameters()  // validation info of token
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "localhost",
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudience = "Melih",
                    IssuerSigningKey = regKey,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };

                var jwt = new JwtSecurityToken(
                    claims: claims,
                    issuer: "localhost",
                    audience: "Melih",
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromHours(2)),
                    signingCredentials: new SigningCredentials(regKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt); //Write created token
                var tokenInfo = $"Here is the token.. Now it can be used for CRUD operations on microservices {encodedJwt}";
                var responseJson = new
                {
                    write_token = tokenInfo,  //Token info
                    expires = (int)TimeSpan.FromHours(2).TotalHours  //Time 
                };
                return Ok(responseJson);    //If method InsertCoin() is acted, return token and its expiration time
            }

            if (user != "melih" && pass == "pass")
            {
                var responseJson = "Password is correct but incorrect user name";
                return BadRequest(responseJson);
            }

            if (user =="melih" && pass != "pass")
            {
                var responseJson = "Username is correct but incorrect password";
                return BadRequest(responseJson);
            }

            const string jsonResponse = "You should authorize to get token\nCorrect http address\n ->" +
                                        "http:localhost://44347/api/authorize?user=***&pass=***";
            return NotFound(jsonResponse); //If localhost://44347/api/authorize?user=melih&pass=pass is not reached, return Status404
        }
    }
}