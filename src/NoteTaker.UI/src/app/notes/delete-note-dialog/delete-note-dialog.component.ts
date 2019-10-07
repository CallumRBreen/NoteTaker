import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-note-dialog',
  templateUrl: './delete-note-dialog.component.html',
  styleUrls: ['./delete-note-dialog.component.css']
})
export class DeleteNoteDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeleteNoteDialogComponent>, @Inject(MAT_DIALOG_DATA) public id: string) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
