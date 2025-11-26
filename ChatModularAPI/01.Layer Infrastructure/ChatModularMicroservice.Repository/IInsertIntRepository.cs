using ChatModularMicroservice.Entities.Models;
namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz base para operaciones de inserción que retornan ID entero
/// </summary>
/// <typeparam name="T">Tipo de entidad a insertar</typeparam>
public interface IInsertIntRepository<T>
{
    /// <summary>
    /// Inserta una nueva entidad
    /// </summary>
    /// <param name="item">Entidad a insertar</param>
    /// <returns>ID generado de la entidad insertada</returns>
    Task<int> Insert(T item);
}