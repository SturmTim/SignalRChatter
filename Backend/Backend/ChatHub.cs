using Microsoft.AspNetCore.SignalR;

namespace Backend;

public class ChatHub : Hub<IChatClient>
{
    private readonly ClientRepository _chatRepository;

    public ChatHub(ClientRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public bool SignIn(string username, string pwd)
    {
        if (pwd.Length < 5)
        {
            throw new Exception("Password too short");
        }

        _chatRepository.Clients.Add(Context.ConnectionId, new Client
        {
            Username = username,
            RegisterTime = TimeOnly.FromDateTime(DateTime.Now),
            LastMessageTime = new TimeOnly(0,0,0)
        });

        Clients.All.ClientConnected(username, TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"));
        SendCurrentClientNumber();

        if (username.StartsWith("Admin"))
        {
            Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            return true;
        }

        return false;

    }
    
    public void SignOut()
    {
        Clients.All.ClientDisconnected(_chatRepository.Clients[Context.ConnectionId].Username, TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"));
        _chatRepository.Clients.Remove(Context.ConnectionId);
        SendCurrentClientNumber();
    }
    
    public void SendMessage(string message)
    {
        var client = _chatRepository.Clients[Context.ConnectionId];
        client.LastMessageTime = TimeOnly.FromDateTime(DateTime.Now);
        Clients.All.NewMessage(client.Username, message, TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"));
    }

    public void SendCurrentClientNumber()
    {
        Clients.Group("Admins").NrClientsChanged(_chatRepository.Clients.Count);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        SignOut();
        return base.OnDisconnectedAsync(exception);
    }
}