using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{

[Table("TokenRegistros")]
public class TokenRegistro : BaseModel
{
    [PrimaryKey("nTokenRegistrosId")]
    public int nTokenRegistroId { get; set; }
    
    // For compatibility with AuthService
    public long nTokenCodigo => nTokenRegistroId;
    
    [Column("cTokenRegistrosCodigoApp")]
    public string cTokenRegistroCodigoApp { get; set; } = string.Empty;
    
    [Column("cTokenRegistrosPerJurCodigo")]
    public string cTokenRegistroPerJurCodigo { get; set; } = string.Empty;
    
    [Column("cTokenRegistrosPerCodigo")]
    public string cTokenRegistroPerCodigo { get; set; } = string.Empty;
    
    [Column("cTokenRegistrosJwtToken")]
    public string cTokenRegistroJwtToken { get; set; } = string.Empty;
    
    [Column("cTokenRegistrosRefreshToken")]
    public string? cTokenRegistroRefreshToken { get; set; }
    
    [Column("cTokenRegistrosUsuarioId")]
    public string? cTokenRegistroUsuarioId { get; set; }
    
    [Column("dTokenRegistrosFechaExpiracion")]
    public DateTime dTokenRegistroFechaExpiracion { get; set; }
    
    [Column("bTokenRegistrosEsActivo")]
    public bool bTokenRegistroEsActivo { get; set; } = true;
    
    [Column("dTokenRegistrosFechaCreacion")]
    public DateTime dTokenRegistroFechaCreacion { get; set; } = DateTime.UtcNow;
    
    // Alias properties for compatibility
    public string cAppCodigo 
    { 
        get => cTokenRegistroCodigoApp; 
        set => cTokenRegistroCodigoApp = value; 
    }
    
    public string cTokenJWT 
    { 
        get => cTokenRegistroJwtToken; 
        set => cTokenRegistroJwtToken = value; 
    }
    
    public string cPerJurCodigo 
    { 
        get => cTokenRegistroPerJurCodigo; 
        set => cTokenRegistroPerJurCodigo = value; 
    }
    
    public string cPerCodigo 
    { 
        get => cTokenRegistroPerCodigo; 
        set => cTokenRegistroPerCodigo = value; 
    }
    
    public bool bTokenActivo 
    { 
        get => bTokenRegistroEsActivo; 
        set => bTokenRegistroEsActivo = value; 
    }
    
    public DateTime? dTokenExpiracion 
    { 
        get => dTokenRegistroFechaExpiracion; 
        set => dTokenRegistroFechaExpiracion = value ?? DateTime.UtcNow; 
    }
    
    public string? cUsuarioId 
    { 
        get => cTokenRegistroUsuarioId; 
        set => cTokenRegistroUsuarioId = value; 
    }
    
    // Additional alias properties for compatibility
    public string cTokenTipo { get; set; } = "JWT";
    
    public string cTokenValor 
    { 
        get => cTokenRegistroJwtToken; 
        set => cTokenRegistroJwtToken = value; 
    }
    
    public DateTime dTokenCreacion 
    { 
        get => dTokenRegistroFechaCreacion; 
        set => dTokenRegistroFechaCreacion = value; 
    }
    
    public DateTime CreatedAt 
    { 
        get => dTokenRegistroFechaCreacion; 
        set => dTokenRegistroFechaCreacion = value; 
    }

    /// <summary>
    /// Alias para nTokenRegistroId
    /// </summary>
    public int Id 
    { 
        get => nTokenRegistroId; 
        set => nTokenRegistroId = value; 
    }
}

/// <summary>
/// Alias para compatibilidad hacia atrás
/// </summary>
public class Token : TokenRegistro
{
}

}