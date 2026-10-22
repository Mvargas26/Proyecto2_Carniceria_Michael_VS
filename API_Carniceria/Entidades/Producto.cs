using API_Carniceria.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Carniceria.Entidades
{
    public class Producto
    {
        public Guid Id { get; set; }
        public required string CodigoProducto { get; set; }
        public required string NombreProducto { get; set; }
        public Categoria Categoria { get; set; }
        public UnidadMedida UnidadMedida { get; set; }
        public decimal PrecioPorUnidadMedida { get; set; }
        public decimal ExistenciaActual { get; set; }
        public decimal ExistenciaMinima { get; set; }

        public Producto()
        {
            Id = Guid.NewGuid();//aqui si necesitamos el contru porque no hereda como Persona
        }


    }//fn class
}//fn spcae
