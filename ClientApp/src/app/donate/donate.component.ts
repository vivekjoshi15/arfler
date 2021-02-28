import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IPayPalConfig, ICreateOrderRequest } from 'ngx-paypal';

interface DialogData {
  price: string;
}

@Component({
  selector: 'app-donate',
  templateUrl: './donate.component.html',
  styleUrls: ['./donate.component.css']
})
export class DonateComponent implements OnInit {

  showSuccess = false;
  showCancel = false;
  showError = false;
  public payPalConfig?: IPayPalConfig;

  constructor(
    public dialogRef: MatDialogRef<DonateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.payPalConfig = this.initConfig();
  }

  private initConfig(): IPayPalConfig {
    const payPalConfig: IPayPalConfig = {
      currency: 'USD',
      clientId: 'AU3Qb9gGtEOzodxyfpEEkED0gf1OQMj9cLjIFfyOJulATj9SJm0r23qTKEx-XTn04yc_fN8EF7qMZuLY',
      createOrderOnClient: (data1) => <ICreateOrderRequest>{
        intent: 'CAPTURE',
        purchase_units: [{
          amount: {
            currency_code: 'USD',
            value: this.data.price,
            breakdown: {
              item_total: {
                currency_code: 'USD',
                value: this.data.price
              }
            }
          },
          items: [{
            name: 'Donation',
            quantity: '1',
            category: 'DIGITAL_GOODS',
            unit_amount: {
              currency_code: 'USD',
              value: this.data.price,
            },
          }]
        }]
      },
      advanced: {
        commit: 'true'
      },
      style: {
        label: 'paypal',
        layout: 'vertical'
      },
      onApprove: (data, actions) => {
        console.log('onApprove - transaction was approved, but not authorized', data, actions);
        actions.order.get().then(details => {
          console.log('onApprove - you can get full order details inside onApprove: ', details);
        });

      },
      onClientAuthorization: (data) => {
        console.log('onClientAuthorization - you should probably inform your server about completed transaction at this point', data);
        this.showSuccess = true;
      },
      onCancel: (data, actions) => {
        console.log('OnCancel', data, actions);
        this.showCancel = true;

      },
      onError: err => {
        console.log('OnError', err);
        this.showError = true;
      },
      onClick: (data, actions) => {
        console.log('onClick', data, actions);
        this.resetStatus();
      },
    };
    return payPalConfig;
  }

  private resetStatus() {
    console.log('reset');
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
    //console.log(this.data)
  }
}
