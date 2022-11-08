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

        public void EliminarPedidos()
        {
            ListadoPedidos.Clear();
        }

        public void EliminarPedido(int num)
        {
            int index = 0;

            for(index = 0; index < ListadoPedidos.Count; index++)
            {
                if (ListadoPedidos[index].Nro == num)
                {
                    break;
                }
            }

            ListadoPedidos.RemoveAt(index);
        }
    }
}
