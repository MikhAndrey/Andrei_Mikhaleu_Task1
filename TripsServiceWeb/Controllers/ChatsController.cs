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

    private readonly IHubContext<ChatHub> _chatHubContext;

    public ChatsController(
        IChatService chatService, 
        ChatCreateCommand chatCreateCommand,
        ChatJoiningCommand chatJoiningCommand,
        IHubContext<ChatHub> chatHubContext)
    {
        _chatService = chatService;
        _chatCreateCommand = chatCreateCommand;
        _chatJoiningCommand = chatJoiningCommand;
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
            ChatDetailsDTO chatDetails = await _chatService.GetById(id);
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
            await _chatHubContext.Clients.All.SendAsync("BroadcastMessage", chatJoinInfo.Message);
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
            ChatMessageDTO result = await _chatService.LeaveChat(dto);
            await _chatHubContext.Clients.All.SendAsync("BroadcastMessage", result);
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
        ChatMessageDTO result = await _chatService.SendMessage(dto);
        await _chatHubContext.Clients.All.SendAsync("BroadcastMessage", result);
        return Ok();
    }

    [HttpGet("participationId/{id}")]
    public async Task<IActionResult> GetParticipationId(int id)
    {
        try
        {
            int currentParticipationId = await _chatService.GetCurrentChatParticipationId(id);
            return Ok(currentParticipationId);
        } catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

