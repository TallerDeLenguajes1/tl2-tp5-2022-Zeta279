namespace Cadeteria.Models
{
    public class CadeteModel: PersonaModel
    {
        public CadeteModel(int id = 0, string nom = "", string direc = "", string tel = "") : base(id, nom, direc, tel)
        {

        }

        public static bool operator == (CadeteModel cad1, CadeteModel cad2)
        {
            return cad1.ID == cad2.ID;
        }

        public static bool operator != (CadeteModel cad1, CadeteModel cad2)
        {
            return cad1.ID != cad2.ID;
        }
    }
}
