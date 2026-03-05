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


@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [ButtonModule, TableModule, MatButtonModule, CommonModule, MatExpansionModule,
    MatInputModule, MatFormFieldModule, MatIconModule, FormsModule, ContextMenuModule, ContextMenu],
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


    // getRandomData(numRec : number) : any {
    //   var data: any[] = [];
    //   for (let i = 0; i < numRec; i++) {
    //     data.push({
    //       ETADate: this.ETADateValueGetter(this.initialData[i]),
    //       ETATime: this.ETATimeValueGetter(this.initialData[i]),
    //       PTADate: this.PTADateValueGetter(this.initialData[i]),
    //       PTATime: this.PTATimeValueGetter(this.initialData[i]),
    //       PJADate: this.PJADateValueGetter(this.initialData[i],i),
    //       PJATime: this.PJATimeValueGetter(this.initialData[i],i),
    //       Cnd: CndValueGetter(),
    //       Trlr1: Trlr1ValueGetter(),
    //       Plan: PlanValueGetter(),
    //       RDO: RDOValueGetter(),
    //       VHOS11Hrs: VHOS11ValueGetter(),
    //       VHOS14Hrs: VHOS14ValueGetter(),
    //       VHOS70Hrs: VHOS70ValueGetter(),
    //       TPT: TPTValueGetter(),
    //     });
    //   }
    //   return data;
    // }

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

      return colorCode == ""? '' : colorCode + '-' + theme;
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

  // ETADateValueGetter(row: any) {
  //   var d = row.EODDAT.toString().trim();
  //   if (d.length < 8)
  //     return row.EODDAT;
  //   return d.substring(4,6) + '/' + d.substring(6,8);
  // }

  // ETATimeValueGetter(row: any) {
  //   var t = row.EODTIM.toString().trim();
  //   if (t.length < 8)
  //     return row.EODTIM;
  //   return t.substring(0,2) + ':' + t.substring(3,5);
  // }

  // PTADateValueGetter(row: any) {
  //   var d = row.EOPDAT.toString().trim();
  //   if (d.length < 8)
  //     return row.EODDAT;
  //   return d.substring(4,6) + '/' + d.substring(6,8);
  // }

  // PTATimeValueGetter(row: any) {
  //   var t = row.EOPTIM.toString().trim();
  //   if (t.length < 8)
  //     return row.EODTIM;
  //   return t.substring(0,2) + ':' + t.substring(3,5);
  // }

  // PJADateValueGetter(row: any, rowIdx: number) {
  //   var d = row.EOPDAT.toString().trim();
  //   var tmp: string[] = ['0', '0', '0', '1', '0', '0', '1', '0', '1', '1']; 
  //   var idx = rowIdx? rowIdx : 0;
  //   if (tmp[idx%10] == '0')
  //     return '';
  //   if (d.length < 8)
  //     return row.EODDAT;
  //   return d.substring(4,6) + '/' + d.substring(6,8);
  // }

  // PJATimeValueGetter(row: any, rowIdx: number) {
  //   var t = row.EOPTIM.toString().trim();
  //   var tmp: string[] = ['0', '0', '0', '1', '0', '0', '1', '0', '1', '1']; 
  //   var idx = rowIdx? rowIdx : 0;
  //   if (tmp[idx%10] == '0')
  //     return '';
  //   if (t.length < 8)
  //     return row.EODTIM;
  //   return t.substring(0,2) + ':' + t.substring(3,5);
  // }


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





// function ETATimeValueGetter(params: ValueGetterParams) {
//   var t = params.data.EODTIM.toString().trim();
//   if (t.length < 8)
//     return params.data.EODTIM;
//   return t.substring(0,2) + ':' + t.substring(3,5);
// }

// function PTADateValueGetter(params: ValueGetterParams) {
//   var d = params.data.EOPDAT.toString().trim();
//   if (d.length < 8)
//     return params.data.EODDAT;
//   return d.substring(4,6) + '/' + d.substring(6,8);
// }

// function PTATimeValueGetter(params: ValueGetterParams) {
//   var t = params.data.EOPTIM.toString().trim();
//   if (t.length < 8)
//     return params.data.EODTIM;
//   return t.substring(0,2) + ':' + t.substring(3,5);
// }

// function PJDDateValueGetter(params: ValueGetterParams) {
//   var d = params.data.EOPDAT.toString().trim();
//   var tmp: string[] = ['0', '0', '0', '1', '0', '0', '1', '0', '1', '1']; 
//   var idx = params.node?.id? parseInt(params.node.id) : 0;
//   if (tmp[idx%10] == '0')
//     return '';
//   if (d.length < 8)
//     return params.data.EODDAT;
//   return d.substring(4,6) + '/' + d.substring(6,8);
// }

// function PJDTimeValueGetter(params: ValueGetterParams) {
//   var t = params.data.EOPTIM.toString().trim();
//   var tmp: string[] = ['0', '0', '0', '1', '0', '0', '1', '0', '1', '1']; 
//   var idx = params.node?.id? parseInt(params.node.id) : 0;
//   if (tmp[idx%10] == '0')
//     return '';
//   if (t.length < 8)
//     return params.data.EODTIM;
//   return t.substring(0,2) + ':' + t.substring(3,5);
// }

// function CndValueGetter() {
//   var tmpCnd: string[] = ['', 'D', 'DLP', 'L@', 'LE!', 'M', 'T', 'P', 'Dm', 'LPM'];

//   return tmpCnd[Math.floor(Math.random() * 10)];
// }

// function Trlr1ValueGetter() {

//   var tmpTrlr1: string[] = ['', 'A78394', '2332', 'B99884', 'G23009', '9008', 'K2290', 'D12231', '5344', '1239'];  

//   return tmpTrlr1[Math.floor(Math.random() * 10)];
// }


// function PlanValueGetter() {

//   var tmpPlan: string[] = ['', '00:51', '00:07', '', '00:34', '', '', '00:12', '', ''];  

//   return tmpPlan[Math.floor(Math.random() * 10)];
// }


// function RDOValueGetter() {

//   var tmpPlan: string[] = ['7', '8', '0', '', '', '', '', '13', '', '2'];  

//   return tmpPlan[Math.floor(Math.random() * 10)];
// }


// function VHOS11ValueGetter() {
//   var tmpHr= getRandomNumber(0, 11).toString().padStart(2,'0');
//   var tmpMin = tmpHr == '11'? '00' : getRandomNumber(0, 5).toString().padEnd(2,'0');

//   return tmpHr + ':' + tmpMin; 
// }


// function VHOS14ValueGetter() {
//   var tmpHr= getRandomNumber(0, 14).toString().padStart(2,'0');
//   var tmpMin =  tmpHr == '14'? '00' : getRandomNumber(0, 5).toString().padEnd(2,'0');

//   return tmpHr + ':' + tmpMin; 
// }

// function VHOS70ValueGetter() {
//   var tmpHr= getRandomNumber(0, 6).toString().padEnd(2,'0');
//   var tmpMin = getRandomNumber(0, 5).toString().padEnd(2,'0');

//   return tmpHr + ':' + tmpMin; 
// }


// function TPTValueGetter() {

//   var tmpTPT: string[] = ['O', 'N', 'O', '', '', '', 'N', '', 'O', 'N'];  

//   return tmpTPT[Math.floor(Math.random() * 10)];
// }

// function getRandomNumber(min: number, max: number): number {
//   const minCeil = Math.ceil(min);
//   const maxFloor = Math.floor(max);
//   return Math.floor(Math.random() * (maxFloor - minCeil + 1)) + minCeil;
// }



    