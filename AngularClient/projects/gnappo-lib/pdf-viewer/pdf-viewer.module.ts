import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PdfViewerComponent } from './pdf-viewer.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';


@NgModule({
  declarations: [
    PdfViewerComponent
  ],
  imports: [
    CommonModule,
    PdfViewerModule
  ],
  exports:[PdfViewerComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GnappoPdfViewerModule { }
