using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoAgenciaApi.Utilitarios;
using proyectoAgenciaApi.Entities;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace proyectoAgenciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
      

        public ContactosController(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        [HttpPost]
        [Route("RegistrarContactos")]
    
        public IActionResult RegistrarContactos(ContactosEnt entidad)
        {
            var respuesta = new ContactosEntRespuesta();
            entidad.IdUsuario = long.Parse(User.Identity.Name.ToString());

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("REGISTRAR_CONTACTO",
                        new {  entidad.IdUsuario, entidad.Mensaje},
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se registró el mensaje";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su mensaje fue enviado correctamente";
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


        [HttpGet]
        [Route("ConsultarContactos")]
        public IActionResult ConsultarContactos()
        {
            var resultado = new List<ContactosEnt>();
            var respuesta = new ContactosEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<ContactosEnt>("CONSULTAR_CONTACTOS",
                        new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los contactos";
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

        [HttpPut]
        [Route("CambiarEstado")]
        public IActionResult CambiarEstado(ContactosEnt entidad)
        {
            var respuesta = new ContactosEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("ACTUALIZAR_ESTADOcontactos",
                        new { entidad.IdContacto },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se actualizó la información del estado";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "El estado fue actualizado correctamente";
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
