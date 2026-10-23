using System.ComponentModel.DataAnnotations;

namespace MVC_Carniceria.Models
{
    public class ProveedorViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de identificación.")]
        public int? TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La razón social o nombre comercial es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres.")]
        public string RazonSocial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la persona de contacto es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres.")]
        public string NombreContacto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un tipo de suministro.")]
        public int? TipoSuministro { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una frecuencia de entrega.")]
        public int? FrecuenciaEntrega { get; set; }

    }// fin class
}// fin space
