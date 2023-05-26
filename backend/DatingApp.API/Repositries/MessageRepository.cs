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
	public class MessageRepository : IMessageRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public MessageRepository(DataContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		public void AddGroup(Group group)
		{
			_context.Groups.Add(group);
		}

		public void AddMessage(Message message)
		{
			_context.Messages.Add(message);
		}

		public void DeleteMessage(Message message)
		{
			_context.Messages.Remove(message);
		}

		public async Task<Connection> GetConnection(string connectionId)
		{
			return await _context.Connections.FindAsync(connectionId);
		}

		public async Task<Group> GetGroupForConnection(string connectionId)
		{
			return await _context.Groups
				.Include(x => x.Connections)
				.Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
				.FirstOrDefaultAsync();
		}

		public async Task<Message> GetMessage(int id)
		{
			return await _context.Messages.FindAsync(id);
		}

		public async Task<Group> GetMessageGroup(string groupName)
		{
			return await _context.Groups
				.Include(x => x.Connections)
				.FirstOrDefaultAsync(x => x.Name == groupName);
		}

		public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
		{
			var query = _context.Messages
				.OrderByDescending(x => x.MessageSent)
				.AsQueryable();

			query = messageParams.Container switch
			{
				// 訊息收件夾
				"Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
										&& u.RecipientDeleted == false),

				// 訊息寄件夾
				"Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
										&& u.SenderDeleted == false),

				// 未讀訊息
				_ => query.Where(u => u.RecipientUsername == messageParams.Username
								&& u.RecipientDeleted == false
								&& u.DateRead == null),
			};

			var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

			// 依分頁查詢參數回傳結果
			return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
		}

		public async Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUsername, string recipientUsername)
		{
			var query = _context.Messages
				//// 發送者的頭像照片信息
				//.Include(u => u.Sender).ThenInclude(p => p.Photos)
				//// 接收者的頭像照片信息
				//.Include(u => u.Recipient).ThenInclude(p => p.Photos)
				.Where(
					// 條件：接收者用戶名等於 currentUsername 且發送者用戶名等於 recipientUsername
					m => m.RecipientUsername == currentUsername && m.SenderUsername == recipientUsername && m.RecipientDeleted == false
					||
					// 或者條件：接收者用戶名等於 recipientUsername 且發送者用戶名等於 currentUsername
					m.RecipientUsername == recipientUsername && m.SenderUsername == currentUsername && m.SenderDeleted == false
				)
				.OrderBy(m => m.MessageSent)
				.AsQueryable();

			var unreadMessages = query.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername).ToList();

			// 如果有未讀消息，標記它們為已讀並保存到資料庫
			if (unreadMessages.Any())
			{
				foreach (var message in unreadMessages)
				{
					message.DateRead = DateTime.UtcNow;
				}
			}

			// Map to MessageDto
			return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
		}

		public void RemoveConnection(Connection connection)
		{
			_context.Connections.Remove(connection);
		}
	}
}