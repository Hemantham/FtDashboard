
import { SELECT_DIRECTIVES, SelectComponent } from 'ng2-select';
import { SelectItem } from 'ng2-select/components/select/select-item';
import { Observable } from 'rxjs/Rx'
import { Injectable }     from '@angular/core';


@Injectable()
export class DropDownTools {

  public  setMultipleDropDownAllActive<T>(dropdown: SelectComponent,
        array: Array<T>,
        callback: (selectItem: SelectItem) => T,
        isInitial: boolean): void {

        setTimeout(() => {
                if (dropdown.itemObjects.length > 0) {
                    if (isInitial) {
                        dropdown.active.splice(0);
                        array.splice(0);
                        dropdown.itemObjects.forEach((item) => {
                            dropdown.active.push(item);
                            array.push(callback(item));
                        });
                    }
                } else {
                    array = [];
                    dropdown.active = [];
                }
            },
            500);
    }

  public setMultipleDropDownActive<T>(dropdown: SelectComponent,
      array: Array<T>,
      selected: Array<T>,
      callback: (selectItem: SelectItem) => T,
      getId :(value : T) => string ,
      isInitial: boolean): void {

      setTimeout(() => {
          if (dropdown.itemObjects.length > 0) {
              if (isInitial) {
                  dropdown.active.splice(0);
                  array.splice(0);
                  dropdown.itemObjects.forEach((item) => {
                      if (selected.filter((s) => getId(s) == item.id).length > 0) {
                          dropdown.active.push(item);
                          array.push(callback(item));
                      }

                  });
              }
          } else {
              array = [];
              dropdown.active = [];
          }
      },
          500);
  }
 
  private  alertError(error: any): void {
      alert(error);
  }

  public  loadSingleDropDownData<T>(observable: Observable<T[]>, dropdown: SelectComponent, boundArray: Array<any>, selectedItem: any, mapper: (item: T) => any, callback: () => void): void {
      
      observable
          .subscribe((res: Array<T>) => {
            boundArray = res.map(mapper);
                  
              if (boundArray.length > 0) {
                  selectedItem = boundArray[0];
                  if (dropdown.itemObjects.length > 0)
                      dropdown.active = [dropdown.itemObjects[0]];
                  if (callback) {
                      callback();
                  }
                }

          }, this.alertError);
  }

}