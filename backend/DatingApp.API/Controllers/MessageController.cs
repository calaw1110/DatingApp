using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Params;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
	public class MessageController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMessageRepository _messageRepository;
		private readonly IMapper _mapper;

		public MessageController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
		{
			this._userRepository = userRepository;
			this._messageRepository = messageRepository;
			this._mapper = mapper;
		}

		[HttpPost]
		public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
		{
			var username = User.GetUsername();

			if (username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You cannot send messages to yourself");

			var sender = await _userRepository.GetUserByUsernameAsync(username);
			var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

			if (recipient == null) return NotFound();

			var message = new Message
			{
				Sender = sender,
				Recipient = recipient,
				SenderUsername = sender.UserName,
				RecipientUsername = recipient.UserName,
				Content = createMessageDto.Content
			};

			_messageRepository.AddMessage(message);

			if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

			return BadRequest("Failed to send message");
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
		{
			messageParams.Username = User.GetUsername();

			var messages = await _messageRepository.GetMessagesForUser(messageParams);

			Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

			return messages;
		}
	}
}
