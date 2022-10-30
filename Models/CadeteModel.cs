namespace TP5.Models
{
    public class CadeteModel: PersonaModel
    {
        public CadeteModel(int id, string nom, string direc, long tel): base(id, nom, direc, tel)
        {

        }

        static public List<CadeteModel> ObtenerCadetes(){
            string[] array;
            List<CadeteModel> cadetes = new List<CadeteModel>();

            foreach (string s in System.IO.File.ReadAllLines("CSV/cadetes.csv"))
            {
                if (s != "")
                {
                    try{
                        array = s.Split(";");
                        cadetes.Add(new CadeteModel(Int32.Parse(array[0]), array[1], array[2], long.Parse(array[3])));
                    }
                    catch(Exception ex){
                        Console.WriteLine("Ha ocurrido un error: " + ex.Message);
                    }
                    
                }
            }

            return cadetes;
        }

        static public void IngresarCadetes(List<CadeteModel> ListaCadetes)
        {
            string csv = "";

            foreach(var cadete in ListaCadetes){
                csv += $"{cadete.id};{cadete.nombre};{cadete.direccion};{cadete.telefono}\n";
            }

            System.IO.File.WriteAllText("CSV/cadetes.csv", csv);
        }
    }
}
