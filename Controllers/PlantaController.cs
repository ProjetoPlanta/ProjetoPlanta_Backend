
using Microsoft.AspNetCore.Mvc;
using ProjetoPlanta_Backend.Models;
using ProjetoPlanta_Backend.ViewModels;
using ProjetoPlanta_Backend.Data;


namespace ProjetoPlanta_Backend.Controllers
{
    [ApiController]
    public class PlantaController : ControllerBase

    {
        private readonly ProjetoPlanta_Backend.Data.FirestoreService _service;
        public PlantaController()
        {
            _service = new FirestoreService();
        }

        [HttpPost]
        [Route("plantas")]
        public async Task<IActionResult> CadastroPlantaAsync([FromBody] PlantaCadastroViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.nomePopular))
                {
                    return BadRequest(new { error = "Nome da planta inválido!" });
                }

                var novaPlanta = new Planta
                {
                    nomePopular = model.nomePopular,
                    nomeCientifico = model.nomeCientifico,
                    categoriaGeral = model.categoriaGeral,
                    cicloVida = model.cicloVida,
                    descricao = model.descricao,
                    epocaFloracao = model.epocaFloracao,
                    necessidadeAgua = model.necessidadeAgua,
                    necessidadeLuz = model.necessidadeLuz,
                    necessidadePoda = model.necessidadePoda,
                    porte = model.porte,
                    preco = model.preco,
                    umidadeSolo = model.umidadeSolo,
                    ambiente = model.ambiente,
                    atraiAbelha = model.atraiAbelha,
                    medicinal = model.medicinal,
                    toxicidade = model.toxicidade,
                    imagem = model.imagem
                };

                string autoId = await _service.AddDocAsync("Plantas", novaPlanta);
                Console.WriteLine("Documento salvo no Firestore com ID: " + autoId);
                return Ok(new { message = "Planta cadastrada com sucesso!", id = autoId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar no Firestore: {ex.Message}");
                return StatusCode(500, new { error = "Erro ao salvar no Firestore", detalhes = ex.Message });
            }
        }

        [HttpGet]
        [Route("plantas")]
        public async Task<IActionResult> ExibePlantasAsync()
        {
            try
            {
                var plantas = await _service.getAllDocsAsync<Planta>("Plantas");

                if (plantas == null || plantas.Count == 0)
                    return NotFound(new { error = "Nenhuma planta encontrada." });

                return Ok(plantas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }
        [HttpGet]
        [Route("plantas/{id}")]
        public async Task<IActionResult> ExibePlantaPorIdAsync([FromRoute] string id)
        {
            try
            {
                var planta = await _service.getDocAsync<Planta>("Plantas", id);

                if (planta == null)
                    return NotFound(new { error = "Planta não encontrada!" });

                return Ok(planta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

        [HttpPut("plantas/{id}")]
        public async Task<IActionResult> AtualizaDocAsync([FromRoute] string id, [FromBody] PlantaAtualizaViewModel model)
        {
            try
            {
                var plantaExistente = await _service.getDocAsync<Planta>("Plantas", id);

                if (plantaExistente == null)
                {
                    return NotFound(new { error = "Planta não encontrada!" });
                }

                plantaExistente.nomeCientifico = model.nomeCientifico;
                plantaExistente.categoriaGeral = model.categoriaGeral;
                plantaExistente.cicloVida = model.cicloVida;
                plantaExistente.descricao = model.descricao;
                plantaExistente.epocaFloracao = model.epocaFloracao;
                plantaExistente.necessidadeAgua = model.necessidadeAgua;
                plantaExistente.necessidadeLuz = model.necessidadeLuz;
                plantaExistente.necessidadePoda = model.necessidadePoda;
                plantaExistente.porte = model.porte;
                plantaExistente.preco = model.preco;
                plantaExistente.umidadeSolo = model.umidadeSolo;
                plantaExistente.ambiente = model.ambiente;
                plantaExistente.atraiAbelha = model.atraiAbelha;
                plantaExistente.medicinal = model.medicinal;
                plantaExistente.toxicidade = model.toxicidade;
                plantaExistente.imagem = model.imagem;

                await _service.updateDocAsync("Plantas", id, plantaExistente);

                return Ok(new { message = "Planta atualizada com sucesso!", planta = plantaExistente });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }


        [HttpDelete("plantas/{id}")]
        public async Task<IActionResult> DeletaPlantaAsync([FromRoute] string id)
        {
            try
            {
                var planta = await _service.getDocAsync<Planta>("Plantas", id);

                if (planta == null)
                {
                    return NotFound(new { error = "Planta não encontrada!" });
                }

                await _service.deleteDocAsync("Plantas", id);
                return Ok(new { message = "Planta deletada com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

        
    }
}
