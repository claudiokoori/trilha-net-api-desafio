using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;



namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var content = _context.Tarefas.FirstOrDefault(op => op.Id == id); 
             
            if ( content == null) {
                return NotFound();
            }
     
            return Ok(content);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var content = _context.Tarefas.ToList();
            return Ok(content);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var content = _context.Tarefas.Where(op => op.Titulo == titulo);

            return Ok(content);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(op => op.Status == status);

            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue) {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
                
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {

            var tarafaExistente = _context.Tarefas.FirstOrDefault(op => op.Id == id);

            if ( tarafaExistente == null) {
                return NotFound();
            }
                
            if (tarefa.Data == DateTime.MinValue) {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
                
            _context.Entry(tarafaExistente).CurrentValues.SetValues(tarefa);
            _context.SaveChanges();
            return Ok(tarafaExistente);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null) {
                return NotFound();
            }
                

            _context.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
