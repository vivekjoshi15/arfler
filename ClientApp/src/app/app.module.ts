import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { NgxPayPalModule } from 'ngx-paypal';

import { AppComponent } from './app.component';
import { HeaderComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ModalComponent } from './modal/modal.component';
import { ContactComponent } from './contact/contact.component';
import { AboutMeComponent } from './aboutme/aboutme.component';
import { LetterPopupComponent } from './letter-popup/letter-popup.component';
import { DonateComponent } from './donate/donate.component';
import { BuyComponent } from './buy/buy.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    FetchDataComponent,
    ModalComponent,
    ContactComponent,
    AboutMeComponent,
    LetterPopupComponent,
    DonateComponent,
    BuyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule, MaterialModule,
    HttpClientModule,
    FormsModule,
    NgxPayPalModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'AboutMe', component: AboutMeComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'buy/:book/:id', component: BuyComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
