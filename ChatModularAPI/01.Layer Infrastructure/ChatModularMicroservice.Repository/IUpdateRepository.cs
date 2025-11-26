using ChatModularMicroservice.Entities.Models;
namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz base para operaciones de actualización
/// </summary>
/// <typeparam name="T">Tipo de entidad a actualizar</typeparam>
public interface IUpdateRepository<T>
{
    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    /// <param name="item">Entidad con los datos actualizados</param>
    /// <returns>True si se actualizó correctamente</returns>
    Task<bool> Update(T item);
}