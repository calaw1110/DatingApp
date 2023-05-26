using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Params;
using DatingApp.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repositries
{
    public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public UserRepository()
		{
		}

		public UserRepository(DataContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		public async Task<MemberDto> GetMemberAsync(string username)
		{
			return await _context.Users
				.Where(x => x.UserName == username)
				.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}

		public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
		{
			var query = _context.Users.AsQueryable();
			query = query.Where(w => w.UserName != userParams.CurrentUserName);
			query = query.Where(w => w.Gender == userParams.Gender);

			var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
			var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
			query = query.Where(w => w.DateOfBirth >= minDob && w.DateOfBirth <= maxDob);

			query = userParams.OrderBy switch
			{
				"created" => query.OrderByDescending(o => o.Created),
				_ => query.OrderByDescending(o => o.LastActive)
			};

			return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);



			//var query = _context.Users
			//	.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
			//	// 讓ef對此次查詢結果不追蹤，提高查詢的性能，節省記憶體
			//	.AsNoTracking();
			//return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
		}

		public async Task<AppUser> GetUserByIdAsync(int id)
		{
			return await _context.Users
				.Include(p => p.Photos)
				.SingleOrDefaultAsync(x => x.Id == id);
		}

		public async Task<AppUser> GetUserByUsernameAsync(string username)
		{
			return await _context.Users
				.Include(p => p.Photos)
				.SingleOrDefaultAsync(x => x.UserName == username);
		}

		public async Task<string> GetUserGender(string username)
		{
			return await _context.Users.Where(x=>x.UserName ==username).Select(x=>x.Gender).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<AppUser>> GetUsersAsync()
		{
			return await _context.Users
				.Include(p => p.Photos)
				.ToListAsync();
		}

		public void Update(AppUser user)
		{
			_context.Entry(user).State = EntityState.Modified;
		}
	}
}
