import { Component, OnInit, ViewChild } from '@angular/core';
import { NoteService } from '../core/services/note.service';
import { Note } from '../core/models/note';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { map, tap, scan, mergeMap, throttleTime } from 'rxjs/operators';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {

  selectedNote: Note;
  notes: Note[] = [];
  notesSubscription: Subscription;

  constructor(private noteService: NoteService) {

  }

  ngOnInit() {
    this.notesSubscription = this.noteService.getNotes().subscribe(notes => this.notes = notes);
  }

  onSelect(note: Note) {
    this.notes.length
    this.selectedNote = note;
  }

  ngOnDestroy(): void {
    this.notesSubscription.unsubscribe();
  }
}
