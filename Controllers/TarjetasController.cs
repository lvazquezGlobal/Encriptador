using Microsoft.AspNetCore.Mvc;
using Tarjetas_ENC_Liber.Interfaces;

namespace Tarjetas_ENC_Liber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarjetasController : ControllerBase
    {

      private ICardEncryptionService encriptadorService;
        public TarjetasController(ICardEncryptionService _encriptadorService)
        {
            encriptadorService = _encriptadorService;
        }
        [HttpPost("[controller]/Encrypt")]
        public string GetEncTarjeta(string nroTarjeta)
        {
            return encriptadorService.Encrypt(nroTarjeta);
        }

        [HttpPost("[controller]/Decrypt")]
        public void GetDesTarjeta(string nroTarjeta)
        {
            encriptadorService.Decrypt(nroTarjeta);
        }
    }
}