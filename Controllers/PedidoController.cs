using Microsoft.AspNetCore.Mvc;
using ProjetoPlanta_Backend.Models;
using ProjetoPlanta_Backend.Data;
using System;
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

        [HttpPost]
        public async Task<IActionResult> CriarPedidoAsync([FromBody] Pedido pedido)
        {
            try
            {
                var planta = await _service.getDocAsync<Planta>("Plantas", pedido.plantaId);
                if (planta == null)
                {
                    return NotFound(new { message = "Planta não encontrada." });
                }

                if (planta.estoque < pedido.quantidade)
                {
                    return BadRequest(new { message = "Estoque insuficiente para a quantidade solicitada." });
                }

                pedido.status = "pendente";
                pedido.data = DateTime.UtcNow;

                string pedidoId = await _service.AddDocAsync("Pedidos", pedido);
                Console.WriteLine(pedidoId);
                return Ok(new { message = "Pedido criado com sucesso!", id = pedidoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor.", detalhes = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterPedidosAsync()
        {
            try
            {
                var pedidos = await _service.getAllDocsAsync<Pedido>("Pedidos");

                if (pedidos == null || pedidos.Count == 0)
                {
                    return NotFound(new { message = "Nenhum pedido encontrado." });
                }

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor.", detalhes = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarStatusPedidoAsync(string id, [FromBody] StatusPedido request)
        {
            try
            {
                string novoStatus = request.Status;  // Acessando o Status corretamente

                // Buscar o pedido pelo ID
                var pedido = await _service.getDocAsync<Pedido>("Pedidos", id);
                if (pedido == null)
                {
                    return NotFound(new { message = "Pedido não encontrado." });
                }

                // Atualizar status do pedido
                pedido.status = novoStatus;
                await _service.updateDocAsync("Pedidos", id, pedido);

                // Se o pedido for aprovado, remover do estoque da planta
                if (novoStatus.ToLower() == "aprovado")
                {
                    var planta = await _service.getDocAsync<Planta>("Plantas", pedido.plantaId);
                    if (planta != null && planta.estoque >= pedido.quantidade)
                    {
                        planta.estoque -= pedido.quantidade;
                        await _service.updateDocAsync("Plantas", pedido.plantaId, planta);
                    }
                    else
                    {
                        return BadRequest(new { message = "Estoque insuficiente para aprovar o pedido." });
                    }
                }

                return Ok(new { message = "Status do pedido atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor.", detalhes = ex.Message });
            }
        }
    }
}