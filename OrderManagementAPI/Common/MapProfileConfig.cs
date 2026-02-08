using AutoMapper;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Common
{
    public class MapProfileConfig : Profile
    {
        public MapProfileConfig()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<CreateClienteDto, Cliente>().ReverseMap();
            CreateMap<UpdateClienteDto, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Ordenes, opt => opt.Ignore());

            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<CreateProductoDto, Producto>().ReverseMap();
            CreateMap<UpdateProductoDto, Producto>()
                .ForMember(dest => dest.ProductoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DetallesOrden, opt => opt.Ignore());

            CreateMap<Orden, OrdenDto>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.DetallesOrden));
            
            CreateMap<CreateOrdenDto, Orden>()
                .ForMember(dest => dest.OrdenId, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.DetallesOrden, opt => opt.Ignore());

            CreateMap<DetalleOrden, DetalleOrdenDto>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre));
            
            CreateMap<CreateDetalleOrdenDto, DetalleOrden>()
                .ForMember(dest => dest.DetalleOrdenId, opt => opt.Ignore())
                .ForMember(dest => dest.OrdenId, opt => opt.Ignore())
                .ForMember(dest => dest.Orden, opt => opt.Ignore())
                .ForMember(dest => dest.Producto, opt => opt.Ignore());
        }
    }
}
