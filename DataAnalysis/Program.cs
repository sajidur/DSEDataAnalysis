using HtmlAgilityPack;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DataAnalysis
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            client.DefaultRequestHeaders.Accept.Clear();

            var stringTask = client.GetStringAsync("https://www.dse.com.bd/day_end_archive.php?startDate=2021-01-01&endDate=2021-06-28&inst=All%20Instrument&archive=data");

            var res = stringTask.Result.ToString();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(res);
            var headers = doc.DocumentNode.SelectNodes("//table[contains(@class, 'table table-bordered background-white shares-table fixedHeader')]//tr/th");
            DataTable table = new DataTable();
            foreach (HtmlNode header in headers)
                table.Columns.Add(header.InnerText.Replace('*',' ').Replace('#','I').TrimEnd()); // create columns from th
                                                     // select rows with td elements 
            var data = doc.DocumentNode.SelectNodes("//table[contains(@class, 'table table-bordered background-white shares-table fixedHeader')]//tbody/tr");
            foreach (var row in data)
            {
                var dtrow = row.SelectNodes("td").Select(td => td.InnerText.Trim()).ToArray();
                table.Rows.Add(dtrow);
            }
        }
    }
}
