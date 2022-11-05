namespace Cadeteria.Models
{
    public class ClienteModel: PersonaModel
    {
        public string DatosRef {get;}

        public ClienteModel(int id, string nom, string direc, long tel, string datos): base(id, nom, direc, tel)
        {
            DatosRef = datos;
        }
    }
}
