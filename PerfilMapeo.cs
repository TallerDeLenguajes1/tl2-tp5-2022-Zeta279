using AutoMapper;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Cadeteria.Repo;

namespace Cadeteria
{
    public class PerfilMapeo: Profile
    {
        public PerfilMapeo()
        {
            // Repositorios
            CadeteRepository cadeteRepo = new();
            ClienteRepository clienteRepo = new();

            // Cadetes
            CreateMap<CadeteViewModel, CadeteModel>().ReverseMap();
            CreateMap<EditarCadeteViewModel, CadeteModel>().ReverseMap();
            CreateMap<CrearCadeteViewModel, CadeteModel>();

            // Pedidos
            CreateMap<PedidoModel, PedidoViewModel>().ReverseMap();
            CreateMap<CrearPedidoViewModel, PedidoModel>()
                .ForPath(dest => dest.Cliente.ID, opt => opt.MapFrom(opt => opt.ClienteID))
                .ForPath(dest => dest.Cliente.Nombre, opt => opt.MapFrom(opt => opt.ClienteNombre))
                .ForPath(dest => dest.Cliente.Direccion, opt => opt.MapFrom(opt => opt.ClienteDireccion))
                .ForPath(dest => dest.Cliente.Telefono, opt => opt.MapFrom(opt => opt.ClienteTelefono));


            // Clientes
            CreateMap<ClienteModel, ClienteViewModel>().ReverseMap();
            CreateMap<EditarClienteViewModel, ClienteModel>().ReverseMap();
            CreateMap<CrearClienteViewModel, ClienteModel>();
        }
    }
}
