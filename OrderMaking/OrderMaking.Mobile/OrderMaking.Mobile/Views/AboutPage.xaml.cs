using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace OrderMaking.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        async void GenerateOrder_Clicked(object sender, EventArgs e)
        {
            var url = new Uri($"{Constants.BaseUri}/Function");
            try
            {

                var order = new { OderList = "Groceries" };

                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };


                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Generate Order", "Order has been placed, Please check your email", "OK");
                }
            }
            catch (Exception ex)
            {

                DisplayAlert("Generate Order", "Generate Order failed, Please copy the file manually.", "OK");
            }
        }

        async void GenerateCigarettes_Clicked(object sender, EventArgs e)
        {
            var url = new Uri($"{Constants.BaseUri}/Function/");
            try
            {

                var order = new { OderList = "Cigarettes" };

                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };


                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Generate Order", "Order has been placed, Please check your email", "OK");
                }
            }
            catch (Exception ex)
            {

                DisplayAlert("Generate Order", "Generate Order failed, Please copy the file manually.", "OK");
            }
        }
    }
}