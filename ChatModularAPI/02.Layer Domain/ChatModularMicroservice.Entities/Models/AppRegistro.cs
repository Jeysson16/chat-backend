using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{

 [Table("AppRegistros")]
public class AppRegistro : BaseModel
{
    [PrimaryKey("nAppRegistrosId")]
    public int nAppRegistroId { get; set; }
    
    [Column("nAppRegistrosAplicacionId")]
    public int nAppRegistrosAplicacionId { get; set; }
    
    [Column("cAppRegistrosCodigoApp")]
    public string cAppRegistroCodigoApp { get; set; } = string.Empty;
    
    [Column("cAppRegistrosNombreApp")]
    public string cAppRegistroNombreApp { get; set; } = string.Empty;
    
    [Column("cAppRegistrosTokenAcceso")]
    public string cAppRegistroTokenAcceso { get; set; } = string.Empty;
    
    [Column("cAppRegistrosSecretoApp")]
    public string? cAppRegistroSecretoApp { get; set; }
    
    [Column("bAppRegistrosEsActivo")]
    public bool bAppRegistroEsActivo { get; set; } = true;
    
    [Column("dAppRegistrosFechaCreacion")]
    public DateTime dAppRegistroFechaCreacion { get; set; } = DateTime.UtcNow;
    
    [Column("dAppRegistrosFechaExpiracion")]
    public DateTime? dAppRegistroFechaExpiracion { get; set; }
    
    [Column("cAppRegistrosConfiguracionesAdicionales")]
    public string? cAppRegistroConfiguracionesAdicionales { get; set; }
    
    // Alias properties for compatibility
    public int Id 
    { 
        get => nAppRegistroId; 
        set => nAppRegistroId = value; 
    }
    
    public string cAppCodigo 
    { 
        get => cAppRegistroCodigoApp; 
        set => cAppRegistroCodigoApp = value; 
    }
    
    public string cAccessToken 
    { 
        get => cAppRegistroTokenAcceso; 
        set => cAppRegistroTokenAcceso = value; 
    }
    
    public DateTime CreatedAt 
    { 
        get => dAppRegistroFechaCreacion; 
        set => dAppRegistroFechaCreacion = value; 
    }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool bAppActivo 
    { 
        get => bAppRegistroEsActivo; 
        set => bAppRegistroEsActivo = value; 
    }
    
    public string cSecretToken 
    { 
        get => cAppRegistroSecretoApp ?? string.Empty; 
        set => cAppRegistroSecretoApp = value; 
    }
    
    public string cAppUrl { get; set; } = string.Empty;
}

}
