
import { SELECT_DIRECTIVES, SelectComponent } from 'ng2-select';
import { SelectItem } from 'ng2-select/components/select/select-item';
import { Observable } from 'rxjs/Rx'
import { Injectable }     from '@angular/core';
import 'jspdf'
import 'canvg-browser'
import * as Enumerable from "linq-es2015";

declare var jQuery: any;
declare var jsPDF: any;
declare var canvg: any;

@Injectable()
export class PrintTools {

    ///elements need to be a jQuery selection
    public printHeadersAndSvg(elements: any) {

        debugger;

        let doc = new jsPDF("landscape", "mm", "a4");

        doc.setFontSize(12);

        doc.setTextColor(50, 54, 57);

        let pritedList =
            jQuery('[attr\\.data-print]')
                .map((i: number, val: any) => {
                    let chartname = jQuery(val).attr('attr.data-print').split('-')[1];

                    return {
                        attribute: chartname,
                        text: jQuery(val).text().trim()
                    }
                })
                .toArray()
                .sort((left: string, right: string): number => (left < right) ? -1 : (left > right) ? 1 : 0);

        
        let promises = elements
            .map((i: number, element: any) => {

                let $element = jQuery(element);

                canvg(jQuery('#img-out')[0],
                    jQuery("<p>").append($element.clone()).html()
                    ,
                    {
                        log: false,
                        ignoreMouse: true
                    });

                return new Promise((resolve, reject) => {

                   if (i > 0) {
                        doc.addPage();
                    }
                    let lastk = 0;
                    pritedList.forEach((prntGroup: any, k: number) => {
                      
                        lastk = k;
                        doc.text(20, 20 + (k * 8), `${prntGroup.attribute} : ${prntGroup.text}`);
                       
                    });

                    doc.text(20, 20 + (++lastk * 8), `Chart Name : ${$element.closest('.panel').find('[attr\\.data-print-name]').text()}`);


                    doc.addImage(jQuery('#img-out')[0].toDataURL('image/jpeg'), 'JPEG', 20, 55);
                    resolve();
                   
                });
            });

        //wait for images to be written
       
        Promise.all(promises)
            .then(() => {
                console.log(3);
                doc.save('chart-file.pdf');
            }
            );
    }
}