namespace proyectoAgenciaApi.Entities
{
    public class ContactosEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public ContactosEnt Objeto { get; set; } = null;
        public List<ContactosEnt> Objetos { get; set; } = new List<ContactosEnt>();
        public bool ResultadoTransaccion { get; set; }
    }
    public class ContactosEnt
    {
        public long IdContacto { get; set; }
        public long IdUsuario { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}
