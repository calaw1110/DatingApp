﻿using AutoMapper;
using DatingApp.API.Interfaces;
using DatingApp.API.Repositries;

namespace DatingApp.API.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public UnitOfWork(DataContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		public IUserRepository UserRepository => new UserRepository(_context, _mapper);

		public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

		public ILikeRepository LikeRepository => new LikesRepository(_context);

		public async Task<bool> Complete()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public bool HasChanges()
		{
			return _context.ChangeTracker.HasChanges();
		}
	}
}