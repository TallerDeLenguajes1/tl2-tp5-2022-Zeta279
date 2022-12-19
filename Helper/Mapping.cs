using Cadeteria.Models;
using Cadeteria.ViewModels;

namespace Cadeteria.Helper
{
    public interface IMapping
    {
        public CadeteModel Mapear(CadeteViewModel cadete);
        public CadeteModel Mapear(CrearCadeteViewModel cadete);
        public CadeteModel Mapear(EditarCadeteViewModel cadete);
        public CadeteViewModel Mapear(CadeteModel cadete);
        public PedidoModel Mapear(PedidoViewModel pedido);
        public PedidoModel Mapear(CrearPedidoViewModel pedido);
        public PedidoViewModel Mapear(PedidoModel pedido);
        public ClienteModel Mapear(ClienteViewModel cliente);
        public ClienteModel Mapear(CrearClienteViewModel cliente);
        public ClienteViewModel Mapear(ClienteModel cliente);
    }

    public class Mapping: IMapping
    {
        public CadeteModel Mapear(CadeteViewModel cadete)
        {
            return new CadeteModel(cadete.CadeteId, cadete.nombre, cadete.direccion, cadete.telefono);
        }

        public CadeteModel Mapear(CrearCadeteViewModel cadete)
        {
            return new CadeteModel(0, cadete.Nombre, cadete.Direccion, cadete.Tel.ToString());
        }

        public CadeteModel Mapear(EditarCadeteViewModel cadete)
        {
            return new CadeteModel(cadete.ID, cadete.Nombre, cadete.Direccion, cadete.Tel.ToString());
        }
        public CadeteViewModel Mapear(CadeteModel cadete)
        {
            return new CadeteViewModel(cadete.id, cadete.nombre, cadete.direccion, cadete.telefono);
        }

        public PedidoModel Mapear(PedidoViewModel pedido)
        {
            return new PedidoModel(pedido.NumPedido, pedido.Detalles, pedido.Cliente.ID, pedido.Cliente.Nombre, pedido.Cliente.Direccion, pedido.Cliente.Telefono, pedido.Estado);
        }

        public PedidoModel Mapear(CrearPedidoViewModel pedido)
        {
            return new PedidoModel(0, pedido.Detalles, pedido.IDCliente, pedido.NombreCliente, pedido.DireccionCliente, pedido.TelefonoCliente);
        }

        public PedidoViewModel Mapear(PedidoModel pedido)
        {
            return new PedidoViewModel(pedido.Nro, pedido.Detalles, this.Mapear(pedido.cliente), pedido.Estado);
        }

        public ClienteModel Mapear(ClienteViewModel cliente)
        {
            return new ClienteModel(cliente.ID, cliente.Nombre, cliente.Direccion, cliente.Telefono);
        }

        public ClienteModel Mapear(CrearClienteViewModel cliente)
        {
            return new ClienteModel(cliente.ID, cliente.Nombre, cliente.Direccion, cliente.Telefono);
        }

        public ClienteViewModel Mapear(ClienteModel cliente)
        {
            return new ClienteViewModel(cliente.id, cliente.nombre, cliente.direccion, cliente.telefono);
        }
    }
}
