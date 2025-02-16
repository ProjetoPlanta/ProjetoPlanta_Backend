using System.Runtime.CompilerServices;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using ProjetoPlanta_Backend.Models;
using ProjetoPlanta_Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CadastroPlantaAsync([FromBody]  PlantaCadastroViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.nomePopular))
                {
                    return BadRequest(new { error = "Nome da planta inválido!" });
                }

                // Criar um ID seguro baseado no nome da planta
                string id = FirestoreService.CriarID(model.nomePopular);
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "ID inválido gerado!" });
                }
                Console.WriteLine($" ID gerado: {id}");


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

                try
                {
                    await _service.AddDocAsync("Plantas", id, novaPlanta);
                    Console.WriteLine(" Documento salvo no Firestore!");
                    return Ok(new { message = "Planta cadastrada com sucesso!", id = id });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Erro ao salvar no Firestore: {ex.Message}");
                    return StatusCode(500, new { error = "Erro ao salvar no Firestore", detalhes = ex.Message });
                }

                //Console.WriteLine(" Criando planta no Firestore...");
                //await _service.AddDocAsync("Plantas", id, novaPlanta);
                //Console.WriteLine(" Planta criada com sucesso!");
                //return Ok(new { message = "Planta cadastrada com sucesso!", id = id });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }
    }
}
