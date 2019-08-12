import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { NoteService } from 'src/app/core/services/note.service';
import { Note } from 'src/app/core/models/note';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent implements OnInit {
  @Input() note: Note;
  constructor(private route: ActivatedRoute, private noteService: NoteService) {
  }

  ngOnInit() {
  }

}
