using API_Carniceria.Entidades;
using API_Carniceria.Repositories.Producto_Repository;
using API_Carniceria.Repositories.Proveedor_Repository;

namespace API_Carniceria.Negocios.ProductoNegocios
{
    public class ProductoNegocios : IProductoNegocios
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IProveedorRepository _proveedorRepository;

        //constru
        public ProductoNegocios(IProductoRepository productoRepository,IProveedorRepository proveedorRepository)
        {
            _productoRepository = productoRepository;
            _proveedorRepository = proveedorRepository;
        } // fn

        public Task<IReadOnlyCollection<Producto>> ObtenerTodosAsync()
        {
            return _productoRepository.ObtenerTodosAsync();
        } // fn

        public Task<Producto?> ObtenerPorIdAsync(Guid id)
        {
            return _productoRepository.ObtenerPorIdAsync(id);
        } // fn

        public Task<IReadOnlyCollection<Producto>> BuscarAsync(string texto, string criterio)
        {
            return _productoRepository.BuscarAsync(texto, criterio);
        } // fn

        public async Task<Producto> CrearAsync(Producto producto)
        {
            await ValidarProductoAsync(producto);
            return await _productoRepository.CrearAsync(producto);
        } // fn

        public async Task<bool> ActualizarAsync(Producto producto)
        {
            Producto? actual = await _productoRepository.ObtenerPorIdAsync(producto.Id);

            if (actual == null)
            {
                return false;
            }

            await ValidarProductoAsync(producto);
            return await _productoRepository.ActualizarAsync(producto);
        } // fn

        public async Task<bool> EliminarAsync(Guid id)
        {
            Producto? actual = await _productoRepository.ObtenerPorIdAsync(id);

            if (actual == null)
            {
                return false;
            }

            return await _productoRepository.EliminarAsync(id);
        } // fn

        #region MetodosInternos

        private async Task ValidarProductoAsync(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.CodigoProducto))
            {
                throw new Exception("El código del producto es obligatorio.");
            }

            var productos = await _productoRepository.ObtenerTodosAsync();
            bool codigoRepetido = productos.Any(p =>
                p.Id != producto.Id &&
                p.CodigoProducto.Trim().Equals(producto.CodigoProducto.Trim(), StringComparison.OrdinalIgnoreCase));

            if (codigoRepetido)
            {
                throw new Exception("Ya existe un producto registrado con este código.");
            }

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
            {
                throw new Exception("El nombre del producto es obligatorio.");
            }

            if (producto.PrecioPorUnidadMedida <= 0)
            {
                throw new Exception("El precio por unidad de medida debe ser mayor a cero.");
            }

            if (producto.ExistenciaActual < 0)
            {
                throw new Exception("La existencia actual no puede ser un valor negativo.");
            }

            if (producto.ExistenciaMinima < 0)
            {
                throw new Exception("La existencia mínima no puede ser un valor negativo.");
            }

            if (producto.ProveedorId == Guid.Empty)
            {
                throw new Exception("Debe seleccionar un proveedor principal válido.");
            }

            var proveedorExiste = await _proveedorRepository.ObtenerPorIdAsync(producto.ProveedorId);
            if (proveedorExiste == null)
            {
                throw new Exception("El proveedor principal seleccionado no existe en el sistema.");
            }
        } // fn

        #endregion

    }//fn class
}//fn space
