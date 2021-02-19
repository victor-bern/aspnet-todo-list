using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo.Data;
using todo.Models;
using todo.Services;

namespace todo.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetAll([FromServices] DataContext context)
        {
            var users = await context.Users
            .Include(x => x.Todos)
            .AsNoTracking()
            .ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model, [FromServices] DataContext context)
        {
            var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Name == model.Name && x.Password == model.Password)
            .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha Inválidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<User>> Update([FromServices] DataContext context, [FromBody] User model, int id)
        {
            var user = context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                model.Id = user.Id;
                context.Entry<User>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Usuário já foi atualizado" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Ocorreu um erro ao atualizar" });

            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<string>> Delete([FromServices] DataContext context, int id)
        {
            var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new { message = "Usuário deletado com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Ocorreu um erro ao excluir o usuário" });
            }
        }

    }
}