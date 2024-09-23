import { ChartOptions } from 'chart.js';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.scss']
})
export class PieChartComponent {
  @Input() pieChartDatasets: any;
  public pieChartOptions: ChartOptions<'pie'> = {
    responsive: false,
  };
  public pieChartLabels = ['Người đăng ký onl', 'Người mua hàng'];

  public pieChartLegend = true;
  public pieChartPlugins = [];

}
