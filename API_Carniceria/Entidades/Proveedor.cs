using API_Carniceria.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Carniceria.Entidades
{
    public class Proveedor
    {
        public Guid Id { get; set; }
        public TipoIdentificacion TipoIdentificacion { get; set; }
        public required string Identificacion { get; set; }
        public required string RazonSocial { get; set; }
        public required string NombreContacto { get; set; }
        public required string Telefono { get; set; }
        public required string CorreoElectronico { get; set; }
        public TipoSuministro TipoSuministro { get; set; }
        public FrecuenciaEntrega FrecuenciaEntrega { get; set; }

        public Proveedor()
        {
            Id = Guid.NewGuid();
        }
    }
}
