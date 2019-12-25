﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            var order = new { };

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                DisplayAlert("Generate Order", "Order has been placed, Please check your email", "OK");
            }
        }
    }
}