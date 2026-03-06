import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Table, TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from "@angular/material/button";
import { MatExpansionModule } from '@angular/material/expansion';
import { MatInputModule} from '@angular/material/input';
import { MatFormFieldModule} from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";
import { MenuItem } from 'primeng/api';
import { ContextMenu, ContextMenuModule } from 'primeng/contextmenu';
import { SortEvent } from 'primeng/api';
import { TagModule} from 'primeng/tag';


@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [ButtonModule, TableModule, MatButtonModule, CommonModule, MatExpansionModule,
    MatInputModule, MatFormFieldModule, MatIconModule, FormsModule, ContextMenuModule, ContextMenu, TagModule],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css'
})

export class GridComponent implements OnInit {
//export class TablePaginatorprogrammaticDemo implements OnInit {
    // private customerService = inject(CustomerService);
    // customers!: Customer[];

    constructor(private http: HttpClient, private changeDetectorRef: ChangeDetectorRef) { }
    @ViewChild('dt') table!: Table;
    @ViewChild('cm') cm!: ContextMenu;

    darkMode: string = '';
    buttonClass: string = '';
    drvMgrFilter: string = '';
    rowData: any[] = [];
    row: any[] = [];
    first: number = 0;
    rows: number = 10;
    loading = false;
    isPanelOpen: boolean = true;
    initialData: any[] = [];
    selectedUnit: any | null = null;
    selectedMenuItem: any | null = null;
    contextMenuItems: MenuItem[] = [];
    rightClickMenuPositionX: number = 0;
    rightClickMenuPositionY: number = 0;
    showMenu: boolean = false;
    columns: { field: string; header: string }[] = [];
    isPageToggled: boolean = false;
    // op!: MenuItem[];

    ngOnInit() {
      // this.darkMode = this.getCssVariable('color-scheme');
  
        // this.customerService.getCustomersLarge().then((customers) => {
        //     this.customers = customers;
        // });

    // console.log(this.darkMode);
    //     if (this.darkMode == 'dark') {
    //       this.buttonClass = 'example-button-row';
    //     }
    //     else {
    //       this.buttonClass = 'example-button-row-dark';
    //     }
        this.loading = true;
        this.loadData();
        // this.op = [
        //     { label: 'View', icon: 'pi pi-fw pi-search', command: () => this.viewProduct(this.selectedUnit) },
        //     { label: 'Delete', icon: 'pi pi-fw pi-times', command: () => this.deleteProduct(this.selectedUnit) }
        // ];
    }

    onHide() {
        this.selectedUnit = undefined;
    }

    onRowRightClick(event: any) {
      // this.cm.hide();
    console.log(event);
    this.selectedUnit = event.data;
    console.log(this.selectedUnit);
    this.buildContextMenuItems(event);
  }

  onRowSelected(event: any) {
 console.log('here');
    this.selectedUnit = event.data;
    console.log(this.selectedUnit);
    this.buildContextMenuItems(event);
  }

  onRowUnselect(event: any) {
 console.log(event);
    this.selectedUnit = undefined;
    console.log(this.selectedUnit);
    this.contextMenuItems = [];
  }

 buildContextMenuItems(event: any) {
    if (!this.selectedUnit) return;
console.log(this.selectedUnit);
this.showMenu = false;
 setTimeout(() => {
           //this.http.get<any[]>('https://localhost:7270/FleetServices/GetOpts/' + this.selectedUnit.unit + '/' + this.selectedUnit.order)
          //this.http.get<any[]>('https://sc.aos.biz/FleetServicesWebAPI/FleetServices/FleetServices')
          //const url = 'https://localhost:7270/FleetServices/GetOpts';
          const url = 'https://sc.aos.biz/FleetServicesWebAPI/FleetServices/GetOpts';
          //const body = { title: 'Angular POST Request Example' };
          const jsonStr = JSON.stringify(this.selectedUnit.fields);
          const body = JSON.parse(jsonStr);

    console.log(body);
          this.http.post<any[]>(url, body) //.subscribe(data => {
// this.postId = data.id;
// });
          .subscribe({
            next: (data) => {
                if (data.length > 0) {
        console.log(data);
                  //this.contextMenuItems = data.map(item => { label: item.text, command: );
                  var m: MenuItem[] = [];
                  var idx = 1;
                  //context menu header
                  var headerItem: MenuItem = {
                    label: 'Select Option',
                    index: '0',
                  };
                  m.push(headerItem);
                  var separator: MenuItem = {
                    separator: true
                  };
                  m.push(separator);
                  data.forEach(c => {

                    const item: MenuItem = {
                      label: c.text,
                      index: idx.toString(),
                      //routerLink: [c.link]
                    };

                    m.push(item);
                    idx++;
                  });
                  this.contextMenuItems = m;
                  //this.selectedMenuItem = m;
this.rightClickMenuPositionX = event.originalEvent.clientX;
    this.rightClickMenuPositionY = event.originalEvent.clientY;
        console.log(this.rightClickMenuPositionX);
        //this.cm.show(event);
        this.showMenu = true;
                  this.changeDetectorRef.detectChanges();
                  
                }
                else
                {
                  this.contextMenuItems = [];
                  this.showMenu = false;
                }
                // this.gridApi.setGridOption("loading", false);
                // this.gridApi.setGridOption("rowData", this.rowData);
                // this.gridApi.refreshCells();
                // this.gridApi.sizeColumnsToFit();
            },
            error: (err) => {
console.log(err);
                // this.gridApi.setGridOption("loading", false);
                // this.gridApi.refreshCells();
                // this.gridApi.sizeColumnsToFit();
            }
          });
        });
    // if (this.selectedRow.status === 'active') {
    //   this.contextMenuItems = [
    //     {
    //       label: 'Deactivate',
    //       icon: 'pi pi-ban',
    //       command: () => this.deactivateRow(this.selectedRow!)
    //     },
    //     {
    //       label: 'Edit',
    //       icon: 'pi pi-pencil',
    //       command: () => this.editRow(this.selectedRow!)
    //     }
    //   ];
    // } else {
    //   this.contextMenuItems = [
    //     {
    //       label: 'Activate',
    //       icon: 'pi pi-check',
    //       command: () => this.activateRow(this.selectedRow!)
    //     },
    //     {
    //       label: 'Delete',
    //       icon: 'pi pi-trash',
    //       command: () => this.deleteRow(this.selectedRow!)
    //     }
    //   ];
    // }
  }

  getRightClickMenuStyle() {
    return {
      height: 'auto',
      maxHeight: '150px',
      left: `${this.rightClickMenuPositionX}px`,
      top: `${this.rightClickMenuPositionY}px`
    }
    //return 'p-contextmenu-class';
  }



    loadData() {
      this.darkMode = this.getCssVariable('color-scheme');
    console.log(this.darkMode);
        //this.http.get<any[]>('https://sc.aos.biz/EDIWebAPI/EDI/FleetServices')   //https://sc.aos.biz/EDIWebAPI/EDI/EDIMain
        //this.http.get<any[]>('https://localhost:44360/EDI/FleetServices')
        setTimeout(() => {
           //this.http.get<any[]>('https://localhost:7270/FleetServices/FleetServices')
          this.http.get<any[]>('https://sc.aos.biz/FleetServicesWebAPI/FleetServices/FleetServices')
          .subscribe({
            next: (data) => {
                if (data.length > 0) {
                // If no data returned, generate random data for demonstration
                 this.initialData = data;
console.log(this.initialData);
this.rowData = data;

      this.columns = Object.keys(this.rowData[0].fields).map(key => ({
        field: key,
        header: this.capitalize(key)
      }));

                  // var tmpData = this.getRandomData(data.length);
                  // this.rowData = data.map((row, i) => ({ ...row, ...tmpData[i] }));
                 
  // console.log(this.rowData);
                  this.changeDetectorRef.detectChanges();
                }
                else
                  this.rowData = [];
                this.loading = false;
                // this.gridApi.setGridOption("loading", false);
                // this.gridApi.setGridOption("rowData", this.rowData);
                // this.gridApi.refreshCells();
                // this.gridApi.sizeColumnsToFit();
            },
            error: (err) => {
              this.loading = false;
                // this.gridApi.setGridOption("loading", false);
                // this.gridApi.refreshCells();
                // this.gridApi.sizeColumnsToFit();
            }
          });
        });
    }

    private capitalize(text: string): string {
    return text.charAt(0).toUpperCase() + text.slice(1);
  }


    next() {
        this.first = this.first + this.rows;
    }

    prev() {
        this.first = this.first - this.rows;
    }

    reset() {
        this.first = 0;
    }

    // pageChange(event) {
    //     this.first = event.first;
    //     this.rows = event.rows;
    // }

    pageChange(event: any) {
        this.first = event.first;
        this.rows = event.rows;
    }

    isLastPage(): boolean {
        //return this.customers ? this.first + this.rows >= this.customers.length : true;
        return true;
    }

    isFirstPage(): boolean {
        //return this.customers ? this.first === 0 : true;
        return true;
    }

    getHeaderClass(header: string) : any {

      var style= '';
      switch (header) {
        case 'Opt' :
        case 'Stat' :
        case 'Cnd' :
        case 'Dest' :
        case 'ETA' :
        case 'PTA' :
        case 'Cont' :
        case '7Day':
        case 'RDO':
        case 'VHOS':
          if (this.darkMode == 'dark')
            style = 'highlight-header-pink-dark'; //{ color: 'Fuchsia'};
          else
            style = 'highlight-header-pink'; //{ color: 'Fuchsia'};
          break;
        case 'Unit':
        case 'Order':
        case 'Trlr1':
        case 'Driver':
        case 'Home':
          if (this.darkMode == 'dark')
            style = 'highlight-header-yellow-dark'; //{ color: 'Yellow'};
          else
            style = 'highlight-header-yellow'; //{ color: '#ffbf00' };
          break;
        default:
          if (this.darkMode == 'dark')
            style = ''; //{ color: 'White'};
      }      
      return style;
    }

     
    getCellStyle(row: any, fldName: string, rowIdx: number) : any {
    // if (this.darkMode) {
    //   if (params.value >= '11:00') {
    //     return { color: 'Red' };
    //   }
    // }
      var style= {color: 'Black'};
      switch (fldName) {
        case 'Order':
          var tmp: string[] = ['0', '1', '1', '0', '0', '1', '0', '1', '0', '1']; 
          var idx = rowIdx? rowIdx : 0;
          if (tmp[idx%10] == '1')
          {
            if (this.darkMode=='dark')
              style = { color: 'Aqua'};
            else  
              style = { color: '#06A1BC'};
          }
          else
          {
            if (this.darkMode == 'dark')
            {         
              style = { color: 'White'};
            }
            else 
              style = { color: 'Black'};
          }
          break;
        case 'Dest':
          var tmp: string[] = ['0', '0', '0', '0', '0', '0', '0', '1', '0', '1']; 
          var idx = rowIdx? rowIdx : 0;
          if (tmp[idx%10] == '1')
          {
            if (this.darkMode =='dark')
              style = { color: 'Yellow'};
            else  
              style = { color: '#B7A106'};
          }
          else
          {
            if (this.darkMode == 'dark')
            {         
              style = { color: 'White'};
            }
            else 
              style = { color: 'Black'};
          }
          break;
        case 'Projected':
          var tmp: string[] = ['0', '0', '0', '1', '0', '0', '0', '0', '1', '1']; 
          var idx = rowIdx? rowIdx : 0;
          if (tmp[idx%10] == '0')
          {
            if (this.darkMode == 'dark')
              style = { color: 'White'};
            else  
              style = { color: 'Black'};
          }
          else
          {
            if (this.darkMode == 'dark')
            {         
              style = { color: 'Red'};
            }
            else 
              style = { color: '#E20405'};
          }
          break;
        case 'VHOS11Hrs':
           if (this.darkMode == 'dark')
          {
            if (row.VHOS11Hrs === '00:00') 
              style = { color: 'Red'};
            else
              style = { color: '#3ED006'};
          }
          else {
            if (row.VHOS11Hrs === '00:00') 
              style = { color: '#E20405'};
            else
              style = { color: '#018E1A'};  //green
          }
          break;
        case 'VHOS70Hrs':
          if (this.darkMode == 'dark')
          {
            if (row.VHOS70Hrs === '00:00') 
              style = { color: 'Red'};
            else
              style = { color: '#3ED006'};   //green
          }
          else {
            if (row.VHOS70Hrs === '00:00') 
              style = { color: '#E20405'};
            else
              style = { color: '#018E1A'};  
          }
          break;
        case 'VHOS14Hrs':
          if (this.darkMode == 'dark')
          {
            if (row.VHOS14Hrs === '00:00') 
              style = { color: 'Yellow'};
            else
              style = { color: '#3ED006'};  //green
          }
          else {
            if (row.VHOS14Hrs === '00:00') 
              style = { color: '#D08906'};
            else
              style = { color: '#018E1A'};
          }
          break;
        case 'RDO':
          if (this.darkMode == 'dark')
          {
            if (row.RDO > '6') 
              style = { color: '#3ED006'}; //3ED006
            else
              style = { color: 'Yellow'}; //D7B304
          }
          else {
            if (row.RDO > '6') 
              style = { color: '#018E1A'};
            else
              style = { color: '#B7A106'}; ///B7A106
          }
          break;
      }
      return style;
    }


    getCellClass(colorCode: string) : any {

      var theme = this.darkMode == 'dark' ? 'dark' : 'light';
      return colorCode == ''? '' : colorCode.endsWith('Y')? '' : colorCode + '-' + theme;
    }

    isReverseImage(val: string) : boolean {
      if (!val || val == '' || val.length < 2)
        return false;
      else if (val.endsWith('Y')) 
        return true;
      return false;
    }
    
    getSeverity(colorCode: string) : any {
      if (!colorCode || colorCode == '' || colorCode.length < 2)
        return '';
      var color = colorCode.substring(0, 1)
      switch (color) {
        case 'G':
          return 'success';
        case 'B':
          return 'info';
        case 'Y':
          return 'warn';
        case 'R':
          return 'danger';
        case 'W':
          return 'contrast';
        default:
            return '';
      }
    }


    showHideColumn(columnIndex: number) : any {
      var result;
      if (columnIndex < 25)
        result =  this.isPageToggled;
      else
        result = !this.isPageToggled;
      return result;
    }

  
  //   getCellClass(row: any, fldName: string, rowIdx: number) : any {
  //     var cellClass= '';
  //     // if (row.trim() == '')
  //     //   return '';
  //     switch (fldName) {
  //       case 'Stat':
  //         var tmp: string[] = ['0', '1', '1', '0', '0', '0', '1', '0', '1', '0']; 
  //         var idx = rowIdx? rowIdx : 0;
  //         if (tmp[idx%10] == '1')
  //         {
  //           if (this.darkMode == 'dark')
  //             cellClass = 'highlight-cell-white-dark';
  //           else  
  //             cellClass = 'highlight-cell-white';
  //         }
  //         else {
  //             cellClass = '';
  //         }
  //         break;
  //       case 'PTA':
  //         var tmp: string[] = ['0', '0', '0', '1', '0', '0', '0', '0', '1', '1']; 
  //         var idx = rowIdx? rowIdx : 0;
  //         if (tmp[idx%10] == '1')
  //         {
  //           if (this.darkMode == 'dark')
  //             cellClass = 'highlight-cell-red-dark';
  //           else  
  //             cellClass = 'highlight-cell-red';
  //         }
  //         else {
  //             cellClass = '';
  //         }
  //         break;
  //       case 'Driver':
  //         var tmp: string[] = ['0', '1', '0', '1', '0', '0', '0', '0', '0', '2']; 
  //         var idx = rowIdx? rowIdx : 0;
  //         if (tmp[idx%10] == '1')
  //         {
  //           if (this.darkMode == 'dark')
  //             cellClass = 'highlight-cell-yellow-dark';
  //           else  
  //             cellClass = 'highlight-cell-yellow';
  //         }
  //         else if (tmp[idx%10] == '2')
  //         {
  //           if (this.darkMode == 'dark')
  //           {         
  //             cellClass = 'highlight-cell-bg-green-dark';
  //           }
  //           else 
  //             cellClass = 'highlight-cell-green';
  //         }
  //         break;
  //       case 'Plan':
  //         if (this.darkMode == 'dark')
  //           {         
  //             cellClass = 'highlight-cell-green-dark';
  //           }
  //           else 
  //             cellClass = 'highlight-cell-green'; 
  //           break;
  //     }
  // //       var tmp: string[] = ['0', '0', '0', '1', '0', '0', '1', '0', '1', '1']; 
  // // var idx = params.node?.id? parseInt(params.node.id) : 0;
  //     return cellClass;
  //   }


  getCssVariable(name: string): string {
    return getComputedStyle(document.documentElement)
      .getPropertyValue(name)
      .trim();
  }

  

// isSorted: boolean = false;
//   customSort(event: SortEvent) {
//         if (this.isSorted == null || this.isSorted === undefined) {
//             this.isSorted = true;
//             this.sortTableData(event);
//         } else if (this.isSorted == true) {
//             this.isSorted = false;
//             this.sortTableData(event);
//         } else if (this.isSorted == false) {
//             this.isSorted = false;
//             this.rowData = [...this.initialData];
//             this.table.clear();
//         }
//     }

//     sortTableData(event: SortEvent) {
//         event.data?.sort((data1, data2) => {
//             let value1 = data1[event.field?event.field:''];
//             let value2 = data2[event.field?event.field:''];
//             let result = null;
//             if (value1 == null && value2 != null) result = -1;
//             else if (value1 != null && value2 == null) result = 1;
//             else if (value1 == null && value2 == null) result = 0;
//             else if (typeof value1 === 'string' && typeof value2 === 'string') result = value1.localeCompare(value2);
//             else result = value1 < value2 ? -1 : value1 > value2 ? 1 : 0;
        
//             return event.order? event.order * result : '';
//         });
//     }


}

    