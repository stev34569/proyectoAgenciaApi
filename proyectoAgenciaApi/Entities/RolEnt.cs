namespace proyectoAgenciaApi.Entities
{
    public class RolEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public RolEnt Objeto { get; set; } = null;
        public List<RolEnt> Objetos { get; set; } = new List<RolEnt>();
        public bool ResultadoTransaccion { get; set; }
    }
    public class RolEnt
    {
        public short IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}
