using Application.DTOs;
using Application.Interfaces;
using Domain.Core;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task<GroupDto> CreateGroupAsync(GroupDto groupDto)
        {
            var users = new List<User>();

            foreach (var username in groupDto.MemberNames)
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user != null)
                {
                    users.Add(user);
                }
            }

            var group = new UserGroups
            {
                Name = groupDto.GroupName
            };

            await _groupRepository.AddGroupAsync(group);

            // Grup üyelikleri ekleme
            foreach (var groupMember in users.Select(user => new UserGroupMember
                     {
                         GroupId = group.Id,
                         UserId = user.Id,
                         JoinDate = DateTime.Now
                     }))
            {
                await _groupRepository.AddGroupMemberAsync(groupMember);
            }

            return groupDto;
        }
    }
}