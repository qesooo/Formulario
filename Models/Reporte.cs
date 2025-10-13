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

    public string? MantenimientoLogico { get; set; } = string.Empty;
    public string? MantenimientoFisico { get; set; } = string.Empty;
    public string? MantenimientoReemplazo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = null!;

    public string? TrabajoRealizado { get; set; }

    public string Estado { get; set; } = null!;

    public int IdUsuarioResponsable { get; set; }

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual Sucursal? IdSucursalNavigation { get; set; }

    public virtual Usuario? IdUsuarioResponsableNavigation { get; set; }

    public virtual Usuario? IdUsuarioTecnicoNavigation { get; set; }

    public byte[]? Firma { get; set; }
}
