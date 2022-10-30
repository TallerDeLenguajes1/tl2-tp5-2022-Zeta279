namespace TP5.Models
{
    public class PedidoModel
    {
        public int Nro { get; }
        public string Detalles { get; }
        public ClienteModel cliente { get; }
        public CadeteModel Cadete { get; private set; }
        private bool EnCurso;
        private bool Entregado;

        public PedidoModel(int nro, string det, int id, string nom, string direc, long tel, string datos)
        {
            Nro = nro;
            Detalles = det;
            cliente = new ClienteModel(id, nom, direc, tel, datos);
            EnCurso = false;
            Entregado = false;
        }

        public void IniciarPedido(CadeteModel cadete)
        {
            EnCurso = true;
            Cadete = cadete;
        }

        public void BorrarCadete()
        {
            Cadete = null;
        }

        public void EntregarPedido()
        {
            EnCurso = false;
            Entregado = true;
        }

        public void AsignarCadete(CadeteModel cadete)
        {
            Cadete = cadete;
        }

        public bool FueEntregado()
        {
            return Entregado;
        }

        public bool EstaEnCurso()
        {
            return EnCurso;
        }

        public override string ToString()
        {
            string curso;
            if (EnCurso) curso = "Si";
            else curso = "No";
            return $"Número: {Nro}\nDetalles: {Detalles}\nEn curso: {curso}\nCliente: \n{cliente}";
        }

        public string PasarACSV()
        {
            int auxA = 0, auxB = 0, cadID;

            if (EnCurso) auxA = 1;

            if (Entregado) auxB = 1;

            if (!EnCurso && !Entregado) cadID = 0;
            else if(Entregado && Cadete is not null)
            {
                cadID = -1;
            }

            else cadID = Cadete.id;

            return $"{Nro};{Detalles};{cliente.id};{cliente.nombre};{cliente.direccion};{cliente.telefono};{cliente.DatosRef};{cadID};{auxA};{auxB}\n";
        }
    }
}
