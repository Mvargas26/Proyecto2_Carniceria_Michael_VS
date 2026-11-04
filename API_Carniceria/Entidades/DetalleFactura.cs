using API_Carniceria.Enums;

namespace API_Carniceria.Entidades
{
    public class DetalleFactura
    {
        public Guid Id { get; set; }
        public Guid FacturaId { get; set; }
        public Guid ProductoId { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public UnidadMedida UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Math.Round(Cantidad * PrecioUnitario, 2);

        public DetalleFactura()
        {
            Id = Guid.NewGuid();
        }


    }//fn class
}//fn space
