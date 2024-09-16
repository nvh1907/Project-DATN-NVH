import { ChartOptions } from 'chart.js';
import { Component } from '@angular/core';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.scss']
})
export class PieChartComponent {
  public pieChartOptions: ChartOptions<'pie'> = {
    responsive: false,
  };
  public pieChartLabels = ['Người đăng ký onl', 'Người mua hàng'];
  public pieChartDatasets = [{
    data: [500, 200]
  }];
  public pieChartLegend = true;
  public pieChartPlugins = [];

}
