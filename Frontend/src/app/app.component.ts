import {Component, OnDestroy, OnInit} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {Message} from "./Message";
import {ChatService} from "../openapi";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  private hubConnection: HubConnection | null = null;

  isAdmin = false;
  isSignedIn = false;
  message = "";
  username: string = "";
  pwd: string = "";
  messages: Message[] = [];
  clientsNr: number = 0;

  constructor(
    private chatService: ChatService
  ) {
  }


  ngOnInit(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/chat")
      .build();

    this.hubConnection.on('NewMessage', (name: string, message: string, timestamp: string) =>
      this.AddMessage(message, name, timestamp)
    )

    this.hubConnection.on('ClientConnected', (name: string, timestamp: string) =>
      this.AddMessage(`Client ${name} Connected`, "", timestamp)
    )

    this.hubConnection.on('ClientDisconnected', (name: string, timestamp: string) =>
      this.AddMessage(`Client ${name} Disconnected`, "", timestamp)
    )

    this.hubConnection.on('NrClientsChanged', (clientNumber: number) =>
      this.clientsNr = clientNumber
    )

    this.hubConnection.on('AdminNotification', (message: string, name: string, timestamp: string) => {
      if (this.isAdmin) {
        console.log(name)
        this.AddMessage(message, name, timestamp)
      }
    })

    this.connect();
  }

  private AddMessage(message: string, username: string, timestamp: string) {
    if (this.isSignedIn) {
      let newMessage = {message, username, timestamp};
      this.messages.push(newMessage)
    }
  }

  private connect() {
    this.hubConnection!
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
  }

  public get isConnected(): boolean {
    return this.hubConnection!.state === HubConnectionState.Connected;
  }

  public signIn() {
    if (this.isConnected) {
      this.hubConnection!.invoke('signIn', this.username, this.pwd)
        .then(x => {
          this.isAdmin = x
          if (this.isAdmin) {
            this.chatService.chatClientsGet().subscribe(x => {});
          }
        })
        .catch(err => console.error(err));
      this.isSignedIn = true;
    }
  }


  signOut() {
    if (this.isConnected) {
      this.hubConnection!.invoke('signOut')
        .catch(err => console.error(err));
      this.isSignedIn = false;
    }
  }

  sendMessage() {
    if (this.isConnected && this.isSignedIn) {
      this.hubConnection!.invoke('sendMessage',this.message)
        .catch(err => console.error(err));
    }
  }
}
