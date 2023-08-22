namespace proyectoAgenciaApi.Entities
{
    public class PaqueteEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public PaqueteEnt Objeto { get; set; } = null;
        public List<PaqueteEnt> Objetos { get; set; } = new List<PaqueteEnt>();
        public bool ResultadoTransaccion { get; set; }
    }

    public class PaqueteEnt
        {
        public long IdPaquete { get; set; }
        public decimal Precio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Agente { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime Finalizacion { get; set; }
        public int Campo { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public string Video { get; set; } = string.Empty;
    }
    }

