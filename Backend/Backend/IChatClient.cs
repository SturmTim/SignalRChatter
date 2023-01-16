namespace Backend;

public interface IChatClient
{
    Task NewMessage(string name, string message, string timestamp);
    Task ClientConnected(string name);
    Task ClientDisconnected(string name);
    Task AdminNotification(string message);
    Task NrClientsChanged(int nr);
}