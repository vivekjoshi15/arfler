import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContactComponent } from './contact/contact.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  result: string;

  constructor(public dialog: MatDialog) {
  }

  openContact(): void {
    const dialogRef = this.dialog.open(ContactComponent, {
      width: '320px',
      data: { }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.result = result;
    });
  }
}
