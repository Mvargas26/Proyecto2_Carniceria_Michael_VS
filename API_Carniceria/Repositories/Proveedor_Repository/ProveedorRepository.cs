using API_Carniceria.Entidades;
using API_Carniceria.Enums;

namespace API_Carniceria.Repositories.Proveedor_Repository
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly List<Proveedor> _proveedores = new();

        //constru
        public ProveedorRepository()
        {
            AgregarProveedoresEjemplo();
        }//fn

        public Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync()
        {
            IReadOnlyCollection<Proveedor> resultado = _proveedores.ToList();
            return Task.FromResult(resultado);
        }//fn

        public Task<Proveedor?> ObtenerPorIdAsync(Guid id)
        {
            Proveedor? proveedor = _proveedores.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(proveedor);
        }//fn 

        public Task<IReadOnlyCollection<Proveedor>> BuscarAsync(string texto, string criterio)
        {
            List<Proveedor> encontrados;

            if (criterio == "Identificacion")
            {
                encontrados = _proveedores.Where(p => p.Identificacion == texto).ToList();
            }
            else
            {
                encontrados = _proveedores.Where(p => p.RazonSocial == texto).ToList();
            }

            IReadOnlyCollection<Proveedor> resultado = encontrados;
            return Task.FromResult(resultado);
        }//fn

        public Task<Proveedor> CrearAsync(Proveedor proveedor)
        {
            proveedor.Id = Guid.NewGuid();
            _proveedores.Add(proveedor);
            return Task.FromResult(proveedor);

        }//fn

        public Task<bool> ActualizarAsync(Proveedor proveedor)
        {
            Proveedor? actual = _proveedores.FirstOrDefault(p => p.Id == proveedor.Id);

            if (actual == null)
            {
                return Task.FromResult(false);
            }

            actual.TipoIdentificacion = proveedor.TipoIdentificacion;
            actual.Identificacion = proveedor.Identificacion;
            actual.RazonSocial = proveedor.RazonSocial;
            actual.NombreContacto = proveedor.NombreContacto;
            actual.Telefono = proveedor.Telefono;
            actual.CorreoElectronico = proveedor.CorreoElectronico;
            actual.TipoSuministro = proveedor.TipoSuministro;
            actual.FrecuenciaEntrega = proveedor.FrecuenciaEntrega;

            return Task.FromResult(true);
        }//fn




        #region MetodosInternos

        private void AgregarProveedoresEjemplo()
        {
            _proveedores.Add(new Proveedor
            {
                TipoIdentificacion = TipoIdentificacion.CedulaJuridica,
                Identificacion = "3-101-123456",
                RazonSocial = "Distribuidora La Pampa S.A.",
                NombreContacto = "Roberto Alfaro",
                Telefono = "26661234",
                CorreoElectronico = "ventas@lapampa.com",
                TipoSuministro = TipoSuministro.CarneDeRes,
                FrecuenciaEntrega = FrecuenciaEntrega.DosVecesPorSemana
            });

            _proveedores.Add(new Proveedor
            {
                TipoIdentificacion = TipoIdentificacion.CedulaNacional,
                Identificacion = "5-0198-0642",
                RazonSocial = "Empaques del Norte",
                NombreContacto = "Lucía Mendoza",
                Telefono = "88451290",
                CorreoElectronico = "lucia@empaquesnorte.com",
                TipoSuministro = TipoSuministro.Empaques,
                FrecuenciaEntrega = FrecuenciaEntrega.ContraPedido
            });
        }

        #endregion

    }//fn class
}//fn space
