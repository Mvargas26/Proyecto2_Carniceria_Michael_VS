using API_Carniceria.Enums;

namespace API_Carniceria.Entidades
{
    public class Factura
    {
        public Guid Id { get; set; } 
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public Guid ClienteId { get; set; }
        public string IdentificacionCliente { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public Guid EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public MetodoPago MetodoPago { get; set; }
        public EstadoFactura Estado { get; set; } = EstadoFactura.Pendiente;
        public decimal PorcentajeDescuento { get; set; }
        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
        public decimal Subtotal => Detalles?.Sum(d => d.Subtotal) ?? 0m;
        public decimal MontoDescuento => Math.Round((Subtotal * PorcentajeDescuento) / 100m, 2);
        public decimal BaseGravable => Subtotal - MontoDescuento;
        public decimal Impuesto => Math.Round(BaseGravable * 0.13m, 2);
        public decimal Total => BaseGravable + Impuesto;

        //constru
        public Factura()
        {
            Id = Guid.NewGuid();
        }
    }//fn clas
}//fn space
