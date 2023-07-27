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
    public class VuelosController : ControllerBase
    {
        private readonly IConfiguration _configuration;


        public VuelosController(IConfiguration configuration)
        {
            _configuration = configuration;
        
        }


        [HttpGet]
        [Route("ConsultarVuelos")]
        public IActionResult ConsultarAgenciaViajes()

        {
            var resultado = new List<VuelosEnt>();
            var respuesta = new VuelosEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<VuelosEnt>("Consultar_Vuelos",
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
                    //foreach( var element in resultado) {
                    //    var fechaInicioA = element.FechaInicio.Split('/');
                    //    // Annio mes dia
                    //    // BD Mes Dia Annio
                    //    int diaInicio = Int32.Parse(fechaInicioA[1]);
                    //    int mesInicio = Int32.Parse(fechaInicioA[0]);
                    //    int annioInicio = Int32.Parse(fechaInicioA[2]);
                    //    var fechaInicioFinal = new DateOnly(annioInicio, mesInicio, diaInicio);
                    //}
                    respuesta.Vuelos = resultado;
                    return Ok(resultado);
                }
            }
            catch (Exception err)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente."+err;
                return Ok(respuesta);
            }
        }



        [HttpPost]
        [Route("RegistrarVuelo")]
        public IActionResult RegistrarVuelo(VuelosEnt entidad)
        {
            var respuesta = new VuelosEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("Registrar_Vuelo",
                        new { entidad.AeropuertoInicio, entidad.AeropuertoDestino, entidad.FechaInicio,
                            entidad.FechaDestino, entidad.Precio, entidad.Descripcion, entidad.Imagen
                        },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se registró la información del vuelo";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su Vuelo fue registrado correctamente";
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
        [Route("ActualizarVuelo")]
        public IActionResult ActualizarVuelo(int IdVuelo, string FechaInicioP, string FechaDestinoP, float Precio, string Descripcion, string Imagen = "" )
        {
            var respuesta = new VuelosEntRespuesta();

            DateTime FechaInicio = DateTime.Parse(FechaDestinoP);
            DateTime FechaDestino = DateTime.Parse(FechaDestinoP);

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("ACTUALIZAR_VUELO",
                        new
                        {
                            IdVuelo,
                            FechaInicio,
                            FechaDestino,
                            Precio,
                            Descripcion,
                            Imagen
                        },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se actualizo la información del vuelo";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "El Vuelo fue actualizado correctamente";
                    respuesta.ResultadoTransaccion = true;
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente."+ ex;
                return Ok(respuesta);
            }
        }



        [HttpPut]
        [Route("EliminarVuelo")]
        public IActionResult EliminarVuelo(int IdVuelo)
        {
            var respuesta = new VuelosEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("ELIMINAR_VUELO",
                        new
                        {
                            IdVuelo
                        },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se elimino la información del vuelo";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "El Vuelo fue eliminado correctamente";
                    respuesta.ResultadoTransaccion = true;
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 3;
                respuesta.Mensaje = "Se presentó un inconveniente." + ex;
                return Ok(respuesta);
            }
        }




    }
}
