using System.ComponentModel.DataAnnotations;

namespace MVC_Carniceria.Models
{
    public class EmpleadoViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de identificación.")]
        [Display(Name = "Tipo de Identificación")]
        public int? TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El primer apellido debe tener entre 3 y 75 caracteres.")]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El segundo apellido debe tener entre 3 y 75 caracteres.")]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El salario mensual es obligatorio.")]
        [Range(1, 5000000, ErrorMessage = "El salario debe ser mayor a cero y hasta ₡5,000,000.")]
        [Display(Name = "Salario Mensual")]
        public decimal SalarioMensual { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe seleccionar un área de trabajo.")]
        [Display(Name = "Área de Trabajo")]
        public int? AreaTrabajo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un puesto.")]
        [Display(Name = "Puesto")]
        public int? Puesto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un turno.")]
        [Display(Name = "Turno")]
        public int? Turno { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{Nombre} {PrimerApellido} {SegundoApellido}".Trim();
    }
}//fn space
