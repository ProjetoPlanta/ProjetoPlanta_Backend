
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
        [Route("Plantas")]
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
                    descricao = model.descricao,
                    comoCuidar = model.comoCuidar,
                    epocaFloracao = model.epocaFloracao,
                    necessidadeAgua = model.necessidadeAgua,
                    necessidadeLuz = model.necessidadeLuz,
                    frequenciaPoda = model.frequenciaPoda,
                    porte = model.porte,
                    preco = model.preco,
                    umidadeSolo = model.umidadeSolo,
                    ambiente = model.ambiente,
                    atraiAbelha = model.atraiAbelha,
                    petFriendly = model.petFriendly,
                    imagem = model.imagem,
                    estoque = model.estoque
                };

                string autoId = await _service.AddDocAsync("Plantas", novaPlanta);
                Console.WriteLine("Documento salvo no Firestore com ID: " + autoId);


                var movimentacao = new Movimentacao
                {
                    plantaId = autoId,
                    quantidade = novaPlanta.estoque,
                    tipoTransacao = "reabastecimento",
                    data = DateTime.UtcNow
                };

                string movimentacaoId = await _service.AddDocAsync("Movimentacoes", movimentacao);
                return Ok(new { message = "Planta cadastrada com sucesso!", id = autoId,
                                message2 = "Gerada movimentação de estoque com ID:", movimentacaoId});
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar no Firestore: {ex.Message}");
                return StatusCode(500, new { error = "Erro ao salvar no Firestore", detalhes = ex.Message });
            }
        }
        [HttpPost("Plantas/Movimentar")]
        public async Task<IActionResult> MovimentarEstoque([FromBody] Movimentacao movimentacao)
        {
            try
            {
                var planta = await _service.getDocAsync<Planta>("Plantas", movimentacao.plantaId);
                if (planta == null)
                {
                    return NotFound(new { message = "Planta não encontrada." });
                }

                if (movimentacao.tipoTransacao == "venda presencial" || movimentacao.tipoTransacao == "venda online")
                {
                    if (planta.estoque < movimentacao.quantidade)
                    {
                        return BadRequest(new { message = "Estoque insuficiente." });
                    }
                    planta.estoque -= movimentacao.quantidade;
                }
                else if (movimentacao.tipoTransacao == "reabastecimento")
                {
                    planta.estoque += movimentacao.quantidade;
                }
                else
                {
                    return BadRequest(new { message = "Tipo de transação inválido." });
                }

                await _service.updateDocAsync("Plantas", movimentacao.plantaId, planta);
                await _service.AddDocAsync("Movimentacoes", movimentacao);
                string autoId = await _service.AddDocAsync("Movimentacoes", movimentacao);
                Console.WriteLine("Documento salvo no Firestore com ID: " + autoId);
                return Ok(new { message = "Movimentação registrada com sucesso!", id = autoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor.", detalhes = ex.Message });
            }
        }


        [HttpGet]
        [Route("Plantas")]
        public async Task<IActionResult> ExibePlantasAsync()
        {
            try
            {
                var plantas = await _service.getAllDocsAsync<Planta>("Plantas");

                if (plantas == null || plantas.Count == 0)
                {
                    return NotFound(new { error = "Nenhuma planta encontrada." });
                }

                return Ok(plantas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar plantas no Firestore: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

        [HttpGet]
        [Route("Plantas/{id}")]
        public async Task<IActionResult> ExibePlantaPorIdAsync([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "ID inválido!" });
                }

                var planta = await _service.getDocAsync<Planta>("Plantas", id);

                if (planta == null)
                {
                    return NotFound(new { error = "Planta não encontrada!" });
                }

                return Ok(planta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar planta por ID: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }

        [HttpGet("Movimentacoes")]
        public async Task<IActionResult> ObterMovimentacoesAsync()
        {
            try
            {
                var movimentacoes = await _service.getAllDocsAsync<Movimentacao>("Movimentacoes");

                if (movimentacoes == null || movimentacoes.Count == 0)
                {
                    return NotFound(new { message = "Nenhuma movimentação encontrada." });
                }

                return Ok(movimentacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor.", detalhes = ex.Message });
            }
        }

        [HttpPut("Plantas/{id}")]
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
                plantaExistente.descricao = model.descricao;
                plantaExistente.comoCuidar = model.comoCuidar;
                plantaExistente.epocaFloracao = model.epocaFloracao;
                plantaExistente.necessidadeAgua = model.necessidadeAgua;
                plantaExistente.necessidadeLuz = model.necessidadeLuz;
                plantaExistente.frequenciaPoda = model.frequenciaPoda; // Corrigido para o nome correto
                plantaExistente.porte = model.porte;
                plantaExistente.preco = model.preco;
                plantaExistente.nomePopular = model.nomePopular;
                plantaExistente.umidadeSolo = model.umidadeSolo;
                plantaExistente.ambiente = model.ambiente;
                plantaExistente.atraiAbelha = model.atraiAbelha;
                plantaExistente.petFriendly = model.petFriendly; // Corrigido para usar petFriendly
                plantaExistente.imagem = model.imagem;

                await _service.updateDocAsync("Plantas", id, plantaExistente);

                return Ok(new { message = "Planta atualizada com sucesso!", planta = plantaExistente });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar planta: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }



        [HttpDelete("Plantas/{id}")]
        public async Task<IActionResult> DeletaPlantaAsync([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "ID inválido!" });
                }

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
                Console.WriteLine($"Erro ao deletar planta: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }



    }
}
