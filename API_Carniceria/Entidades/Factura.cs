namespace API_Carniceria.Entidades
{
    public class Factura
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int NumeroFactura { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public Guid ClienteId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public Guid EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
        public decimal Subtotal => Detalles?.Sum(d => d.Subtotal) ?? 0;
        public decimal Impuesto => Subtotal * 0.13m;
        public decimal Total => Subtotal + Impuesto;
    }//fn clas
}//fn space
