namespace Cadeteria.Models
{
    public class ClienteModel: PersonaModel
    {
        public ClienteModel(int id = 0, string nom = "" , string direc = "", string tel = ""): base(id, nom, direc, tel)
        {

        }
    }
}
