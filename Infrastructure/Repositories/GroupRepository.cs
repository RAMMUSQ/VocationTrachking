using Application.Interfaces;
using Domain.Core;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{


    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;
       

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<UserGroups> AddGroupAsync(UserGroups group)
        {
            _context.UserGroups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }
        public async Task AddGroupMemberAsync(UserGroupMember groupMember)//yeni
        {
            _context.UserGroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();
        }
    }
}