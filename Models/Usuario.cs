using System;
using System.Collections.Generic;

namespace Formulario_soporte.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();

    public virtual ICollection<Reporte> ReporteIdUsuarioResponsableNavigations { get; set; } = new List<Reporte>();

    public virtual ICollection<Reporte> ReporteIdUsuarioTecnicoNavigations { get; set; } = new List<Reporte>();
}
