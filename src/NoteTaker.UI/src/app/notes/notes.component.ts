import { Component, OnInit } from '@angular/core';
import { NoteService } from '../core/services/note.service';
import { Note } from '../core/models/note';
import { FormControl } from '@angular/forms';
import { timer, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.styl']
})
export class NotesComponent implements OnInit {

  selectedNote: Note;
  notes: Note[] = [];
  noteSearch: string;
  searchNotesControl = new FormControl('');
  searchNotesSubscription = new Subscription();

  constructor(private noteService: NoteService) {

  }

  ngOnInit() {
    this.noteService.getNotes().subscribe(notes => {
      this.notes = notes;
      this.selectedNote = notes[0];
    });

    this.searchNotesSubscription = this.searchNotesControl.valueChanges.pipe(debounceTime(1000)).subscribe(() => {
      this.noteService.searchNotes(this.searchNotesControl.value).subscribe(notes => {
        this.notes = notes;
        this.selectedNote = notes[0];
      })
    });
  }

  onSelect(note: Note) {
    this.selectedNote = note;
  }

  ngOnDestroy(): void {
    this.searchNotesSubscription.unsubscribe();
  }

  addNote() {
    this.noteService.addNote({ title: '', content: '' }).subscribe(createdNote => {
      this.notes.unshift(createdNote);
      this.notes = [...this.notes]
      this.selectedNote = this.notes[0];
    });
  }
}
