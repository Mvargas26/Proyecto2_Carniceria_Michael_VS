using System.ComponentModel.DataAnnotations;

namespace MVC_Carniceria.Models
{
    public class ProductoViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código del producto es obligatorio.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El código debe tener entre 1 y 20 caracteres.")]
        public string CodigoProducto { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreProducto { get; set; } = string.Empty;

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public int? Categoria { get; set; }

        [Display(Name = "Unidad de Medida")]
        [Required(ErrorMessage = "Debe seleccionar una unidad de medida.")]
        public int? UnidadMedida { get; set; }

        [Display(Name = "Precio por Unidad")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal? PrecioPorUnidadMedida { get; set; }

        [Display(Name = "Existencia Actual")]
        [Required(ErrorMessage = "La existencia actual es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "La existencia actual debe ser igual o mayor que cero.")]
        public decimal? ExistenciaActual { get; set; }

        [Display(Name = "Existencia Mínima")]
        [Required(ErrorMessage = "La existencia mínima es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "La existencia mínima debe ser igual o mayor que cero.")]
        public decimal? ExistenciaMinima { get; set; }

        [Display(Name = "Proveedor Principal")]
        [Required(ErrorMessage = "Debe seleccionar el proveedor principal.")]
        public Guid? ProveedorId { get; set; }

        // Propiedad complementaria para mostrar el nombre del proveedor en tablas y vistas
        public string? RazonSocialProveedor { get; set; }

        // Propiedad calculada para disparar el badge o clase de advertencia (danger) en la vista
        public bool RequiereReabastecimiento =>
            ExistenciaActual.HasValue &&
            ExistenciaMinima.HasValue &&
            ExistenciaActual.Value <= ExistenciaMinima.Value;

    } // fn class
}//fn space
