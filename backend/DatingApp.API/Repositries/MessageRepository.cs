using Microsoft.EntityFrameworkCore;

using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;
using DatingApp.API.Interfaces;

namespace DatingApp.API.Repositries
{
	public class MessageRepository : IMessageRepository
	{
		private readonly DatingAppDataContext _context;

		public MessageRepository(DatingAppDataContext context)
		{
			this._context = context;
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

		public Task<PagedList<MessageDto>> GetMessagesForUser()
		{

			throw new NotImplementedException();
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
