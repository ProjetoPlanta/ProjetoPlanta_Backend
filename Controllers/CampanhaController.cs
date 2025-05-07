
using Microsoft.AspNetCore.Mvc;
using ProjetoPlanta_Backend.Models;
using ProjetoPlanta_Backend.ViewModels;
using ProjetoPlanta_Backend.Data;


namespace ProjetoPlanta_Backend.Controllers
{
    [ApiController]
    public class CampanhaController : ControllerBase

    {
        private readonly ProjetoPlanta_Backend.Data.FirestoreService _service;
        public CampanhaController()
        {
            _service = new FirestoreService();
        }

        [HttpPost]
        [Route("Campanhas")]
        public async Task<IActionResult> CadastroCampanhaAsync([FromBody] CampanhaCadastroViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Nome))
                {
                    return BadRequest(new { error = "Nome da Campanha inválido!" });
                }

                var novaCampanha = new Campanha
                {
                    Nome=model.Nome,
                    Imagem=model.Imagem,
                    isAtivo=model.isAtivo,

                };

                string autoId = await _service.AddDocAsync("Campanhas", novaCampanha);
                Console.WriteLine("Documento salvo no Firestore com ID: " + autoId);
                return Ok(new { message = "Campanha cadastrada com sucesso!", id = autoId });
                               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar no Firestore: {ex.Message}");
                return StatusCode(500, new { error = "Erro ao salvar no Firestore", detalhes = ex.Message });
            }
        }
        
        [HttpGet]
        [Route("Campanhas")]
        public async Task<IActionResult> ExibePlantasAsync()
        {
            try
            {
                var campanhas = await _service.getAllDocsAsync<Campanha>("Campanhas");

                if (campanhas == null || campanhas.Count == 0)
                {
                    return NotFound(new { error = "Nenhuma campanha encontrada." });
                }

                return Ok(campanhas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar campanhas no Firestore: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

        [HttpGet]
        [Route("Campanhas/{id}")]
        public async Task<IActionResult> ExibeCampanhaPorIdAsync([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "ID inválido!" });
                }

                var campanha = await _service.getDocAsync<Campanha>("Campanhas", id);

                if (campanha == null)
                {
                    return NotFound(new { error = "Campanha não encontrada!" });
                }

                return Ok(campanha);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar campanha por ID: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

       
        [HttpPut("Campanhas/{id}")]
        public async Task<IActionResult> AtualizaDocAsync([FromRoute] string id, [FromBody] CampanhaAtualizaViewModel model)
        {
            try
            {
                var CampanhaExistente = await _service.getDocAsync<Campanha>("Campanhas", id);

                if (CampanhaExistente == null)
                {
                    return NotFound(new { error = "Planta não encontrada!" });
                }

                CampanhaExistente.Nome = model.Nome;
                CampanhaExistente.Imagem = model.Imagem;
                CampanhaExistente.isAtivo = model.isAtivo;

                await _service.updateDocAsync("Campanhas", id, CampanhaExistente);

                return Ok(new { message = "Campanha atualizada com sucesso!", Campanha = CampanhaExistente });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar campanha: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

        [HttpDelete("Campanhas/{id}")]
        public async Task<IActionResult> DeletaCampanhaAsync([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "ID inválido!" });
                }

                var campanha = await _service.getDocAsync<Campanha>("Campanhas", id);

                if (campanha == null)
                {
                    return NotFound(new { error = "Campanha não encontrada!" });
                }

                await _service.deleteDocAsync("Campanhas", id);
                return Ok(new { message = "Campanha deletada com sucesso!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar Campanha: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }



    }
}
