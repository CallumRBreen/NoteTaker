import { Component, OnInit } from '@angular/core';
import { NoteService } from '../core/services/note.service';
import { Note } from '../core/models/note';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {
  selectedNote: Note;
  notes: Note[];

  constructor(private noteService: NoteService) {
    this.noteService.getNotes().subscribe(notes => this.notes = notes);
  }

  ngOnInit() {
  }

  onSelect(note: Note) {
    this.selectedNote = note;
  }

}
