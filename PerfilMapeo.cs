using AutoMapper;
using Cadeteria.Models;
using Cadeteria.ViewModels;

namespace Cadeteria
{
    public class PerfilMapeo: Profile
    {
        public PerfilMapeo()
        {
            // Cadetes
            CreateMap<CadeteViewModel, CadeteModel>();
            CreateMap<EditarCadeteViewModel, CadeteModel>();
            CreateMap<CadeteModel, CadeteViewModel>();

            // Pedidos
            CreateMap<PedidoModel, PedidoViewModel>();
            CreateMap<CrearPedidoViewModel, PedidoModel>();
            CreateMap<PedidoViewModel, PedidoModel>();

            // Clientes
            CreateMap<ClienteModel, ClienteViewModel>();
            CreateMap<ClienteViewModel, ClienteModel>();
            CreateMap<CrearClienteViewModel, ClienteModel>();
        }
    }
}
