using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Controllers.Extensions;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Services;

namespace OrderManagementAPI.Controllers
{
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductosController(ProductoService productoService) => _productoService = productoService;

        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var response = await _productoService.ObtenerProductos();
            return response.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoPorId([FromRoute] long id)
        {
            var response = await _productoService.ObtenerProductoPorId(id);
            return response.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] CreateProductoDto request)
        {
            var response = await _productoService.CrearProducto(request);
            return response.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto([FromRoute] long id, [FromBody] UpdateProductoDto request)
        {
            var response = await _productoService.ActualizarProducto(id, request);
            return response.ToActionResult();
        }

    }
}
