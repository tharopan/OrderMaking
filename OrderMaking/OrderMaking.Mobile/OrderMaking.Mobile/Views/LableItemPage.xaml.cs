using Newtonsoft.Json;
using OrderMaking.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace OrderMaking.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LableItemPage : ContentPage
    {
        ZXingScannerPage scanPage;

        public LableItemPage()
        {
            InitializeComponent();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            await Navigation.PushModalAsync(new NavigationPage(scanPage));

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //await Navigation.PopAsync();
                    await Navigation.PopModalAsync();
                    DidAddScan(result.Text);
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
        }

        async void RemoveItem_Clicked(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            await Navigation.PushModalAsync(new NavigationPage(scanPage));

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //await Navigation.PopAsync();
                    await Navigation.PopModalAsync();
                    DidRemoveScan(result.Text);
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
        }

        async void RemoveAllItems_Clicked(object sender, EventArgs e)
        {
            try
            {
                var url = new Uri($"{Constants.BaseUri}/Label");


                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

                HttpResponseMessage response = httpClient.DeleteAsync($"{url}").Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Scanned Barcode", "Label items have been removed", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Scanned Barcode", "Error Occured", "OK");
                //throw;
            }
        }

        async void PrintLabel_Clicked(object sender, EventArgs e)
        {
            var url = new Uri($"{Constants.BaseUri}/PrintFunction/");
            try
            {

                var order = new OrderType { };

                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };


                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Print Labels", "Print labels has been completed", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Generate Order", "Print labels failed.", "OK");
            }
        }

        public async Task DidAddScan(string barcode)
        {
            try
            {
                var url = new Uri($"{Constants.BaseUri}/Label");

                var shoppingCart = new LabelItem
                {
                    Barcode = barcode,
                };

                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

                var json = JsonConvert.SerializeObject(shoppingCart);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Scanned Barcode", "Item has been added to the lable list", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Scanned Barcode", "Error Occured", "OK");
                //throw;
            }
        }

        public async Task DidRemoveScan(string barcode)
        {
            try
            {
                var url = new Uri($"{Constants.BaseUri}/Label");


                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

                var json = JsonConvert.SerializeObject(new { Barcode = barcode});
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.DeleteAsync($"{url}?barcode={barcode}").Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Scanned Barcode", "Item has been removed", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Scanned Barcode", "Error Occured", "OK");
                //throw;
            }
        }
    }
}