using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Controllers.Extensions;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Services;

namespace OrderManagementAPI.Controllers
{
    [Route("api/ordenes")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenService _ordenService;

        public OrdenesController(OrdenService ordenService) => _ordenService = ordenService;

        [HttpPost]
        public async Task<IActionResult> CreateOrden([FromBody] CreateOrdenDto request)
        {
            var response = await _ordenService.CrearOrden(request);
            return response.ToActionResult();
        }
    }
}
