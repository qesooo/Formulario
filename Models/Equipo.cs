using System;
using System.Collections.Generic;

namespace Formulario_soporte.Models;

public partial class Equipo
{
    public int IdEquipo { get; set; }

    public string Tipo { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public string Activo { get; set; } = null!;

    public int IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; } 

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
}
