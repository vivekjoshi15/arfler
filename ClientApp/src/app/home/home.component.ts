import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ModalComponent } from '../modal/modal.component';
import { LetterPopupComponent } from '../letter-popup/letter-popup.component';
import { DonateComponent } from '../donate/donate.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  price: string;

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    public dialog: MatDialog) {
     
  }

  setDonatePrice(price: string) {
    this.price = price;
  }

  openAboutMe(title: string): void {
    this.dialog.open(LetterPopupComponent, {
      width: '55%',
      data: {
        title: title
      }
    });
  }

  openDialog(id: number, image: string, title: string): void {
    this.dialog.open(ModalComponent, {
      width: '55%',
      data: {
        title: title,
        image: image,
        id: id
      }
    });
  }

  openDonate(): void {
    this.dialog.open(DonateComponent, {
      width: '55%',
      data: {
        price: this.price
      }
    });
  }
}
