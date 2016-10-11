import { Directive, ElementRef, Input, Renderer } from '@angular/core';
declare var jQuery: any;

@Directive({ selector: '[ng-dropdown-panel]' })
export class DropDownPanelDirective {
    constructor(el: ElementRef, renderer: Renderer) {
        //renderer.setElementStyle(el.nativeElement, 'backgroundColor', 'yellow');

        renderer.listen(el.nativeElement,
            'click',
            (event: any) => {
                var $this = jQuery(el.nativeElement).parent();
                if (!$this.hasClass('panel-collapsed')) {
                    $this.parents('.panel').find('.panel-body').slideUp();
                    $this.addClass('panel-collapsed');
                    $this.find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
                } else {
                    $this.parents('.panel').find('.panel-body').slideDown();
                    $this.removeClass('panel-collapsed');
                    $this.find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
                }
            });

    }
}