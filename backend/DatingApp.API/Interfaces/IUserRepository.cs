using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Params;

namespace DatingApp.API.Interfaces
{
    public interface IUserRepository
	{
		void Update(AppUser user);

		Task<bool> SaveAllAsync();

		Task<IEnumerable<AppUser>> GetUsersAsync();

		Task<AppUser> GetUserByIdAsync(int id);

		Task<AppUser> GetUserByUsernameAsync(string username);

		Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

		Task<MemberDto> GetMemberAsync(string username);
	}
}
