namespace proyectoAgenciaApi.Utilitarios
{
    public interface IUtils
    {
        public string CreatePassword(int length);

        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);
    }
}
