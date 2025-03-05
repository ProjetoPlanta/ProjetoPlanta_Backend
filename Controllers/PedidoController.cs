using Microsoft.AspNetCore.Mvc;
using ProjetoPlanta_Backend.Models;
using ProjetoPlanta_Backend.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoPlanta_Backend.Controllers
{
    [ApiController]
    [Route("Pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly FirestoreService _service;

        public PedidoController()
        {
            _service = new FirestoreService();
        }

        // Criar Pedido (Atualizado para múltiplas plantas)
        [HttpPost]
        public async Task<IActionResult> CriarPedidoAsync([FromBody] Pedido pedido)
        {
            try
            {
                if (pedido.plantas == null || pedido.plantas.Count == 0)
                {
                    return BadRequest(new { message = "O pedido deve conter pelo menos uma planta." });
                }

                List<Planta> plantasNoPedido = new List<Planta>();

                foreach (var item in pedido.plantas)
                {
                    var planta = await _service.getDocAsync<Planta>("Plantas", item.plantaId);
                    if (planta == null)
                    {
                        return NotFound(new { message = $"Planta com ID {item.plantaId} não encontrada." });
                    }

                    if (planta.estoque < item.quantidade)
                    {
                        return BadRequest(new { message = $"Estoque insuficiente para a planta {planta.nomePopular}." });
                    }

                    // Adiciona a planta validada à lista de retorno
                    plantasNoPedido.Add(planta);
                }

                pedido.status = "pendente";
                pedido.data = DateTime.UtcNow;

                string pedidoId = await _service.AddDocAsync("Pedidos", pedido);

                return Ok(new
                {
                    message = "Pedido criado com sucesso!",
                    id = pedidoId,
                    plantas = plantasNoPedido
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor.", detalhes = ex.Message });
            }
        }

        // Obter todos os pedidos
        [HttpGet]
        public async Task<IActionResult> ObterPedidosAsync()
        {
            try
            {
                var pedidos = await _service.getAllDocsAsync<Pedido>("Pedidos");

                foreach (var pedido in pedidos)
                {
                    if (pedido.plantas != null && pedido.plantas.Count > 0)
                    {
                        List<Planta> detalhesDasPlantas = new List<Planta>();

                        foreach (var item in pedido.plantas)
                        {
                            var planta = await _service.getDocAsync<Planta>("Plantas", item.plantaId);
                            if (planta != null)
                            {
                                detalhesDasPlantas.Add(planta);
                            }
                        }

                        // Substitui a lista de PlantaPedido pela lista de plantas completas
                        pedido.plantasDetalhadas = detalhesDasPlantas;
                    }
                }

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar pedidos.", detalhes = ex.Message });
            }
        }



        // Obter um pedido específico por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorIdAsync(string id)
        {
            try
            {
                var pedido = await _service.getDocAsync<Pedido>("Pedidos", id);
                if (pedido == null)
                {
                    return NotFound(new { message = "Pedido não encontrado." });
                }
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar pedido.", detalhes = ex.Message });
            }
        }

        // Atualizar status do pedido (aprovado, cancelado, etc.)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatusPedidoAsync(string id, [FromBody] string novoStatus)
        {
            try
            {
                var pedido = await _service.getDocAsync<Pedido>("Pedidos", id);
                if (pedido == null)
                {
                    return NotFound(new { message = "Pedido não encontrado." });
                }

                pedido.status = novoStatus;
                await _service.updateDocAsync("Pedidos", id, pedido);

                if (novoStatus.ToLower() == "aprovado")
                {
                    foreach (var item in pedido.plantas)
                    {
                        var planta = await _service.getDocAsync<Planta>("Plantas", item.plantaId);
                        if (planta != null && planta.estoque >= item.quantidade)
                        {
                            planta.estoque -= item.quantidade;
                            await _service.updateDocAsync("Plantas", item.plantaId, planta);

                            // Registrar movimentação de estoque
                            var movimentacao = new Movimentacao
                            {
                                plantaId = item.plantaId,
                                quantidade = item.quantidade,
                                tipoTransacao = "venda online",
                                data = DateTime.UtcNow
                            };
                            await _service.AddDocAsync("Movimentacoes", movimentacao);
                        }
                    }
                }

                return Ok(new { message = "Status do pedido atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar status do pedido.", detalhes = ex.Message });
            }
        }

        // Deletar um pedido
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedidoAsync(string id)
        {
            try
            {
                var pedido = await _service.getDocAsync<Pedido>("Pedidos", id);
                if (pedido == null)
                {
                    return NotFound(new { message = "Pedido não encontrado." });
                }

                await _service.deleteDocAsync("Pedidos", id);
                return Ok(new { message = "Pedido deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao deletar pedido.", detalhes = ex.Message });
            }
        }
    }
}
