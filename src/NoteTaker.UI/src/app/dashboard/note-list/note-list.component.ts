import { Component, OnInit } from '@angular/core';
import { NoteService } from 'src/app/core/services/Note.service';
import { Note } from 'src/app/core/models/note';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-note-list',
  templateUrl: './note-list.component.html',
  styleUrls: ['./note-list.component.css']
})
export class NoteListComponent implements OnInit {

  notes: Observable<Note[]>;

  constructor(private noteService: NoteService) { }

  ngOnInit() {
    this.notes = this.noteService.getNotes();
  }
}
