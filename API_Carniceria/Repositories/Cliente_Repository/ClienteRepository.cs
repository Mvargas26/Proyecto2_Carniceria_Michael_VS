using API_Carniceria.Entidades;
using API_Carniceria.Enums;

namespace API_Carniceria.Repositories.Cliente_Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly List<Cliente> _clientes = new();

        public ClienteRepository()
        {
            AgregarClienteEjemplo();
        }

        public Task<List<Cliente>> ObtenerTodosAsync()
        {
            return Task.FromResult(_clientes.ToList());
        }//fn

        public Task<Cliente?> ObtenerPorIdAsync(Guid id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(cliente);
        }//fn

        public Task<Cliente?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            var cliente = _clientes.FirstOrDefault(c =>
                string.Equals(c.Identificacion?.Trim(), identificacion?.Trim(), StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(cliente);
        }//fn

        public Task RegistrarAsync(Cliente cliente)
        {
            _clientes.Add(cliente);
            return Task.CompletedTask;
        }//fn

        public Task ActualizarAsync(Cliente cliente)
        {
            var indice = _clientes.FindIndex(c => c.Id == cliente.Id);
            if (indice != -1)
            {
                _clientes[indice] = cliente;
            }
            return Task.CompletedTask;
        }//fn

        public Task EliminarAsync(Guid id)
        {
            _clientes.RemoveAll(c => c.Id == id);
            return Task.CompletedTask;
        }//fn

        public Task<bool> YaExisteEstaIdentificacionAsync(string identificacion, Guid? excluirId = null)
        {
            string identificacionBuscada = identificacion.Trim().ToUpper();

            foreach (var cliente in _clientes)
            {
                // Si estamos editando y coincide con el mismo cliente, lo saltamos
                if (excluirId.HasValue && cliente.Id == excluirId.Value)
                {
                    continue;
                }

                // Limpiamos la identificación del registro actual para comparar en igualdad
                string identificacionExistente = cliente.Identificacion.Trim().ToUpper();

                // 3. Si las identificaciones coinciden, encontramos un duplicado
                if (identificacionExistente == identificacionBuscada)
                {
                    return Task.FromResult(true);
                }
            }

            // 4. Si recorrió toda la lista y no hubo coincidencias
            return Task.FromResult(false);
        } // fn

        #region Metodos Internos

        private void AgregarClienteEjemplo()
        {
            _clientes.Add(new Cliente
            {
                TipoIdentificacion = TipoIdentificacion.Fisica,
                Identificacion = "1-1234-0567",
                Nombre = "María",
                PrimerApellido = "González",
                SegundoApellido = "Rojas",
                FechaNacimiento = new DateTime(1992, 4, 18)
            });
        }

        #endregion
    }
}
