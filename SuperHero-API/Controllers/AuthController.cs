using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperHero_API.Models;
using SuperHero_API.Models.DTOs.SuperHero;
using SuperHero_API.Models.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SuperHero_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // lo creamos estatico para usarlo en todo el controlador
        public static User user = new User();
        private readonly IConfiguration _configuration;


        // cada vez que se inicia la aplicacion el controllador carga la configuracion 
        // que nos permitira obtener el appsettings por medio de GetSection
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
             user.UserName = request.UserName;
             user.PasswordHash = passwordHash;
             user.PasswordSalt = passwordSalt;

             return Ok(user);
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> login(UserDto request)
        {
            ModelState.AddModelError("Error","El usuario no esta registrado");
            if (user.UserName != request.UserName) 
            {
                return ValidationProblem(ModelState);           
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("La contraseña no coinside con la contraseña del usuario ingresado");
            }

            string token = CreateToken(user);
            return Ok(token); // se retorna el token creado
        }
        // metodo para crear el token
        private string CreateToken(User user)
        {
            // los claims son el contenido de nuestro token 
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };


            // Por medio de la interface IConfiguration accedemos al metodo de
            // GetSection para obtener nuestro token localizado en appsettings.json
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            // ahora necesitamos las credenciales de acceso
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            // 
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        // // password hash method that use a crytography algorithm to create passwordSalt and the password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            
            }
        }
        // metodo que verifica el passwordhash
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
                
            }
        }

    }
}
