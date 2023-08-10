namespace proyectoAgenciaApi.Entities
{
    public class CarritoEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public CarritoEnt Objeto { get; set; } = null;
        public List<CarritoEnt> Objetos { get; set; } = new List<CarritoEnt>();
        public bool ResultadoTransaccion { get; set; }
    }

    public class CarritoEnt
    {
        public long IdCarrito { get; set; }
        public long IdPaquete  { get; set; }
        public long IdUsuario { get; set; }
        public DateTime FechaCarrito { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}
