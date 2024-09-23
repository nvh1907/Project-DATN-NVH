import { ChartConfiguration } from 'chart.js';
import { Component } from '@angular/core';
import { ChartsService } from 'src/app/service/charts.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {

  constructor(private chart: ChartsService) {

  }
  dataAPI: any;
  interval: any;
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.chart.getDataToShow().subscribe(res => {
      this.dataAPI = res
      this.lineChartData = {
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
            data: res.revenusByMonth,
            label: 'Doanh thu các tháng trong năm ' + this.thisYear + '(VND)',
            fill: true,
            tension: 0.5,
            borderColor: 'black',
            backgroundColor: 'rgba(255,0,0,0.3)'
          }
        ]
      };
      this.lineChartDataTotal = {
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
            data: res.compareBestSellingData.dataProductFirst,
            label: res.compareBestSellingData.nameProductFirst,
            tension: 0.5,
            borderColor: 'red',
          },
          {
            data: res.compareBestSellingData.dataProductSecond,
            label: res.compareBestSellingData.nameProductSecond,
            tension: 0.5,
            borderColor: 'blue',
          },
          {
            data: res.compareBestSellingData.dataProductLast,
            label: res.compareBestSellingData.nameProductLast,
            tension: 0.5,
            borderColor: 'yellow',
          }
        ]
      };
      this.pieChartDatasets = [{
        data: res.overviewCustomer
      }]
    });
    this.interval = setInterval(() => {
      this.chart.getDataToShow().subscribe(res => {
        if (!this.isEqual(this.dataAPI, res)) {
          this.dataAPI = res
          this.lineChartData = {
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
                data: res.revenusByMonth,
                label: 'Doanh thu các tháng trong năm ' + this.thisYear + '(VND)',
                fill: true,
                tension: 0.5,
                borderColor: 'black',
                backgroundColor: 'rgba(255,0,0,0.3)'
              }
            ]
          };
          this.lineChartDataTotal = {
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
                data: res.compareBestSellingData.dataProductFirst,
                label: res.compareBestSellingData.nameProductFirst,
                tension: 0.5,
                borderColor: 'red',
              },
              {
                data: res.compareBestSellingData.dataProductSecond,
                label: res.compareBestSellingData.nameProductSecond,
                tension: 0.5,
                borderColor: 'blue',
              },
              {
                data: res.compareBestSellingData.dataProductLast,
                label: res.compareBestSellingData.nameProductLast,
                tension: 0.5,
                borderColor: 'yellow',
              }
            ]
          };
          this.pieChartDatasets = [{
            data: res.overviewCustomer
          }]
          console.log('update')
        }

      });
      console.log("get-data")
    }, 5000)
  }

  pieChartDatasets: any;
  thisYear: number = new Date().getUTCFullYear();
  //#region  chart doanh thu
  widthChart = '750rem';
  heightChart = '400rem';
  public lineChartData!: ChartConfiguration<'line'>['data'];
  //#endregion

  //#region  chart total
  widthChartTotal = "1260rem"
  heightChartTotal = "500rem"
  public lineChartDataTotal!: ChartConfiguration<'line'>['data'];
  //#endregion


  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    if (this.interval) {
      clearInterval(this.interval)
      console.log('stop-call')
    }
  }

  // compare
  isEqual(obj1: any, obj2: any): boolean {
    return JSON.stringify(obj1) === JSON.stringify(obj2);
  }
}

