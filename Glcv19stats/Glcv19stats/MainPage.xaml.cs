using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Xamarin.Forms;

namespace Glcv19stats
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ShowCovidData();
        }

        public async void ShowCovidData()
        {
            activity.IsVisible = true;

            try
            {
                var hc = new HttpClient();
                var result = await hc.GetAsync("https://api.covid19api.com/country/poland/status/confirmed");

                var covidData = JsonSerializer.Deserialize<List<CovidData>>(await result.Content.ReadAsStringAsync());

                var last = covidData.Last();

                cases.Text = last.Cases.ToString();
                cases2.Text = (last.Cases - covidData[covidData.Count - 2].Cases).ToString();
                date.Text = DateTime.Parse(last.Date).Humanize();

                activity.IsVisible = false;
                data.IsVisible = true;
            }
            catch (Exception e)
            {
                data.IsVisible = true;

                cases.Text = e.ToString();
                cases2.Text = "";
                date.Text = "";
            }
        }
    }

    internal class CovidData
    {
        public int Cases { get; set; }
        public string Date { get; set; }
    }
}