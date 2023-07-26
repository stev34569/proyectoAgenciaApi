namespace proyectoAgenciaApi.Entities
{
    public class AgenciaViajesEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public AgenciaViajesEnt Objeto { get; set; } = null;
        public List<AgenciaViajesEnt> Objetos { get; set; } = new List<AgenciaViajesEnt>();
        public bool ResultadoTransaccion { get; set; }
    }

    public class AgenciaViajesEnt
    {
        public long IdAgencia { get; set; }
        public string NombreAgencia { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;

        public bool Estado { get; set; }



    }

}
