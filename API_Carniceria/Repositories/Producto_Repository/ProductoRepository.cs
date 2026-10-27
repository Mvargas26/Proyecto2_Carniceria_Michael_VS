using API_Carniceria.Entidades;
using API_Carniceria.Enums;
using API_Carniceria.Repositories.Proveedor_Repository;
using System.Xml.Linq;

namespace API_Carniceria.Repositories.Producto_Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly List<Producto> _productos = new();

        // Constructor: Recibe IProveedorRepository para vincular el producto por defecto
        // asi ya nace con un proveedor registrado 
        public ProductoRepository(IProveedorRepository proveedorRepository)
        {
            var proveedores = proveedorRepository.ObtenerTodosAsync().Result;
            var primerProveedor = proveedores.FirstOrDefault();
            Guid proveedorIdDefecto = primerProveedor?.Id ?? Guid.Empty;

            AgregarProductosEjemplo(proveedorIdDefecto);

        } // fn

        public Task<IReadOnlyCollection<Producto>> ObtenerTodosAsync()
        {
            IReadOnlyCollection<Producto> resultado = _productos.ToList();
            return Task.FromResult(resultado);
        } // fn

        public Task<Producto?> ObtenerPorIdAsync(Guid id)
        {
            Producto? producto = _productos.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(producto);
        } // fn

        public Task<IReadOnlyCollection<Producto>> BuscarAsync(string texto, string criterio)
        {
            List<Producto> encontrados;

            if (criterio == "Codigo")
            {
                encontrados = _productos.Where(p => p.CodigoProducto == texto).ToList();
            }
            else
            {
                encontrados = _productos.Where(p => p.NombreProducto.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            IReadOnlyCollection<Producto> resultado = encontrados;
            return Task.FromResult(resultado);
        } // fn

        public Task<Producto> CrearAsync(Producto producto)
        {
            producto.Id = Guid.NewGuid();
            _productos.Add(producto);
            return Task.FromResult(producto);
        } // fn

        public Task<bool> ActualizarAsync(Producto producto)
        {
            Producto? actual = _productos.FirstOrDefault(p => p.Id == producto.Id);

            if (actual == null)
            {
                return Task.FromResult(false);
            }

            actual.CodigoProducto = producto.CodigoProducto;
            actual.NombreProducto = producto.NombreProducto;
            actual.Categoria = producto.Categoria;
            actual.UnidadMedida = producto.UnidadMedida;
            actual.PrecioPorUnidadMedida = producto.PrecioPorUnidadMedida;
            actual.ExistenciaActual = producto.ExistenciaActual;
            actual.ExistenciaMinima = producto.ExistenciaMinima;
            actual.ProveedorId = producto.ProveedorId;

            return Task.FromResult(true);
        } // fn

        public Task<bool> EliminarAsync(Guid id)
        {
            Producto? actual = _productos.FirstOrDefault(p => p.Id == id);

            if (actual == null)
            {
                return Task.FromResult(false);
            }

            _productos.Remove(actual);
            return Task.FromResult(true);
        } // fn

        #region MetodosInternos

        private void AgregarProductosEjemplo(Guid proveedorId)
        {
            _productos.Add(new Producto
            {
                CodigoProducto = "PROD-001",
                NombreProducto = "Lomito de Res",
                Categoria = Categoria.CarneDeRes,
                UnidadMedida = UnidadMedida.Kilogramo,
                PrecioPorUnidadMedida = 9500m,
                ExistenciaActual = 25m,
                ExistenciaMinima = 5m,
                ProveedorId = proveedorId
            });

            _productos.Add(new Producto
            {
                CodigoProducto = "PROD-002",
                NombreProducto = "Bistec Especial",
                Categoria = Categoria.CarneDeRes,
                UnidadMedida = UnidadMedida.Kilogramo,
                PrecioPorUnidadMedida = 6200m,
                ExistenciaActual = 40m,
                ExistenciaMinima = 10m,
                ProveedorId = proveedorId
            });
        }

        #endregion



    }//fn class
}//fn space
