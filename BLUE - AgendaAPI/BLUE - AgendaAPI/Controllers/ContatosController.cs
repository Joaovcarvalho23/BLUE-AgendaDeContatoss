using Agenda.Application.Services;
using Agenda.Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatosController : Controller
    {
        private readonly ContatoService _contatoService;

        public ContatosController (ContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [HttpGet("BuscarTodosContatos")]
        public async Task<IActionResult> BuscarTodosContatos()
        {
            var contatos = await _contatoService.GetAllAsync();
            if (!contatos.Any())
                return Ok(new { mensagem = "Nenhum contato encontrado.", contatos });

            return Ok(contatos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarContatoPorId(int id)
        {
            var contato = await _contatoService.GetByIdAsync(id);

            if (contato == null)
                return NotFound(new { mensagem = "Contato não encontrado." });

            return Ok(contato);
        }

        [HttpPost("CriarContato")]
        public async Task<IActionResult> CriarContato([FromBody] Contato contato)
        {
            var criado = await _contatoService.AddAsync(contato);

            if (!criado)
                return BadRequest(new { mensagem = "Dados inválidos. Preencha todos os campos obrigatórios." });

            return CreatedAtAction(nameof(BuscarContatoPorId), new { id = contato.Id }, new { mensagem = "Contato criado com sucesso.", contato });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditarContato(int id, [FromBody] Contato contato)
        {
            var atualizado = await _contatoService.UpdateAsync(id, contato);

            if (!atualizado)
                return BadRequest(new { mensagem = "Contato não encontrado ou dados inválidos." });

            return Ok(new { mensagem = "Contato atualizado com sucesso." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ExcluirContato(int id)
        {
            var excluido = await _contatoService.DeleteAsync(id);

            if (!excluido)
                return NotFound(new { mensagem = "Contato não encontrado." });

            return Ok(new { mensagem = "Contato excluído com sucesso." });
        }
    }
}