using System;
using System.Collections.Generic;
using System.Text;
using API_Carniceria.Enums;

namespace API_Carniceria
{
    public abstract class Persona
    {
        public Guid Id { get; set; }

        public TipoIdentificacion TipoIdentificacion { get; set; }
        public required string Identificacion { get; set; }
        public required string Nombre { get; set; }
        public required string PrimerApellido { get; set; }
        public required string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }

        
        protected Persona() //Protected para que lo accedan solo las hijas
        {
            Id = Guid.NewGuid(); //le asignamos en el cosntructor de una vez el Id que es un Guid.
        }

    }//fin class

}//fin namespace
