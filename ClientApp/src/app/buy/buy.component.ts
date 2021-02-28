import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { IPayPalConfig, ICreateOrderRequest } from 'ngx-paypal';

@Component({
  selector: 'app-buy',
  templateUrl: './buy.component.html',
  styleUrls: ['./buy.component.css']
})
export class BuyComponent implements OnInit {

  id = 0;
  qty = 1;
  price = '25.0';
  name = ""; 
  type = ""; 
  showSuccess = false;
  showCancel = false;
  showError = false;
  public payPalConfig?: IPayPalConfig;

  constructor(
    private router: Router,
    private actRoute: ActivatedRoute) {
    this.id = this.actRoute.snapshot.params.id;
    this.type = 'paypal';
    this.name = "Twelve Virtues";

    if (this.id === 1) {
      this.name = "Twelve Virtues";
    }
    else if (this.id === 2) {
      this.name = "B.E.T.A.S";
    }

    this.price = (this.qty * Number(this.price)).toString();
    this.payPalConfig =this.initConfig(this.qty, this.name, this.price);
  }

  ngOnInit(): void {
  }

  selectMethod(type: string): void {
    this.type = type;
  }

  private initConfig(qty, name, price): IPayPalConfig {
    const payPalConfig: IPayPalConfig = {
      currency: 'USD',
      clientId: 'AU3Qb9gGtEOzodxyfpEEkED0gf1OQMj9cLjIFfyOJulATj9SJm0r23qTKEx-XTn04yc_fN8EF7qMZuLY',
      createOrderOnClient: (data) => <ICreateOrderRequest>{
        intent: 'CAPTURE',
        purchase_units: [{
          amount: {
            currency_code: 'USD',
            value: price,
            breakdown: {
              item_total: {
                currency_code: 'USD',
                value: price
              }
            }
          },
          items: [{
            name: name,
            quantity: qty,
            category: 'DIGITAL_GOODS',
            unit_amount: {
              currency_code: 'USD',
              value: '25.0',
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

  changeQty() {
    this.price = (this.qty * 25).toString()+'.0';

    
    try {
      this.payPalConfig = this.initConfig(this.qty, this.name, this.price);
    }
    catch (err) {
      //console.log(err);
    }
  }

}
