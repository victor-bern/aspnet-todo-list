using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo.Data;
using todo.Models;

namespace todo.Controllers
{
    [Route("todos")]
    public class TodoController : Controller
    {

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Todo>>> GetAll([FromServices] DataContext context)
        {
            var todos = await context.Todos
            .AsNoTracking().
            ToListAsync();

            return Ok(todos);
        }

        [HttpPost]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Todo>> Post([FromServices] DataContext context, [FromBody] Todo model, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                model.UserId = user.Id;
                context.Todos.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível criar a tarefa" });
            }
        }

        [HttpPatch]
        [Route("done/{id:int}")]
        [Authorize]
        public async Task<ActionResult<string>> DoneTodo([FromServices] DataContext context, int id)
        {
            var todo = await context.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound(new { message = "Tarefa não encontrada" });

            try
            {
                todo.IsDone = true;
                context.Entry<Todo>(todo).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "Tarefa Concluida", todo });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Houve um erro ao atualizar a tarefa" });
            }
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<string>> Delete([FromServices] DataContext context, int id)
        {
            var todo = await context.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok(new { message = "Tarefa deletada com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Ocorreu um erro ao excluir a tarefa" });
            }
        }

    }
}