using System.ComponentModel.DataAnnotations;

namespace MVC_Carniceria.Models
{
    public class FacturaViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Número de Factura")]
        public string NumeroFactura { get; set; } = string.Empty;

        [Display(Name = "Fecha y Hora de Emisión")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar una persona cliente.")]
        [Display(Name = "Cliente")]
        public Guid ClienteId { get; set; }

        [Display(Name = "Identificación del Cliente")]
        public string IdentificacionCliente { get; set; } = string.Empty;

        [Display(Name = "Nombre del Cliente")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una persona empleada.")]
        [Display(Name = "Empleado Responsable")]
        public Guid EmpleadoId { get; set; }

        [Display(Name = "Empleado")]
        public string NombreEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un método de pago.")]
        [Display(Name = "Método de Pago")]
        public int? MetodoPago { get; set; }

        [Display(Name = "Método de Pago")]
        public string NombreMetodoPago { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public int Estado { get; set; } = 1;

        [Display(Name = "Estado")]
        public string NombreEstado { get; set; } = "Pendiente";

        [Required(ErrorMessage = "El porcentaje de descuento es obligatorio.")]
        [Range(0, 25, ErrorMessage = "El porcentaje de descuento debe estar entre 0% y 25%.")]
        [Display(Name = "Descuento (%)")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        public List<DetalleFacturaViewModel> Detalles { get; set; } = new List<DetalleFacturaViewModel>();

        [Display(Name = "Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Subtotal => Math.Round(Detalles?.Sum(d => d.Subtotal) ?? 0m, 2);

        [Display(Name = "Monto de Descuento")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal MontoDescuento => Math.Round((Subtotal * PorcentajeDescuento) / 100m, 2);

        [Display(Name = "Base Gravable")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal BaseGravable => Subtotal - MontoDescuento;

        [Display(Name = "IVA (13%)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Impuesto => Math.Round(BaseGravable * 0.13m, 2);

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total => BaseGravable + Impuesto;
    }
}