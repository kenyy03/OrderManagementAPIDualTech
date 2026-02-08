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
    public class ClienteService
    {
        private readonly IUnitOfWork _uowOrderManagement;
        private readonly IMapper _mapper;
        private readonly IRepository<Cliente> _clienteRepo;
        public ClienteService(IUnitOfWorkFactory uofFactory, IMapper mapper)
        {
            _uowOrderManagement = uofFactory.CreateUnitOfWork(UnitOfWorkType.OrderManagementDB);
            _mapper = mapper;
            _clienteRepo = _uowOrderManagement.Repository<Cliente>();
        }

        public async Task<ApiResponse<List<ClienteDto>>> ObtenerClientes()
        {
            try
            {
                var clientes = await _uowOrderManagement.Repository<Cliente>().AsQueryable()
                   .AsNoTracking()
                   .ProjectTo<ClienteDto>(_mapper.ConfigurationProvider)
                   .ToListAsync();

                return ApiResponse<List<ClienteDto>>.Success(clientes);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClienteDto>>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

        public async Task<ApiResponse<ClienteDto>> ObtenerClientePorId(long clienteId)
        {
            try
            {
                if (clienteId <= 0)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlObtener, errors: [ErrorMessages.Cliente.IdMayorCero]);
                }

                var cliente = await _clienteRepo.AsQueryable()
                    .AsNoTracking()
                    .Where(x => x.ClienteId == clienteId)
                    .ProjectTo<ClienteDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (cliente is null)
                {
                    return ApiResponse<ClienteDto>.NotFound(message: ErrorMessages.Cliente.NoEncontrado, errors: [string.Format(ErrorMessages.Cliente.ClienteNoEncontradoConId, clienteId)]);
                }

                return ApiResponse<ClienteDto>.Success(cliente);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

        public async Task<ApiResponse<ClienteDto>> CrearNuevoCliente(CreateClienteDto request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.RequestCannotBeNull, errors: [ErrorMessages.EnsureBodyIsSent]);

                if (request.ClienteId < 0 || request.ClienteId > 0)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlCrear, errors: [ErrorMessages.Cliente.IdNoEnviar]);
                }

                if (string.IsNullOrWhiteSpace(request.Nombre) || request.Nombre.Length < ConstantsValues.Cliente.MinLengthNombre || request.Nombre.Length > ConstantsValues.Cliente.MaxLengthNombre)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlCrear, errors: [ErrorMessages.Cliente.NombreLongitud]);
                }

                if (string.IsNullOrWhiteSpace(request.Identidad) || request.Identidad.Length < ConstantsValues.Cliente.LengthIdentidad || request.Identidad.Length > ConstantsValues.Cliente.LengthIdentidad)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlCrear, errors: [ErrorMessages.Cliente.IdentidadLongitud]);
                }

                string? identidadDuplicada = await _clienteRepo.AsQueryable()
                    .AsNoTracking()
                    .Where(x => x.Identidad == request.Identidad)
                    .Select(s => s.Identidad)
                    .FirstOrDefaultAsync();

                if (identidadDuplicada is not null)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlCrear, errors: [string.Format(ErrorMessages.Cliente.IdentidadDuplicada, identidadDuplicada)]);
                }

                var nuevoCliente = _mapper.Map<Cliente>(request);
                await _clienteRepo.AddAsync(nuevoCliente);
                await _uowOrderManagement.SaveAsync();
                return ApiResponse<ClienteDto>.Success(_mapper.Map<ClienteDto>(nuevoCliente), message: SuccessMessages.Cliente.CreadoExitosamente);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }

        public async Task<ApiResponse<ClienteDto>> ActualizarCliente(long clienteId, UpdateClienteDto request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.RequestCannotBeNull, errors: [ErrorMessages.EnsureBodyIsSent]);

                if (clienteId <= 0)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlActualizar, errors: [ErrorMessages.Cliente.IdMayorCero]);
                }

                if (string.IsNullOrWhiteSpace(request.Nombre) || request.Nombre.Length < ConstantsValues.Cliente.MinLengthNombre || request.Nombre.Length > ConstantsValues.Cliente.MaxLengthNombre)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlActualizar, errors: [ErrorMessages.Cliente.NombreLongitud]);
                }

                if (string.IsNullOrWhiteSpace(request.Identidad) || request.Identidad.Length < ConstantsValues.Cliente.LengthIdentidad || request.Identidad.Length > ConstantsValues.Cliente.LengthIdentidad)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlActualizar, errors: [ErrorMessages.Cliente.IdentidadLongitud]);
                }

                var clienteExistente = await _clienteRepo.AsQueryable()
                    .Where(x => x.ClienteId == clienteId)
                    .FirstOrDefaultAsync();

                if (clienteExistente is null)
                {
                    return ApiResponse<ClienteDto>.NotFound(message: ErrorMessages.Cliente.NoEncontrado, errors: [string.Format(ErrorMessages.Cliente.ClienteNoEncontradoConId, clienteId)]);
                }

                string? identidadDuplicada = await _clienteRepo.AsQueryable()
                    .AsNoTracking()
                    .Where(x => x.Identidad == request.Identidad && x.ClienteId != clienteId)
                    .Select(s => s.Identidad)
                    .FirstOrDefaultAsync();

                if (identidadDuplicada is not null)
                {
                    return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.Cliente.ErrorAlActualizar, errors: [string.Format(ErrorMessages.Cliente.IdentidadDuplicada, identidadDuplicada)]);
                }

                _mapper.Map(request, clienteExistente);
                _clienteRepo.Update(clienteExistente);
                await _uowOrderManagement.SaveAsync();
                
                return ApiResponse<ClienteDto>.Success(_mapper.Map<ClienteDto>(clienteExistente), message: SuccessMessages.Cliente.ActualizadoExitosamente);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClienteDto>.Failure(message: ErrorMessages.InternalServerError, errors: [ErrorMessages.FormatException(ex)]);
            }
        }
    }
}
