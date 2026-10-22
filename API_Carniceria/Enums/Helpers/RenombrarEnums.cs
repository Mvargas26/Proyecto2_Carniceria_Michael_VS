using System;
using System.Collections.Generic;
using System.Text;

namespace API_Carniceria.Enums.Helpers
{
    //Esta clase permite mostrar los nombres de los enum de manera más amigable para el usuario final.
    public static class RenombrarEnums
    {
        public static string ObtenerNombreVisible(this AreaTrabajo area)
        {
            switch (area)
            {
                case AreaTrabajo.Administracion:
                    return "Administración";
                case AreaTrabajo.VentasYCaja:
                    return "Ventas y Caja";
                case AreaTrabajo.CorteYPreparacion:
                    return "Corte y Preparación";
                case AreaTrabajo.BodegaEInventario:
                    return "Bodega e Inventario";
                case AreaTrabajo.Distribucion:
                    return "Distribución";
                case AreaTrabajo.Limpieza:
                    return "Limpieza";
                default:
                    return area.ToString();
            }
        }

        public static string ObtenerNombreVisible(this Puesto puesto)
        {
            switch (puesto)
            {
                case Puesto.Administrador:
                    return "Administrador";
                case Puesto.AsistenteAdministrativo:
                    return "Asistente Administrativo";
                case Puesto.Cajero:
                    return "Cajero";
                case Puesto.Vendedor:
                    return "Vendedor";
                case Puesto.ServicioAlCliente:
                    return "Servicio al Cliente";
                case Puesto.Carnicero:
                    return "Carnicero";
                case Puesto.Despostador:
                    return "Despostador";
                case Puesto.Empacador:
                    return "Empacador";
                case Puesto.Bodeguero:
                    return "Bodeguero";
                case Puesto.AuxiliarDeInventario:
                    return "Auxiliar de Inventario";
                case Puesto.Conductor:
                    return "Conductor";
                case Puesto.AyudanteDeDistribucion:
                    return "Ayudante de Distribución";
                case Puesto.PersonalDeLimpieza:
                    return "Personal de Limpieza";
                default:
                    return puesto.ToString();
            }
        }

        public static string ObtenerNombreVisible(this Turno turno)
        {
            switch (turno)
            {
                case Turno.Manana:
                    return "Mañana";
                case Turno.Tarde:
                    return "Tarde";
                case Turno.Mixto:
                    return "Mixto";
                default:
                    return turno.ToString();
            }
        }

        public static string ObtenerNombreVisible(this Categoria categoria)
        {
            switch (categoria)
            {
                case Categoria.CarneDeRes:
                    return "Carne de Res";
                case Categoria.CarneDeCerdo:
                    return "Carne de Cerdo";
                case Categoria.Pollo:
                    return "Pollo";
                case Categoria.Embutidos:
                    return "Embutidos";
                case Categoria.ProductosProcesados:
                    return "Productos Procesados";
                case Categoria.Congelados:
                    return "Congelados";
                case Categoria.Otros:
                    return "Otros";
                default:
                    return categoria.ToString();
            }
        }

        public static string ObtenerNombreVisible(this UnidadMedida unidad)
        {
            switch (unidad)
            {
                case UnidadMedida.Kilogramo:
                    return "Kilogramo";
                case UnidadMedida.Libra:
                    return "Libra";
                case UnidadMedida.Unidad:
                    return "Unidad";
                case UnidadMedida.Paquete:
                    return "Paquete";
                case UnidadMedida.Bandeja:
                    return "Bandeja";
                default:
                    return unidad.ToString();
            }
        }

        public static string ObtenerNombreVisible(this TipoSuministro suministro)
        {
            switch (suministro)
            {
                case TipoSuministro.CarneDeRes:
                    return "Carne de Res";
                case TipoSuministro.CarneDeCerdo:
                    return "Carne de Cerdo";
                case TipoSuministro.Pollo:
                    return "Pollo";
                case TipoSuministro.Embutidos:
                    return "Embutidos";
                case TipoSuministro.ProductosProcesados:
                    return "Productos Procesados";
                case TipoSuministro.Empaques:
                    return "Empaques";
                case TipoSuministro.Condimentos:
                    return "Condimentos";
                case TipoSuministro.Otros:
                    return "Otros";
                default:
                    return suministro.ToString();
            }
        }

        public static string ObtenerNombreVisible(this FrecuenciaEntrega frecuencia)
        {
            switch (frecuencia)
            {
                case FrecuenciaEntrega.Diaria:
                    return "Diaria";
                case FrecuenciaEntrega.DosVecesPorSemana:
                    return "Dos veces por semana";
                case FrecuenciaEntrega.Semanal:
                    return "Semanal";
                case FrecuenciaEntrega.Quincenal:
                    return "Quincenal";
                case FrecuenciaEntrega.ContraPedido:
                    return "Contra pedido";
                default:
                    return frecuencia.ToString();
            }
        }

        public static string ObtenerNombreVisible(this TipoIdentificacion tipo)
        {
            switch (tipo)
            {
                case TipoIdentificacion.CedulaNacional:
                    return "Cédula Nacional";
                case TipoIdentificacion.Dimex:
                    return "DIMEX";
                case TipoIdentificacion.Pasaporte:
                    return "Pasaporte";
                case TipoIdentificacion.CedulaJuridica:
                    return "Cédula Jurídica";
                default:
                    return tipo.ToString();
            }
        }



    }//fn class
}//fn space
