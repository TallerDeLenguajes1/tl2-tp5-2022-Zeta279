namespace Cadeteria.Models
{
    public class PersonaModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public PersonaModel(int iD, string nombre, string direccion, string telefono)
        {
            ID = iD;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }

        public override string ToString()
        {
            return $"ID: {ID}\nNombre: {Nombre}\nDireccion: {Direccion}\nTelefono: {Telefono}";
        }
    }
}
