import { Component, OnInit } from '@angular/core';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  constructor(private messageService: MessageService) { }

  ngOnInit() {
  }

  Count(): number {
    return this.messageService.messages.length;
  }

  Clear(): void {
    this.messageService.clear();
  }
  GetMessages(): string[] {
    return this.messageService.messages;
  }
}


