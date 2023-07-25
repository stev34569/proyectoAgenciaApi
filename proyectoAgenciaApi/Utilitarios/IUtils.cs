namespace proyectoAgenciaApi.Utilitarios
{
    public interface IUtils
    {
        public string GenerarClaveTemporal(int length);

        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);

        public string Encriptar(string toEncrypt);

        public string GenerarToken(long idUsuario);
    }
}
