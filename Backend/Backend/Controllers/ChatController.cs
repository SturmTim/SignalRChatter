using Backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{

    private readonly ClientRepository _clientRepository;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(ClientRepository clientRepository, IHubContext<ChatHub> hubContext)
    {
        _clientRepository = clientRepository;
        _hubContext = hubContext;
    }
    
    [HttpGet("clients")]
    public IEnumerable<ClientDto> Get()
    {
        _hubContext.Clients.All.SendAsync("AdminNotification", "All clients are requested", "Admin", TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"));
        return _clientRepository.Clients.Values.Select(x => new ClientDto
        {
            Username = x.Username,
            RegisterTime = x.RegisterTime.ToString("HH:mm:ss"),
            LastMessageTime = x.LastMessageTime.ToString("HH:mm:ss")
        }).ToList();
    }
    
    [HttpPost("broadcast")]
    public void Broadcast(string message)
    {
        _hubContext.Clients.All.SendAsync("NewMessage", "Admin", message, TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"));
        _hubContext.Clients.All.SendAsync("AdminNotification", "A broadcast message is sent", "Admin", TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"));
    }
    
}