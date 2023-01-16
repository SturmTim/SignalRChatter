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
            RegisterTime = new TimeOnly(),
            LastMessageTime = new TimeOnly(0,0,0)
        });

        Clients.All.ClientConnected(username);
        
        SendCurrentClientNumber();

        if (username.StartsWith("Admin"))
        {
            return true;
        }

        return false;

    }
    
    public void SignOut()
    {
        Clients.All.ClientDisconnected(_chatRepository.Clients[Context.ConnectionId].Username);
        _chatRepository.Clients.Remove(Context.ConnectionId);
        SendCurrentClientNumber();
    }
    
    public void SendMessage(string message)
    {
        var client = _chatRepository.Clients[Context.ConnectionId];
        client.LastMessageTime = new TimeOnly();
        Clients.All.NewMessage(client.Username, message, new TimeOnly().ToString("HH:mm:ss"));
    }
    
    public void AdminNotification(string message)
    {
        Clients.All.AdminNotification(message);
    }
    
    public void SendCurrentClientNumber()
    {
        Clients.All.NrClientsChanged(_chatRepository.Clients.Count);
    }
}