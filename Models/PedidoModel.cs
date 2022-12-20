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
        public int Nro { get; set; }
        public string Detalles { get; set; }
        public ClienteModel Cliente { get; set; }
        public CadeteModel Cadete { get; set; }
        public estado Estado { get; set; }

        public PedidoModel()
        {

        }

        public PedidoModel(int nro, string det, ClienteModel cliente, CadeteModel cadete = null, estado est = estado.SinAsignar)
        {
            Nro = nro;
            Detalles = det;
            Cliente = cliente;
            Cadete = cadete;
            Estado = est;
        }

        public override string ToString()
        {
            string curso;
            if (Estado == estado.EnCurso) curso = "Si";
            else curso = "No";
            return $"Número: {Nro}\nDetalles: {Detalles}\nEn curso: {curso}\nCliente: \n{Cliente}";
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
