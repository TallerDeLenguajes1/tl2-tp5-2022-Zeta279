namespace Cadeteria.Models
{
    public enum estado
    {
        SinAsignar,
        Pendiente,
        EnCurso,
        Entregado,
        EntregadoBorrado
    }

    public class PedidoModel
    {
        public int Nro { get; }
        public string Detalles { get; }
        public ClienteModel cliente { get; }
        public estado Estado { get; private set; }
        private bool CadeteBorrado;

        public PedidoModel(int nro, string det, int id, string nom, string direc, string tel)
        {
            Nro = nro;
            Detalles = det;
            cliente = new ClienteModel(id, nom, direc, tel);
            Estado = estado.SinAsignar;
            CadeteBorrado = false;
        }

        public void AsignarCadete()
        {
            Estado = estado.Pendiente;
        }

        public void IniciarPedido()
        {
            Estado = estado.EnCurso;
        }

        public void EntregarPedido()
        {
            Estado = estado.Entregado;
        }

        public bool FueEntregado()
        {
            return Estado == estado.Entregado;
        }

        public bool EstaEnCurso()
        {
            return Estado == estado.EnCurso;
        }

        public void BorrarCadete()
        {
            if (FueEntregado()) Estado = estado.EntregadoBorrado;
        }

        public override string ToString()
        {
            string curso;
            if (Estado == estado.EnCurso) curso = "Si";
            else curso = "No";
            return $"Número: {Nro}\nDetalles: {Detalles}\nEn curso: {curso}\nCliente: \n{cliente}";
        }
    }
}
