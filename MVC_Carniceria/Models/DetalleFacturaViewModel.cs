using System.ComponentModel.DataAnnotations;

namespace MVC_Carniceria.Models
{
    public class DetalleFacturaViewModel
    {
        public Guid Id { get; set; }

        public Guid FacturaId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [Display(Name = "Producto")]
        public Guid ProductoId { get; set; }

        [Display(Name = "Código")]
        public string CodigoProducto { get; set; } = string.Empty;

        [Display(Name = "Producto")]
        public string NombreProducto { get; set; } = string.Empty;

        [Display(Name = "Unidad de Medida")]
        public int UnidadMedida { get; set; } 

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0.01, 10000, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Precio Unitario")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Subtotal => Math.Round(Cantidad * PrecioUnitario, 2);

        
    }//fn class
}