using ChatModularMicroservice.Entities.Models;
namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz base para operaciones de eliminación con clave entera
/// </summary>
public interface IDeleteIntRepository
{
    /// <summary>
    /// Elimina una entidad por su ID entero
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteEntero(int id);
}