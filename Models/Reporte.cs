using System;
using System.Collections.Generic;

namespace Formulario_soporte.Models;

public partial class Reporte
{
    public int IdReporte { get; set; }

    public int IdUsuarioTecnico { get; set; }

    public int IdEquipo { get; set; }

    public int IdSucursal { get; set; }

    public DateOnly FechaReporte { get; set; }

    public string TipoMantenimiento { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? TrabajoRealizado { get; set; }

    public string Estado { get; set; } = null!;

    public int IdUsuarioResponsable { get; set; }

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual Sucursal IdSucursalNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioResponsableNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioTecnicoNavigation { get; set; } = null!;

    public byte[]? Firma { get; set; }
}
