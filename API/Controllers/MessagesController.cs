using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    IMapper mapper) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageDto)
    {
        var username = User.GetUsername();
        if (username == messageDto.RecipientUserName.ToLower())
            return BadRequest("You cannot message yourself.");

        var sender = await userRepository.GetUserByUserNameAsync(username);
        var recipient = await userRepository.GetUserByUserNameAsync(messageDto.RecipientUserName);
        
        if (recipient == null || sender == null) return BadRequest("Either the sender or recipient user are invalid.");
        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageDto.Content,
        };
        
        messageRepository.AddMessage(message);
        if (await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));
        return BadRequest("Failed to create message.");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername();
        var messages = await messageRepository.GetMessagesForUser(messageParams);
        Response.AddPagination(messages);
        return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
    {
        var currentUsername = User.GetUsername();
        return Ok(await messageRepository.GetMessagesThread(currentUsername, username));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUsername();
        var message = await messageRepository.GetMessage(id);
        if (message == null) return NotFound("Cannot find message.");
        if (message.SenderUsername != username && message.RecipientUsername != username) return Forbid();
        
        if (message.SenderUsername == username) message.SenderDeleted = true;
        if (message.RecipientUsername == username) message.RecipientDeleted = true;
        if (message is { SenderDeleted: true, RecipientDeleted: true })
        {
            messageRepository.DeleteMessage(message);
        }

        if (await messageRepository.SaveAllAsync()) return Ok();
        return BadRequest("Failed to delete message.");
    }
}