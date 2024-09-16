import { ChartConfiguration } from 'chart.js';
import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {

  constructor() {

  }

  //#region  chart doanh thu
  widthChart = '750rem';
  heightChart = '400rem';
  public lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'Octember',
      'November',
      'December'
    ],
    datasets: [
      {
        data: [200001, 200002, 200009, 200006, 200004, 200004, 200101, 200111, 200222, 200333, 200020, 203000],
        label: 'Doanh thu (VND)',
        fill: true,
        tension: 0.5,
        borderColor: 'black',
        backgroundColor: 'rgba(255,0,0,0.3)'
      }
    ]
  };
  //#endregion

  //#region  chart total
  widthChartTotal = "1260rem"
  heightChartTotal = "500rem"
  public lineChartDataTotal: ChartConfiguration<'line'>['data'] = {
    labels: [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'Octember',
      'November',
      'December'
    ],
    datasets: [
      {
        data: [200001, 200002, 200009, 200006, 200004, 200004, 200101, 200111, 200222, 200333, 200020, 203000],
        label: 'Sản phẩm 1',
        tension: 0.5,
        borderColor: 'red',
      },
      {
        data: [290001, 209002, 200809, 208006, 200004, 206004, 200101, 250111, 210222, 230333, 200020, 205000],
        label: 'Sản phẩm 2',
        tension: 0.5,
        borderColor: 'blue',
      },
      {
        data: [280001, 204002, 206809, 208906, 200504, 208804, 200101, 250111, 210222, 230333, 200020, 205000],
        label: 'Sản phẩm 3',
        tension: 0.5,
        borderColor: 'yellow',
      }
    ]
  };
  //#endregion

}

