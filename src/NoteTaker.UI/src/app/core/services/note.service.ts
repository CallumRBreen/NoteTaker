import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Note } from '../models/note';

@Injectable({
  providedIn: 'root',
})
export class NoteService {
  url: string = environment.apiUrl + 'notes/';

  constructor(private http: HttpClient) { }

  getNotes(): Observable<Note[]> {
    return this.http.get<Note[]>(this.url) ; 
  }

  getNote(id: string): Observable<Note> {
    return this.http.get<Note>(`${this.url}${id}`);
  }
}