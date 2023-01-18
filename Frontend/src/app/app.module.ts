import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {BASE_PATH} from "../openapi";
import {environment} from "../environments/environment";
import {FormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    {provide: BASE_PATH, useValue: environment.basePath}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
