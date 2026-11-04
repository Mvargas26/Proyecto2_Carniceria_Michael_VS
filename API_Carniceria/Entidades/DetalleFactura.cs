namespace API_Carniceria.Entidades
{
    public class DetalleFactura
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FacturaId { get; set; }
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }//fn class
}//fn space
