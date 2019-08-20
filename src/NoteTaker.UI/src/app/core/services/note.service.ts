import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Note } from '../models/note';

@Injectable({
  providedIn: 'root',
})
export class NoteService {
  url: string = environment.apiUrl + 'notes/';

  constructor(private http: HttpClient) { }

  getNotes(): Observable<Note[]> {
    return this.http.get<Note[]>(this.url);
  }

  getNote(id: string): Observable<Note> {
    return this.http.get<Note>(`${this.url}${id}`);
  }

  addNote(note: Note): Observable<Note> {
    return this.http.post<Note>(this.url, note);
  }

  updateTitle(id: string, title: string): Observable<Note> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    return this.http.patch<Note>(`${this.url}${id}`, `[{ "op": "replace", "path": "/title", "value": "${title}" }]`, { headers });
  }

  updateContent(id: string, content: string): Observable<Note> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    return this.http.patch<Note>(`${this.url}${id}`, `[{ "op": "replace", "path": "/content", "value": "${content}" }]`, { headers });
  }
}