using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository
{
    public interface IChatUsuarioRepository : IDeleteIntRepository, IInsertIntRepository<ChatUsuario>, IUpdateRepository<ChatUsuario>
    {
        Task<ChatUsuario> GetItem(ChatUsuarioFilter filter, ChatUsuarioFilterItemType filterType);
        Task<IEnumerable<ChatUsuario>> GetLstItem(ChatUsuarioFilter filter, ChatUsuarioFilterListType filterType, Utils.Pagination pagination);
    }
}