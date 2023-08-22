using Dapper;
using proyectoAgenciaApi.Entities;
using proyectoAgenciaApi.Utilitarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;

namespace proyectoAgenciaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BitacoraController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUtils _utils;

        public BitacoraController(IConfiguration configuration, IUtils utils)
        {
            _configuration = configuration;
            _utils = utils;
        }

        [HttpPost]
        [Route("RegistrarBitacora")]
        public IActionResult RegistrarBitacora(BitacoraEnt entidad)
        {
            try
            {
                var IdUsuario = long.Parse(User.Identity.Name.ToString());

                using (var connection = new SqlConnection(_configuration.GetConnectionString("proyectoAgencia")))
                {
                    connection.Execute("REGISTRAR_ERROR",
                        new { entidad.Origen, entidad.Mensaje, IdUsuario, entidad.DireccionIP },
                        commandType: System.Data.CommandType.StoredProcedure);

                }
            }
            catch (Exception)
            {                
            }

            return Ok();
        }

    }
}
