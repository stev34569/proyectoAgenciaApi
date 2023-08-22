using Dapper;
using proyectoAgenciaApi.Entities;
using proyectoAgenciaApi.Utilitarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace proyectoAgenciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    
    public class CarritoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CarritoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        [Route("AgregarPaqueteCarrito")]
        public IActionResult AgregarPaqueteCarrito(CarritoEnt entidad)
        {
            var respuesta = new CarritoEntRespuesta();
            entidad.IdUsuario = long.Parse(User.Identity.Name.ToString());

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("REGISTRAR_PAQUETE_CARRITO",
                        new { entidad.IdPaquete, entidad.IdUsuario },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Este Paquete ya fue comprado o agregado a su carrito anteriormente";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su Paquete fue registrado correctamente";
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


        [HttpDelete]
        [Route("RemoverPaqueteCarrito")]
        public IActionResult RemoverPaqueteCarrito(long IdCarrito)
        {
            var respuesta = new CarritoEntRespuesta();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("REMOVER_PAQUETE_CARRITO",
                        new { IdCarrito },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se ha removido el Paquete de su carrito";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su Paquete fue removido correctamente";
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


        [HttpPost]
        [Route("ConfirmarPago")]
        public IActionResult ConfirmarPago()
        {
            var respuesta = new CarritoEntRespuesta();
            var IdUsuario = long.Parse(User.Identity.Name.ToString());

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    int confirmacion = connection.Execute("REGISTRAR_PAGO_CARRITO",
                        new { IdUsuario },
                        commandType: System.Data.CommandType.StoredProcedure);

                    if (confirmacion <= 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se ha realizado el pago de sus Paquetes";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
                    respuesta.Mensaje = "Su Paquetes fueron pagados correctamente";
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
        [Route("ConsultarResumenCarrito")]
        public IActionResult ConsultarResumenCarrito()
        {
            var resultado = new CarritoEnt();
            var respuesta = new CarritoEntRespuesta();
            long IdUsuario = long.Parse(User.Identity.Name.ToString());

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<CarritoEnt>("CONSULTAR_RESUMEN_CARRITO",
                        new { IdUsuario },
                        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                    if (resultado == null)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de su carrito";
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


        [HttpGet]
        [Route("ConsultarDetalleCarrito")]
        public IActionResult ConsultarDetalleCarrito()
        {
            var resultado = new List<CarritoEnt>();
            var respuesta = new CarritoEntRespuesta();
            long IdUsuario = long.Parse(User.Identity.Name.ToString());

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    resultado = connection.Query<CarritoEnt>("CONSULTAR_DETALLE_CARRITO",
                        new { IdUsuario },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (resultado.Count == 0)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "No se encontró la información de su carrito";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = 1;
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

    }
}
