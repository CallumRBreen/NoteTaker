import { HtmlToPlaintextPipe } from './core/pipes/htmlToPlaintext.pipe';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LimitToPipe } from './core/pipes/limitTo.pipe';
import { TimeAgoPipe } from 'time-ago-pipe'

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [LimitToPipe, HtmlToPlaintextPipe, TimeAgoPipe],
  exports: [LimitToPipe, HtmlToPlaintextPipe, TimeAgoPipe]
})
export class SharedModule { }
