import { NoteService } from './../../core/services/note.service';
import { Component, OnInit, Input } from '@angular/core';
import { Note } from '../../core/models/note';
import * as BalloonEditor from '@ckeditor/ckeditor5-build-balloon';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.styl']
})
export class NoteComponent implements OnInit {
  public TitleEditor = BalloonEditor;
  public ContentEditor = BalloonEditor;

  @Input() note: Note;

  constructor(private notesService: NoteService) {
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
  }

  onSubmit() {

  }

}
