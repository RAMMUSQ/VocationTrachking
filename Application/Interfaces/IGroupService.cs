using Application.DTOs;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDto> CreateGroupAsync(GroupDto groupDto);
    }

    /*public class GroupService : IGroupService
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
                Name = groupDto.GroupName,
                Members = users
            };

            await _groupRepository.AddGroupAsync(group);

            return groupDto;
        }
    }*/
}