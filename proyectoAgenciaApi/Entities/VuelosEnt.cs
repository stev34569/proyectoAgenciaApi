namespace proyectoAgenciaApi.Entities
{
    public class VuelosEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public VuelosEnt? Vuelo { get; set; } = null;
        public List<VuelosEnt>? Vuelos { get; set; } = new List<VuelosEnt>();
        public bool? ResultadoTransaccion { get; set; }
    }

 //   IdVuelo INT PRIMARY KEY IDENTITY(1,1),
 //   AeropuertoInicio INT NOT NULL,
 //   AeropuertoDestino INT NOT NULL,
 //   FechaInicio DATE NOT NULl,
 //   FechaDestino DATE NOT NULL,
 //   Precio MONEY NOT NULL,
 //   Descripcion VARCHAR(100) NOT NULL,

    public class VuelosEnt
    {
        public int IdVuelo { get; set; }
        public int AeropuertoInicio { get; set; } 
        public int AeropuertoDestino { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaDestino { get; set; } 
        public float Precio { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;

    }

    public class ActualizarVueloEnt
    {
        public int IdVuelo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaDestino { get; set; }
        public float Precio { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;
    }

}
