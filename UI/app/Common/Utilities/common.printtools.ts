
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

        let enumPrintList = Enumerable.asEnumerable<any>(pritedList)
            .GroupBy(
            p => p.attribute).
            ToArray();


        let promises = elements
            .map((i: number, element: any) => {
                
              
                
                canvg(jQuery('#img-out')[0],
                            element.outerHTML,
                            {
                                log: false,
                                ignoreMouse: true
                            }
                );


                return new Promise((resolve, reject) => {
                    var j = i;

                    if (j > 0) {
                        doc.addPage();
                    }

                    enumPrintList.forEach((prntGroup: any, k: number) => {
                       
                        if (prntGroup.key !== 'Chart Name') {
                            doc.text(20, 20 + (k * 8), `${prntGroup.key} : ${prntGroup[0].text}`);
                        } else {
                            doc.text(20, 20 + (k * 8), `${prntGroup.key} : ${prntGroup[j].text}`);
                        }
                    });

                    this.printCanvas(jQuery('#img-out')[0],doc, 55);
                    resolve();

                    //html2canvas(element,
                    //{
                    //    onrendered: (canvas: any) => {

                    //        if (j > 0) {
                    //            doc.addPage();
                    //        }

                    //        console.log('lasti + 3' + (lasti + 3));
                    //        console.log('j' + j);
                    //        this.printCanvas(canvas, doc, j === 0 ? (lasti + 3) * 10 : 20);
                    //        resolve();
                    //    }
                    //});
                });
            });

        //wait for images to be written
        Promise.all(promises)
            .then(() => doc.save('chart-file.pdf'));
    }

    public printCanvas(canvas: any, doc: any, row: number) {

        let imgData = canvas.toDataURL('image/png');
        ////var doc = new jsPDF('p', 'mm');
        doc.addImage(imgData, 'PNG', 20, row);
    }
}