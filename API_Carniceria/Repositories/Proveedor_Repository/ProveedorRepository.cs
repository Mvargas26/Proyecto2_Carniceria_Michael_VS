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
        }

        public Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync()
        {
            IReadOnlyCollection<Proveedor> resultado = _proveedores.ToList();
            return Task.FromResult(resultado);
        }

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
