using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoAgenciaApi.Utilitarios;
using proyectoAgenciaApi.Entities;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace proyectoAgenciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciaViajesController : ControllerBase
    {
        private readonly IConfiguration _configuration;


        public AgenciaViajesController(IConfiguration configuration)
        {
            _configuration = configuration;
        
        }


        [HttpPost]
        [Route("RegistrarAgenciaViajes")]

        public IActionResult RegistrarAgenciaViajes(AgenciaViajesEnt entidad)
        {
            var respuesta = new AgenciaViajesEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("RegistrarAgenciaViajes",
                        new { entidad.NombreAgencia, entidad.Direccion, entidad.Ciudad },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se registró la información de su nueva agencia de viajes";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su Agencia fue registrada correctamente";
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
        [Route("ConsultarAgenciaViajes")]
        public IActionResult ConsultarAgenciaViajes()
            
        {
            var resultado = new List<AgenciaViajesEnt>();
            var respuesta = new AgenciaViajesEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<AgenciaViajesEnt>("Consultar_AgenciaViajes",
                        new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de las Agencias";
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
        public IActionResult CambiarEstado(AgenciaViajesEnt entidad)
        {
            var respuesta = new AgenciaViajesEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("ACTUALIZAR_ESTADO",
                        new { entidad.IdAgencia },
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


        [HttpGet]
        [Route("ConsultarAgenciaViaje")]
        public IActionResult ConsultarAgenciaViaje(long q)
        {
            var resultado = new AgenciaViajesEnt();
            var respuesta = new AgenciaViajesEntRespuesta();
            long IdAgencia = q;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<AgenciaViajesEnt>("Consultar_AgenciaViaje",
                        new { IdAgencia },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de su agencia";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
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



    }
}
