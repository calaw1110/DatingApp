using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;

namespace DatingApp.API.Interfaces
{
	public interface IMessageRepository
	{
		/// <summary>
		/// 新增訊息
		/// </summary>
		/// <param name="message"></param>
		void AddMessage(Message message);

		/// <summary>
		/// 刪除訊息
		/// </summary>
		/// <param name="message"></param>
		void DeleteMessage(Message message);

		Task<Message> GetMessage(int id);

		Task<PagedList<MessageDto>> GetMessagesForUser();

		Task<IEnumerable<MessageDto>> GetMessagesThred(int currentUserId, int recipoentId);

		Task<bool> SaveAllAsync();
	}
}
