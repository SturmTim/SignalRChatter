import {Component, OnInit} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  private hubConnection: HubConnection;

  ngOnInit(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/chat")
      .build();
    this.connect();
  }

  private connect() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
  }


}
