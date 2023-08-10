using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace proyectoAgenciaApi.Entities
{
    public class UsuarioEntRespuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public UsuarioEnt Objeto { get; set; } = null;
        public List<UsuarioEnt> Objetos { get; set; } = new List<UsuarioEnt>();
        public bool ResultadoTransaccion { get; set; }
    }

    public class UsuarioEnt
    {
        public long IdUsuario { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public string ContrasennaTemporal { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public bool ContrasennaTemp { get; set; }
        public DateTime ContrasennaVenc { get; set; }
        public string Token { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }

}

