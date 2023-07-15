using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoAgenciaApi.Utilitarios;
using static proyectoAgenciaApi.Entities.UsaurioEnt;
using System.Data.SqlClient;
using System.Text;

namespace proyectoAgenciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUtils _utils;


        //CTOR constructor esta forma queda inyectada la dependecia
        public UsuarioController(IConfiguration configuration, IUtils utils)
        {
            _configuration = configuration;
            _utils = utils;
        }
        //ENVIA INFORMACIÓN
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(UsuarioEnt entidad)
        {
            var resultado = new UsuarioEnt();
            var respuesta = new UsuarioEntRespuesta();
            try
            {
                //PROGRA DESPUES DE HABER ECHO EL PROCEDIMIENTO ALMACENADO LLAMA APPSETTINGS.JSON CON EL NOMBRE DE LA CONEXION
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {

                    resultado = connection.Query<UsuarioEnt>("VALIDAR_USUARIO",
                        new { entidad.CorreoElectronico, entidad.Contrasenna },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de su usuario";
                        return Ok(respuesta);
                    }

                    if (resultado.ContrasennaTemp && resultado.ContrasennaVenc < DateTime.Now)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Su contraseña temporal ha expirado";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su usuario fue validado correctamente";
                    respuesta.Objeto = resultado;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente.";
                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(UsuarioEnt entidad)
        {
            var respuesta = new UsuarioEntRespuesta();

            /*PROGRA DESPUES DE HABER ECHO EL PROCEDIMIENTO ALMACENADO*/
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("REGISTRAR_USUARIO",
                     new { entidad.CorreoElectronico, entidad.Contrasenna, entidad.Identificacion, entidad.Nombre },
                     commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se registró la información de su usuario";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su usuario fue registrado correctamente";
                    respuesta.ResultadoTransaccion = true;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente.";
                return Ok(respuesta);
            }
        }

        //ENVIA INFORMACIÓN
        [HttpPost]
        [Route("ConsultarUsuarios")]
        public IActionResult ConsultarUsuarios()
        {
            var resultado = new List<UsuarioEnt>();
            var respuesta = new UsuarioEntRespuesta();

            try
            {
                //PROGRA DESPUES DE HABER ECHO EL PROCEDIMIENTO ALMACENADO LLAMA APPSETTINGS.JSON CON EL NOMBRE DE LA CONEXION
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<UsuarioEnt>("CONSULTAR_USUARIOS",
                           new { },
                           commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los usuarios";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Información consultada correctamente";
                    respuesta.Objetos = resultado;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente.";
                return Ok(respuesta);
            }
        }
        [HttpGet]
        [Route("ValidarCorreo")]
        public IActionResult ValidarCorreo(string CorreoElectronico)
        {
            var resultado = new UsuarioEnt();
            var respuesta = new UsuarioEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<UsuarioEnt>("VALIDAR_CORREO",
                        new { CorreoElectronico },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de su usuario";
                        return Ok(respuesta);
                    }

                    //Enviar el correo
                    string ClaveTemp = _utils.CreatePassword(8);

                    StringBuilder mensaje = new StringBuilder("");
                    mensaje.Append("Estimado(a) " + resultado.Nombre);
                    mensaje.Append("<br>");
                    mensaje.Append("<br>");
                    mensaje.Append("Le informamos que se ha generado la siguiente contraseña temporal: <b>" + ClaveTemp + "</b>");
                    mensaje.Append("<br>");
                    mensaje.Append("Esta contraseña tiene una vigencia de <b>15 minutos</b>");
                    mensaje.Append("<br>");
                    mensaje.Append("<br>");
                    mensaje.Append("Muchas gracias");
                    mensaje.Append("<br>");
                    mensaje.Append("Favor no responder a este correo electrónico");

                    _utils.EnviarCorreo(resultado.CorreoElectronico, "Recuperar Contraseña", mensaje.ToString());

                    connection.Execute("ACTUALIZAR_CLAVE_TEMP",
                        new { resultado.IdUsuario, ClaveTemp },
                        commandType: System.Data.CommandType.StoredProcedure);

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Se ha enviado un correo electrónico";
                    respuesta.ResultadoTransaccion = true;
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente.";
                return Ok(respuesta);
            }
        }

    }
}



