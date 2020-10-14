using Newtonsoft.Json;
using OrderMaking.Mobile.Models;
using OrderMaking.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace OrderMaking.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        ZXingScannerPage scanPage;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();


            LoadCategory();
            //var cats = new List<Category>()
            //{
            //    new Category { Id = 1, Name = "Soft Drinks 500ml" },
            //    new Category { Id = 2, Name = "Soft Drinks 330ml Cans" }
            //};
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            // ItemsListView.SelectedItem = null;
        }

        async void ScanItem_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectedCategory == null)
            {
                DisplayAlert("Select Category", "Please Select Category", "OK");
                return;
            }

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

                    string numberOfItemsResult = await DisplayPromptAsync("Number of Items", "", initialValue: "", maxLength: 3, keyboard: Keyboard.Numeric);

                    int numberOfItems = string.IsNullOrEmpty(numberOfItemsResult) ? 0 : Convert.ToInt32(numberOfItemsResult);

                    var shoppingCart = new ShoppingCart
                    {
                        Barcode = result.Text,
                        CategoryId = viewModel.SelectedCategory.Id,
                        OrderDate = DateTime.UtcNow,
                        NumberOfItems = numberOfItems
                    };

                    DidAddScan(shoppingCart);
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
        }

        async void TypeProduct_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectedCategory == null)
            {
                DisplayAlert("Select Category", "Please Select Category", "OK");
                return;
            }

            popupProductInputView.IsVisible = true;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var shoppingCart = new ShoppingCart
            {
                ItemDescription = ItemDescription.Text,
                CategoryId = viewModel.SelectedCategory.Id,
                OrderDate = DateTime.UtcNow,
                NumberOfItems = !string.IsNullOrEmpty(NumberOfItems.Text) ? Convert.ToInt32(NumberOfItems.Text) : 0
            };

            DidAddScan(shoppingCart);
            popupProductInputView.IsVisible = false;
            ItemDescription.Text = "";
            NumberOfItems.Text = "";
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

        async void LoadCategory()
        {
            try
            {
                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

                var uri = new Uri($"{Constants.BaseUri}/Category");

                var response = httpClient.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync();
                    var cats = JsonConvert.DeserializeObject<List<Category>>(content.Result);
                    viewModel.Categories = new ObservableCollection<Category>(cats);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Scan Item", "Could not establish the connection to the server!", "Ok");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (scanPage != null)
                scanPage.IsScanning = false;
        }

        public async Task DidAddScan(ShoppingCart shoppingCart)
        {
            try
            {
                var url = new Uri($"{Constants.BaseUri}/Cart");

                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

                var json = JsonConvert.SerializeObject(shoppingCart);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Scanned Barcode", "Item has been added to the order list", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Scanned Barcode", "Error Occured", "OK");
            }
            // This callback is called whenever a barcode is decoded.
        }

        public async Task DidRemoveScan(string barcode)
        {
            try
            {
                var url = new Uri($"{Constants.BaseUri}/Cart");

                var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

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