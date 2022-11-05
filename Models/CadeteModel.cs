namespace Cadeteria.Models
{
    public class CadeteModel: PersonaModel
    {
        List<PedidoModel> ListadoPedidos;

        public CadeteModel(int id, string nom, string direc, string tel) : base(id, nom, direc, tel)
        {
            ListadoPedidos = new();
        }

        public void IngresarPedido(PedidoModel pedido)
        {
            ListadoPedidos.Add(pedido);
        }
    }
}
