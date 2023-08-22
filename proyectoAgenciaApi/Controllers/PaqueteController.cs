using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using proyectoAgenciaApi.Entities;
using System.Data.SqlClient;

namespace proyectoAgenciaApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaqueteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PaqueteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ConsultarPaquetes")]
        public IActionResult ConsultarPaquetes(bool MostraTodo)
        {
            var resultado = new List<PaqueteEnt>();
            var respuesta = new PaqueteEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<PaqueteEnt>("CONSULTAR_PAQUETES",
                        new { MostraTodo },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los Paquetes";
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
        [Route("ConsultarPaquete")]
        public IActionResult ConsultarPaquete(long IdPaquete)
        {
            var resultado = new PaqueteEnt();
            var respuesta = new PaqueteEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<PaqueteEnt>("CONSULTAR_PAQUETE",
                        new { IdPaquete },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los Paquetes";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Información consultada correctamente";
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

        [HttpGet]
        [Route("ConsultarPaquetesUsuario")]
        public IActionResult ConsultarPaquetesUsuario()
        {
            long IdUsuario = long.Parse(User.Identity.Name.ToString());
            var resultado = new List<PaqueteEnt>();
            var respuesta = new PaqueteEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<PaqueteEnt>("CONSULTAR_PAQUETES_USUARIO",
                        new { IdUsuario },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de los Paquetes";
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

        [HttpPost]
        [Route("RegistrarPaquete")]
        public IActionResult RegistrarPaquete(PaqueteEnt entidad)
        {
            var resultado = new PaqueteEnt();
            var respuesta = new PaqueteEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<PaqueteEnt>("REGISTRAR_PAQUETES",
                        new
                        {
                            entidad.Precio,
                            entidad.Nombre,
                            entidad.Descripcion,
                            entidad.Agente,
                            entidad.FechaInicio,
                            entidad.Finalizacion,
                            entidad.Video,
                            entidad.Campo,
                        },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se registró la información del Paquete";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "El Paquete fue registrado correctamente";
                    respuesta.ResultadoTransaccion = true;
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

        [HttpPut]
        [Route("ActualizarImagen")]
        public IActionResult ActualizarImagen(PaqueteEnt entidad)
        {
            var respuesta = new UsuarioEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("ACTUALIZAR_IMAGEN",
                        new { entidad.IdPaquete, entidad.Imagen },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se actualizó la información del Paquete";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "El Paquete fue actualizado correctamente";
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

        [HttpPut]
        [Route("ActualizarPaquete")]
        public IActionResult ActualizarPaquete(PaqueteEnt entidad)
        {
            var respuesta = new UsuarioEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("ACTUALIZAR_PAQUETE",
                        new
                        {
                            entidad.IdPaquete,
                            entidad.Nombre,
                            entidad.Descripcion,
                            entidad.Agente,
                            entidad.Precio,
                            entidad.FechaInicio,
                            entidad.Finalizacion,
                            entidad.Campo,
                            entidad.Video
                        },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se actualizó la información del Paquete";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "El Paquete fue actualizado correctamente";
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
