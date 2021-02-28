import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AboutMeComponent } from '../aboutme/aboutme.component';

@Component({
  selector: 'header',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class HeaderComponent {

  isExpanded = false;
  constructor(
    public dialog: MatDialog) {

  }

  openAboutMe(title: string): void {
    this.dialog.open(AboutMeComponent, {
      width: 'auto',
      data: {
        title: title
      }
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
