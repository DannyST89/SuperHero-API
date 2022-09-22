using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SuperHero_API.Data;
using SuperHero_API.Models;
using SuperHero_API.Models.DTOs.SuperHero;

namespace SuperHero_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        //variables globales
        private readonly DataContext _context;
        // creamos el constructor de la clase
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Por medio del DTO obtenermos todos los superHeroes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<SuperHeroGet>>> GetSuperHeroes()
        {
            return await _context.SuperHeroes.Select(SuperHero => new SuperHeroGet()
            {
                Id = SuperHero.Id,
                Name = SuperHero.Name,
                FirstName = SuperHero.FirstName,
                LastName = SuperHero.LastName,
                Place = SuperHero.Place
            }).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroGet>> GetOneSuperHero(int id)
        {
            ModelState.AddModelError("Error", "El Hero no esta registrado en la base de datos");
            var existHero = await _context.SuperHeroes.FindAsync(id);

            if (existHero == null)
            {
                return ValidationProblem(ModelState);
            }
            SuperHeroGet getOneSuperHero = new SuperHeroGet()
            {
                Id = existHero.Id,
                Name = existHero.Name,
                FirstName = existHero.FirstName,
                LastName = existHero.LastName,
                Place = existHero.Place
            };
            return getOneSuperHero;
        }
        [HttpPost]
        public async Task<ActionResult<List<SuperHeroPost>>> CreateSuperHero(SuperHeroPost _superHeroPost)
        {
            var newSuperHero = new SuperHero
            {
                Name = _superHeroPost.Name,
                FirstName = _superHeroPost.FirstName,
                LastName = _superHeroPost.LastName,
                Place = _superHeroPost.Place
            };
            /*
             Se verifica si el dato existe en la base de datos
             si existe, se notifica al usuario que no puede ingresar el datos
             ya que existe una igual

            var existeEmail = await _context.SuperUsuario.AnyAsync(x => x.Email.Equals(superUsuarioPOST.Email));            
            if (existeEmail)
            {
                ModelState.AddModelError("EmailNoExiste", "El Email ya se encuentra en la Base de datos");
                return ValidationProblem(_modelState);
            }
            */
            _context.SuperHeroes.Add(newSuperHero);
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHeroPut>>> UpdateSuperHero(SuperHeroPost updateHero, int id)
        {
            // ModelState es heredado de ControllerBase
            ModelState.AddModelError("Error", "El Hero no esta registrado en la base de datos");
           
            // primeramente revisamos si el hero existe en la base de datos
            var dbHero = await _context.SuperHeroes.FindAsync(id);
            if ( dbHero == null)
            {
                return ValidationProblem(ModelState);
            }
            // si el id existe podemos actualizar los datos
            dbHero.Name = updateHero.Name;
            dbHero.FirstName = updateHero.FirstName;
            dbHero.LastName = updateHero.LastName;
            dbHero.Place = updateHero.Place;

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {

            ModelState.AddModelError("Error", "El Hero no esta registrado en la base de datos");
            var existHero = await _context.SuperHeroes.FindAsync(id);

            if(existHero == null)
            {
                return ValidationProblem(ModelState);
            }
            _context.SuperHeroes.Remove(existHero);
            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
