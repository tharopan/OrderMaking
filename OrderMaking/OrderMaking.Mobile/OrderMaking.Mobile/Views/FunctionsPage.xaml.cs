using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderMaking.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FunctionsPage : ContentPage
    {
        public FunctionsPage()
        {
            InitializeComponent();
        }

        async void SyncDb_Clicked(object sender, EventArgs e)
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
    }
}