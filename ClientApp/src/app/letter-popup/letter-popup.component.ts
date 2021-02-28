import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AboutMeComponent } from '../aboutme/aboutme.component';

interface DialogData {
  title: string;
}

@Component({
  selector: 'app-letter-popup',
  templateUrl: './letter-popup.component.html',
  styleUrls: ['./letter-popup.component.css']
})
export class LetterPopupComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<AboutMeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public dialog: MatDialog) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
    //console.log(this.data)
  }

  openAboutMe(): void {
    this.dialog.open(AboutMeComponent, {
      width: '55%',
      data: {
      }
    });
    this.dialogRef.close();
  }
}
