using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Controllers.Extensions;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Services;

namespace OrderManagementAPI.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        public ClientesController(ClienteService clienteService) => _clienteService = clienteService;

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var response = await _clienteService.ObtenerClientes();
            return response.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientePorId([FromRoute]long id)
        {
            var response = await _clienteService.ObtenerClientePorId(id);
            return response.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClienteDto request)
        {
            var response = await _clienteService.CrearNuevoCliente(request);
            return response.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente([FromRoute] long id, [FromBody] UpdateClienteDto request)
        {
            var response = await _clienteService.ActualizarCliente(id, request);
            return response.ToActionResult();
        }
    }
}
