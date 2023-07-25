using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace proyectoAgenciaApi.Utilitarios
{
    public class Utils : IUtils
    {
        private readonly IConfiguration _configuration;

        public Utils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarClaveTemporal(int length)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje)
        {
            string correoSMTP = _configuration.GetSection("Variables:correoSMTP").Value;
            string claveSMTP = _configuration.GetSection("Variables:claveSMTP").Value;

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(Destinatario));
            msg.From = new MailAddress(correoSMTP);
            msg.Subject = Asunto;
            msg.Body = Mensaje;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(correoSMTP, claveSMTP);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);
        }

        public string Encriptar(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string key = "vJlg369z5KkIOoT41F3tgfFPvbwlrnJr";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string GenerarToken(long idUsuario)
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, Encriptar(idUsuario.ToString()))
                new Claim(ClaimTypes.Name, idUsuario.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}