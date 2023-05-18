using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Params;

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

		Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);

		Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUsername, string recipoentUsername);

		Task<bool> SaveAllAsync();

		void AddGroup(Group group);

		void RemoveConnection(Connection connection);

		Task<Connection> GetConnection(string connectionId);

		Task<Group> GetMessageGroup(string groupName);

		Task<Group> GetGroupForConnection(string connectionId);
	}
}
