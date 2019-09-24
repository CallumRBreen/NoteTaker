import { DataChanged } from './../../core/models/dataChanged';
import { BehaviorSubject, Subscription } from 'rxjs';
import { NoteService } from './../../core/services/note.service';
import { Component, OnInit, Input } from '@angular/core';
import { Note } from '../../core/models/note';
import * as BalloonEditor from '@ckeditor/ckeditor5-build-balloon';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import { map, filter, debounceTime, groupBy, mergeAll } from 'rxjs/operators';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent implements OnInit {
  public TitleEditor: BalloonEditor = BalloonEditor;
  public ContentEditor: BalloonEditor = BalloonEditor;
  public titleConfig = {
    placeholder: "Title"
  }

  public contentConfig = {
    placeholder: "Content"
  }

  hasTitleChanged: BehaviorSubject<DataChanged>;
  hasContentChanged: BehaviorSubject<DataChanged>;

  hasInitialTitleChangedEventTriggered: boolean;
  hasInitialContentChangedEventTriggered: boolean;

  contentChangeSubscription: Subscription;
  titleChangeSubscription: Subscription;

  @Input() note: Note;

  constructor(private notesService: NoteService) {
    this.hasInitialTitleChangedEventTriggered = false;
    this.hasInitialContentChangedEventTriggered = false;
    this.hasTitleChanged = new BehaviorSubject<DataChanged>({ id: null, changed: false, data: null });
    this.hasContentChanged = new BehaviorSubject<DataChanged>({ id: null, changed: false, data: null });


    this.titleChangeSubscription = this.hasTitleChanged.pipe(filter(x => x.changed === true), groupBy(x => x.id), map(group => group.pipe(debounceTime(2000))), mergeAll()).subscribe(x => {
      this.notesService.updateTitle(x.id, x.data).subscribe(x => { console.log("Note title updated") });
      this.hasTitleChanged.next({ id: this.note.id, changed: false, data: null });
    })

    this.contentChangeSubscription = this.hasContentChanged.pipe(filter(x => x.changed === true), groupBy(x => x.id), map(group => group.pipe(debounceTime(2000))), mergeAll()).subscribe(x => {
      this.notesService.updateContent(x.id, x.data).subscribe(x => { console.log("Note content updated") });
      this.hasContentChanged.next({ id: this.note.id, changed: false, data: null });
    })
  }

  ngOnInit() {
  }

  ngOnChanges() {
    this.hasInitialTitleChangedEventTriggered = false;
    this.hasInitialContentChangedEventTriggered = false;
  }

  ngOnDestroy(): void {
    this.titleChangeSubscription.unsubscribe();
    this.contentChangeSubscription.unsubscribe();
  }

  public titleOnChange({ editor }: ChangeEvent) {
    if (this.hasInitialTitleChangedEventTriggered === false) {
      this.hasInitialTitleChangedEventTriggered = true;
      return;
    }
    this.hasTitleChanged.next({ id: this.note.id, changed: true, data: editor.getData() });
  }

  public contentOnChange({ editor }: ChangeEvent) {
    if (this.hasInitialContentChangedEventTriggered === false) {
      this.hasInitialContentChangedEventTriggered = true;
      return;
    }
    this.hasContentChanged.next({ id: this.note.id, changed: true, data: editor.getData() });
  }
}