using Microsoft.EntityFrameworkCore;

using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;
using DatingApp.API.Interfaces;
using AutoMapper;
using DatingApp.API.Helper.Params;
using AutoMapper.QueryableExtensions;

namespace DatingApp.API.Repositries
{
	public class MessageRepository : IMessageRepository
	{
		private readonly DatingAppDataContext _context;
		private readonly IMapper _mapper;

		public MessageRepository(DatingAppDataContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}
		public void AddMessage(Message message)
		{
			_context.Messages.Add(message);
		}

		public void DeleteMessage(Message message)
		{
			_context.Messages.Remove(message);
		}

		public async Task<Message> GetMessage(int id)
		{
			return await _context.Messages.FindAsync(id);
		}

		public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
		{
			var query = _context.Messages
				.OrderByDescending(x => x.MessageSent)
				.AsQueryable();

			query = messageParams.Container switch
			{
				// 訊息收件夾
				"Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),

				// 訊息寄件夾
				"Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),

				// 未讀訊息
				_ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null),
			};

			var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

			// 依分頁查詢參數回傳結果
			return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
		}

		public Task<IEnumerable<MessageDto>> GetMessagesThred(int currentUserId, int recipoentId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
