using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.SignalR
{
	[Authorize]
	public class MessageHub : Hub
	{

		private readonly IMapper _mapper;
		private readonly IHubContext<PresenceHub> _presenceHub;
		private readonly IUnitOfWork _uow;

		public MessageHub(IMapper mapper, IHubContext<PresenceHub> presenceHub, IUnitOfWork uow)
		{
			this._mapper = mapper;
			this._presenceHub = presenceHub;
			this._uow = uow;
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var currentUsername = Context.User.GetUsername();
			var otherUser = httpContext.Request.Query["user"];
			var groupName = GetGroupName(currentUsername, otherUser);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
			var group = await AddToGroup(groupName);

			await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

			var messages = await _uow.MessageRepository.GetMessagesThread(currentUsername, otherUser);

			if (_uow.HasChanges()) await _uow.Complete();

			await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var group = await RemoveFromMessageGroup();
			await Clients.Group(group.Name).SendAsync("UpdatedGroup");
			await base.OnDisconnectedAsync(exception);
		}

		/// <summary>
		/// 發送訊息
		/// </summary>
		/// <param name="createMessageDto"></param>
		/// <returns></returns>
		/// <exception cref="HubException"></exception>
		public async Task SendMessage(CreateMessageDto createMessageDto)
		{
			var username = Context.User.GetUsername();

			if (username == createMessageDto.RecipientUsername.ToLower())
				throw new HubException("You cannot send messages to yourself");

			var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
			var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

			if (recipient == null) throw new HubException("Not found user");

			var message = new Message
			{
				Sender = sender,
				Recipient = recipient,
				SenderUsername = sender.UserName,
				RecipientUsername = recipient.UserName,
				Content = createMessageDto.Content
			};

			var groupName = GetGroupName(sender.UserName, recipient.UserName);

			var group = await _uow.MessageRepository.GetMessageGroup(groupName);

			if (group.Connections.Any(x => x.Username == recipient.UserName))
			{
				message.DateRead = DateTime.UtcNow;
			}
			else
			{
				var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
				if (connections != null)
				{
					await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
						new { username = sender.UserName, KnownAs = sender.KnownAs });
				}
			}

			_uow.MessageRepository.AddMessage(message);

			if (await _uow.Complete())
			{
				await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
			}
		}

		private string GetGroupName(string caller, string other)
		{
			var stringCompare = string.CompareOrdinal(caller, other) < 0;
			return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
		}

		private async Task<Group> AddToGroup(string groupName)
		{
			var group = await _uow.MessageRepository.GetMessageGroup(groupName);
			var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

			if (group == null)
			{
				group = new Group { Name = groupName };
				_uow.MessageRepository.AddGroup(group);
			}
			group.Connections.Add(connection);

			if (await _uow.Complete()) return group;

			throw new HubException("Failed to add to group");
		}

		private async Task<Group> RemoveFromMessageGroup()
		{
			var group = await _uow.MessageRepository.GetGroupForConnection(Context.ConnectionId);
			var connection = group.Connections.FirstOrDefault(x => x.ConnetionId == Context.ConnectionId);
			_uow.MessageRepository.RemoveConnection(connection);

			if (await _uow.Complete()) return group;

			throw new HubException("Failed to remove from group");
		}
	}
}