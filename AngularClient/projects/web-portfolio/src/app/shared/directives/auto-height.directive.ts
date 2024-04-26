import { AfterViewInit, Directive, ElementRef, HostBinding, Renderer2 } from '@angular/core';

@Directive({
  selector: '[autoHeight]'
})
export class AutoHeightDirective {
  @HostBinding('style.height.px') height!: number;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {
    const windowHeight = window.innerHeight;
    const calculatedheight = `${windowHeight}px`; // can be simply windowheight but you can do the adjuscement here
    this.renderer.setStyle(this.el.nativeElement, 'height', calculatedheight);
  }

  ngOnInit() {
    this.height = window.innerHeight;
  }
}
