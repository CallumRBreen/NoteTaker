import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LimitToPipe } from './core/pipes/limitTo.pipe';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [LimitToPipe],
  exports: [LimitToPipe]
})
export class SharedModule { }
