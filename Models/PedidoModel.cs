namespace Cadeteria.Models
{
    public enum estado
    {
        SinAsignar,
        Pendiente,
        EnCurso,
        Entregado
    }

    public class PedidoModel
    {
        public int Nro { get; }
        public string Detalles { get; }
        public ClienteModel cliente { get; }
        public estado Estado { get; private set; }

        public PedidoModel(int nro, string det, int id, string nom, string direc, string tel)
        {
            Nro = nro;
            Detalles = det;
            cliente = new ClienteModel(id, nom, direc, tel);
            Estado = estado.SinAsignar;
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

        public override string ToString()
        {
            string curso;
            if (Estado == estado.EnCurso) curso = "Si";
            else curso = "No";
            return $"Número: {Nro}\nDetalles: {Detalles}\nEn curso: {curso}\nCliente: \n{cliente}";
        }

        public static bool operator ==(PedidoModel ped1, PedidoModel ped2)
        {
            return ped1.Nro == ped2.Nro;
        }

        public static bool operator !=(PedidoModel ped1, PedidoModel ped2)
        {
            return ped1.Nro != ped2.Nro;
        }
    }
}
