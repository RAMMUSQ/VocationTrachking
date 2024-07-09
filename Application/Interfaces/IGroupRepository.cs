using Domain.Core;

namespace Application.Interfaces
{
    public interface IGroupRepository
    {
        Task<UserGroups> AddGroupAsync(UserGroups group);
        Task AddGroupMemberAsync(UserGroupMember groupMember);
       
    }

    
}