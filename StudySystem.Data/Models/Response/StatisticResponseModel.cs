using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class StatisticResponseModel
    {
        public double RevenusThisYear { get; set; }
        public double RevenusThisDay { get; set; }
        public int TotalProductQuantity { get; set; }
        public int TotalOrderQuantity { get; set; }
        public List<double> RevenusByMonth { get; set; }
        public List<double> OverviewCustomer { get; set; }
        public CompareBestSelling CompareBestSellingData { get; set; }
    }

    public class CompareBestSelling
    {
        public string NameProductFirst { get; set; }
        public List<double> DataProductFirst { get; set; }
        public string NameProductSecond { get; set; }
        public List<double> DataProductSecond { get; set; }
        public string NameProductLast { get; set; }
        public List<double> DataProductLast { get; set; }
    }
}
