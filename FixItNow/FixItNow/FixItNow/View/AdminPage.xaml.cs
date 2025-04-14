using FixItNow.Models;
using FixItNow.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using System.Linq;
using System.IO;


namespace FixItNow.View
{
    public partial class AdminPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();
        private Service _selectedService; // Track single selection

        public AdminPage()
        {
            InitializeComponent();
            LoadServices();
        }

        private async void LoadServices()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;

                var services = await _apiService.GetAllServicesAsync();
                ServicesCollectionView.ItemsSource = services;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load services: {ex.Message}", "OK");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
            }
        }

        // Handle selection changed event to track selected service
        private void ServicesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedService = e.CurrentSelection.FirstOrDefault() as Service;
        }

        private async void OnAddNewServiceClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddServicePage());
        }

        private async void OnEditServiceClicked(object sender, EventArgs e)
        {
            if (_selectedService == null)
            {
                await DisplayAlert("Error", "Select a service first!", "OK");
                return;
            }

            await Navigation.PushAsync(new EditServicePage(_selectedService));
        }

        private async void OnDeleteServiceClicked(object sender, EventArgs e)
        {
            if (_selectedService == null)
            {
                await DisplayAlert("Error", "Select a service first!", "OK");
                return;
            }

            var confirm = await DisplayAlert("Delete", $"Delete {_selectedService.ServiceName}?", "Yes", "No");
            if (!confirm) return;

            var result = await _apiService.DeleteServiceAsync(_selectedService.ServiceId);

            if (result)
            {
                await DisplayAlert("Success", "Service deleted!", "OK");
                LoadServices();
            }
            else
            {
                await DisplayAlert("Error", "Delete failed!", "OK");
            }
        }

        private async void OnDownloadReportsClicked(object sender, EventArgs e)
        {
            var bytes = await _apiService.DownloadReportAsync();

            if (bytes == null)
            {
                await DisplayAlert("Error", "Failed to download report.", "OK");
                return;
            }

            var filePath = Path.Combine(FileSystem.CacheDirectory, "services_report.csv");
            File.WriteAllBytes(filePath, bytes);

            await DisplayAlert("Success", $"Report downloaded to: {filePath}", "OK");
        }

        private async void OnLogOutClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
