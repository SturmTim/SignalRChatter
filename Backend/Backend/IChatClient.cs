namespace Backend;

public interface IChatClient
{
    Task NewMessage(string name, string message, string timestamp);
    Task ClientConnected(string name, string timestamp);
    Task ClientDisconnected(string name, string timestamp);
    Task AdminNotification(string message, string name, string timestamp);
    Task NrClientsChanged(int nr);
}