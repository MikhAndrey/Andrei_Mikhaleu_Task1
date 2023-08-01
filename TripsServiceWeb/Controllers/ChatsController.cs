using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatsController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(ChatCreateDTO dto)
    {
        if (ModelState.IsValid)
        {
            ChatListDTO addedChat = await _chatService.AddAsync(dto);
            return Ok(addedChat);
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
            await _chatService.AddUser(id);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return Unauthorized();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

