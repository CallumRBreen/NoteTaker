import { DashboardModule } from './dashboard/dashboard.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { HttpClientModule } from '@angular/common/http';
import { NoteService } from './core/services/note.service';
import { MaterialModule } from './material/material.module';

@NgModule({
   declarations: [
      AppComponent,
      NavigationComponent,
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      DashboardModule,
      MaterialModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
