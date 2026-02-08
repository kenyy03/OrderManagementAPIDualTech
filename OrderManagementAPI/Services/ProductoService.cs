using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Common.Constants;
using OrderManagementAPI.Data.Uow.Enums;
using OrderManagementAPI.Data.Uow.Interfaces;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Services
{
    public class ProductoService
    {
        private readonly IUnitOfWork _uowOrderManagement;
        private readonly IMapper _mapper;
        private readonly IRepository<Producto> _productoRepo;

        public ProductoService(IUnitOfWorkFactory uofFactory, IMapper mapper)
        {
            _uowOrderManagement = uofFactory.CreateUnitOfWork(UnitOfWorkType.OrderManagementDB);
            _mapper = mapper;
            _productoRepo = _uowOrderManagement.Repository<Producto>();
        }

        public async Task<ApiResponse<List<ProductoDto>>> ObtenerProductos()
        {
            try
            {
                var productos = await _productoRepo.AsQueryable()
                    .AsNoTracking()
                    .ProjectTo<ProductoDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return ApiResponse<List<ProductoDto>>.Success(productos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductoDto>>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

        public async Task<ApiResponse<ProductoDto>> ObtenerProductoPorId(long productoId)
        {
            try
            {
                var producto = await _productoRepo.AsQueryable()
                    .AsNoTracking()
                    .Where(x => x.ProductoId == productoId)
                    .ProjectTo<ProductoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (producto is null)
                {
                    return ApiResponse<ProductoDto>.NotFound(message: ErrorMessages.Producto.NoEncontrado, errors: [string.Format(ErrorMessages.Producto.ProductoNoEncontradoConId, productoId)]);
                }

                return ApiResponse<ProductoDto>.Success(producto);
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

        public async Task<ApiResponse<ProductoDto>> CrearProducto(CreateProductoDto request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.RequestCannotBeNull, errors: [ErrorMessages.EnsureBodyIsSent]);

                if (request.ProductoId < 0 || request.ProductoId > 0)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlCrear, errors: [ErrorMessages.Producto.IdNoEnviar]);
                }

                if (string.IsNullOrWhiteSpace(request.Nombre) || request.Nombre.Length < ConstantsValues.Producto.MinLengthNombre || request.Nombre.Length > ConstantsValues.Producto.MaxLengthNombre)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlCrear, errors: [ErrorMessages.Producto.NombreLongitud]);
                }

                if (request.Precio < 1)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlCrear, errors: [ErrorMessages.Producto.PrecioMayorCero]);
                }

                if (request.Existencia < 0)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlCrear, errors: [ErrorMessages.Producto.ExistenciaMayorIgualCero]);
                }
                var nuevoProducto = _mapper.Map<Producto>(request);
                await _productoRepo.AddAsync(nuevoProducto);
                await _uowOrderManagement.SaveAsync();
                
                return ApiResponse<ProductoDto>.Success(_mapper.Map<ProductoDto>(nuevoProducto), message: SuccessMessages.Producto.CreadoExitosamente);
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

        public async Task<ApiResponse<ProductoDto>> ActualizarProducto(long productoId, UpdateProductoDto request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.RequestCannotBeNull, errors: [ErrorMessages.EnsureBodyIsSent]);

                if (productoId < 1)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlActualizar, errors: [ErrorMessages.Producto.IdMayorCero]);
                }

                if (string.IsNullOrWhiteSpace(request.Nombre) || request.Nombre.Length < ConstantsValues.Producto.MinLengthNombre || request.Nombre.Length > ConstantsValues.Producto.MaxLengthNombre)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlActualizar, errors: [ErrorMessages.Producto.NombreLongitud]);
                }

                if (request.Precio < 1)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlActualizar, errors: [ErrorMessages.Producto.PrecioMayorCero]);
                }

                if (request.Existencia < 0)
                {
                    return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.Producto.ErrorAlActualizar, errors: [ErrorMessages.Producto.ExistenciaMayorIgualCero]);
                }
                var productoExistente = await _productoRepo.AsQueryable()
                    .Where(x => x.ProductoId == productoId)
                    .FirstOrDefaultAsync();

                if (productoExistente is null)
                {
                    return ApiResponse<ProductoDto>.NotFound(message: ErrorMessages.Producto.NoEncontrado, errors: [string.Format(ErrorMessages.Producto.ProductoNoEncontradoConId, productoId)]);
                }

                _mapper.Map(request, productoExistente);
                _productoRepo.Update(productoExistente);
                await _uowOrderManagement.SaveAsync();

                return ApiResponse<ProductoDto>.Success(_mapper.Map<ProductoDto>(productoExistente), message: SuccessMessages.Producto.ActualizadoExitosamente);
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductoDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

    }
}
