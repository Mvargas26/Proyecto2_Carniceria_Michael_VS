namespace MVC_Carniceria.Services
{
    // Clase base que centraliza las llamadas al API
    public class ApiBaseService
    {
        protected readonly HttpClient _clienteHttp;

        public ApiBaseService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

    }//fn class
}//fn space
