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

    // Mantenimiento lógico
    public bool Defragmentar { get; set; }
    public bool WindowsConfiguracion { get; set; }
    public bool ActualizacionDrivers { get; set; }

    // Mantenimiento físico
    public bool LimpiezaTotal { get; set; }
    public bool DiscoDuroFisico { get; set; }
    public bool TecladoFisico { get; set; }
    public bool MemoriaFisico { get; set; }
    public bool RedFisico { get; set; }
    public bool PantallaMonitorFisico { get; set; }

    // Mantenimiento reemplazo
    public bool DiscoDuroReemplazo { get; set; }
    public bool TecladoReemplazo { get; set; }
    public bool MemoriaReemplazo { get; set; }
    public bool RedReemplazo { get; set; }
    public bool PantallaMonitorReemplazo { get; set; }

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
