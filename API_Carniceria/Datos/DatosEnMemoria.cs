using API_Carniceria.Entidades;

namespace Proyecto1_Carniceria_MichaelVS.Datos
{
    using API_Carniceria.Entidades;
    using API_Carniceria.Enums;

    public class DatosEnMemoria //aqui funciona como el almacenamiento solo q en memoria.
    {
        public List<Producto> Productos { get; set; } = new List<Producto>();
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public List<Empleado> Empleados { get; set; } = new List<Empleado>();
        public List<Proveedor> Proveedores { get; set; } = new List<Proveedor>();

        
        // En el Constructor cargamos los datos de ejemplo
        public DatosEnMemoria()
        {
            AgregarProductosEjemplo();
            AgregarClientesEjemplo();
            AgregarEmpleadoEjemplo();
            AgregarProveedorEjemplo();
        }


        #region Metodos internos

        private void AgregarProductosEjemplo()
        {
            Productos.Add(new Producto
            {
                CodigoProducto = "RES001",
                NombreProducto = "Costilla de res",
                Categoria = Categoria.CarneDeRes,
                UnidadMedida = UnidadMedida.Kilogramo,
                PrecioPorUnidadMedida = 5500m,
                ExistenciaActual = 25m,
                ExistenciaMinima = 10m
            });

            Productos.Add(new Producto
            {
                CodigoProducto = "EMB001",
                NombreProducto = "Chorizo artesanal",
                Categoria = Categoria.Embutidos,
                UnidadMedida = UnidadMedida.Paquete,
                PrecioPorUnidadMedida = 2800m,
                ExistenciaActual = 4m,
                ExistenciaMinima = 6m
            });
        }

        private void AgregarClientesEjemplo()
        {
            Clientes.Add(new Cliente
            {
                TipoIdentificacion = TipoIdentificacion.CedulaNacional,
                Identificacion = "5-0123-0456",
                Nombre = "María",
                PrimerApellido = "Rodríguez",
                SegundoApellido = "Vargas",
                FechaNacimiento = new DateTime(1985, 8, 15)
            });

            Clientes.Add(new Cliente
            {
                TipoIdentificacion = TipoIdentificacion.Pasaporte,
                Identificacion = "AB123456",
                Nombre = "Carlos",
                PrimerApellido = "Jiménez",
                SegundoApellido = "Mora",
                FechaNacimiento = new DateTime(1990, 3, 22)
            });
        }

        private void AgregarEmpleadoEjemplo()
        {
            Empleados.Add(new Empleado
            {
                TipoIdentificacion = TipoIdentificacion.CedulaNacional,
                Identificacion = "5-0250-0731",
                Nombre = "Andrea",
                PrimerApellido = "Chavarría",
                SegundoApellido = "Salazar",
                FechaNacimiento = new DateTime(1992, 11, 5),
                SalarioMensual = 650000m,
                FechaIngreso = new DateTime(2022, 2, 14),
                AreaTrabajo = AreaTrabajo.VentasYCaja,
                Puesto = Puesto.Cajero,
                Turno = Turno.Manana
            });

            Empleados.Add(new Empleado
            {
                TipoIdentificacion = TipoIdentificacion.Dimex,
                Identificacion = "155812345678",
                Nombre = "Jorge",
                PrimerApellido = "Martínez",
                SegundoApellido = "Quesada",
                FechaNacimiento = new DateTime(1988, 6, 30),
                SalarioMensual = 780000m,
                FechaIngreso = new DateTime(2020, 9, 1),
                AreaTrabajo = AreaTrabajo.CorteYPreparacion,
                Puesto = Puesto.Carnicero,
                Turno = Turno.Mixto
            });
        }

        private void AgregarProveedorEjemplo()
        {
            Proveedores.Add(new Proveedor
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

            Proveedores.Add(new Proveedor
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

