namespace MVC_Carniceria.Servicios
{
    // Ene sta clase centralizmosa las llamadas HTTP al API
    public class ClienteApi
    {
        protected readonly HttpClient _clienteHttp;

        public ClienteApi(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }


    }//fin class
}//fn space
