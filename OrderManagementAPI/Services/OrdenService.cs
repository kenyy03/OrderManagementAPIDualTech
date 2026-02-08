using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Common.Constants;
using OrderManagementAPI.Data.Uow.Enums;
using OrderManagementAPI.Data.Uow.Interfaces;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Services
{
    public class OrdenService
    {
        private readonly IUnitOfWork _uowOrderManagement;
        private readonly IMapper _mapper;
        private readonly IRepository<Orden> _ordenRepo;
        private readonly IRepository<Producto> _productoRepo;
        private readonly IRepository<Cliente> _clienteRepo;

        public OrdenService(IUnitOfWorkFactory uofFactory, IMapper mapper)
        {
            _uowOrderManagement = uofFactory.CreateUnitOfWork(UnitOfWorkType.OrderManagementDB);
            _mapper = mapper;
            _ordenRepo = _uowOrderManagement.Repository<Orden>();
            _productoRepo = _uowOrderManagement.Repository<Producto>();
            _clienteRepo = _uowOrderManagement.Repository<Cliente>();
        }

        public async Task<ApiResponse<OrdenDto>> CrearOrden(CreateOrdenDto request)
        {
            try
            {
                var clienteExiste = await _clienteRepo.AsQueryable()
                    .AnyAsync(c => c.ClienteId == request.ClienteId);

                if (!clienteExiste)
                {
                    return ApiResponse<OrdenDto>.NotFound(message: ErrorMessages.Cliente.NoEncontrado, 
                        errors: [string.Format(ErrorMessages.Cliente.ClienteNoEncontradoConId, request.ClienteId)]);
                }

                var nuevaOrden = new Orden
                {
                    ClienteId = request.ClienteId,
                    FechaCreacion = DateTime.Now,
                    DetallesOrden = new List<DetalleOrden>()
                };

                decimal subtotalOrden = 0;
                decimal impuestoOrden = 0;
                decimal totalOrden = 0;
                string[] errors = [];
                var productosIds= request.Detalles.Select(d => d.ProductoId).ToList();

                var productos = await _productoRepo.AsQueryable()
                    .Where(x => productosIds.Contains(x.ProductoId))
                    .ToListAsync();

                foreach (var producto in productos)
                {

                    var productoOrdenDetalle = request.Detalles.Find(d => d.ProductoId == producto.ProductoId);
                    if (productoOrdenDetalle is null)
                    {
                        errors = [.. errors, string.Format(ErrorMessages.Producto.ProductoNoEncontradoConId, producto.ProductoId)];
                        continue;
                    }
                    var calculoExistencia = producto.Existencia - productoOrdenDetalle.Cantidad;
                    if (calculoExistencia < 0)
                    {
                        errors = [.. errors, string.Format(ErrorMessages.Producto.ProductoSinExistencia, producto.Nombre, producto.Existencia, productoOrdenDetalle.Cantidad)];
                        continue;
                    }
                    decimal subtotalDetalle = producto.Precio * productoOrdenDetalle.Cantidad;
                    decimal impuestoDetalle = subtotalDetalle * 0.15m;
                    decimal totalDetalle = subtotalDetalle + impuestoDetalle;

                    var detalleOrden = new DetalleOrden
                    {
                        ProductoId = productoOrdenDetalle.ProductoId,
                        Cantidad = productoOrdenDetalle.Cantidad,
                        Subtotal = subtotalDetalle,
                        Impuesto = impuestoDetalle,
                        Total = totalDetalle
                    };

                    nuevaOrden.DetallesOrden.Add(detalleOrden);

                    subtotalOrden += subtotalDetalle;
                    impuestoOrden += impuestoDetalle;
                    totalOrden += totalDetalle;
                    producto.Existencia = calculoExistencia;
                }

                if (errors.Length > 0)
                {
                    return ApiResponse<OrdenDto>.Failure(message: ErrorMessages.Orden.ErrorAlCrear, errors: errors);
                }

                    nuevaOrden.Subtotal = subtotalOrden;
                nuevaOrden.Impuesto = impuestoOrden;
                nuevaOrden.Total = totalOrden;
                await _uowOrderManagement.BeginTransactionAsync();
                await _ordenRepo.AddAsync(nuevaOrden);
                await _uowOrderManagement.SaveAsync();
                await _uowOrderManagement.CommitAsync();

                var ordenDto = _mapper.Map<OrdenDto>(nuevaOrden);

                return ApiResponse<OrdenDto>.Success(ordenDto, message: SuccessMessages.Orden.CreadaExitosamente);
            }
            catch (Exception ex)
            {
                await _uowOrderManagement.RollBackAsync();
                return ApiResponse<OrdenDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }
    }
}
