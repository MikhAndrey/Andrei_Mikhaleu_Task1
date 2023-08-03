using Andrei_Mikhaleu_Task1.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.Commands.Chats;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;

    private readonly ChatCreateCommand _chatCreateCommand;
    private readonly ChatJoiningCommand _chatJoiningCommand;
    private readonly ChatDeleteCommand _chatDeleteCommand;

    private readonly IHubContext<ChatHub> _chatHubContext;

    public ChatsController(
        IChatService chatService, 
        ChatCreateCommand chatCreateCommand,
        ChatJoiningCommand chatJoiningCommand,
        ChatDeleteCommand chatDeleteCommand,
        IHubContext<ChatHub> chatHubContext)
    {
        _chatService = chatService;
        _chatCreateCommand = chatCreateCommand;
        _chatJoiningCommand = chatJoiningCommand;
        _chatDeleteCommand = chatDeleteCommand;
        _chatHubContext = chatHubContext;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(ChatCreateDTO dto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                ChatListDTO addedChat = await _chatCreateCommand.ExecuteAsync(dto);
                return Ok(addedChat);
            } catch (DbOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest(ModelState);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _chatDeleteCommand.ExecuteAsync(id);
            return Ok();
        } catch (DbOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("index")]
    public IActionResult Index()
    {
        IEnumerable<ChatListDTO> chats = _chatService.GetAll();
        return Ok(chats);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            ChatDetailsDTO chatDetails = await _chatService.GetByIdAsync(id);
            return Ok(chatDetails);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("join/{id}")]
    public async Task<IActionResult> AddUser(int id)
    {
        try
        {
            ChatJoinDTO chatJoinInfo = await _chatJoiningCommand.ExecuteAsync(id);
            await _chatHubContext.Clients.Group(id.ToString()).SendAsync("BroadcastMessage", chatJoinInfo.Message);
            return Ok(chatJoinInfo.ChatParticipationId);
        }
        catch (ArgumentNullException)
        {
            return Unauthorized();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DbOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("leave")]
    public async Task<IActionResult> LeaveChat(ChatLeaveDTO dto)
    {
        try
        {
            ChatMessageDTO result = await _chatService.LeaveChatAsync(dto);
            await _chatHubContext.Clients.Group(dto.ChatId.ToString()).SendAsync("BroadcastMessage", result);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage(ChatSendMessageDTO dto)
    {
        ChatMessageDTO result = await _chatService.SendMessageAsync(dto);
        await _chatHubContext.Clients.Group(dto.ChatId.ToString()).SendAsync("BroadcastMessage", result);
        return Ok();
    }

    [HttpGet("participationId/{id}")]
    public async Task<IActionResult> GetParticipationId(int id)
    {
        try
        {
            int currentParticipationId = await _chatService.GetCurrentChatParticipationIdAsync(id);
            return Ok(currentParticipationId);
        } catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

