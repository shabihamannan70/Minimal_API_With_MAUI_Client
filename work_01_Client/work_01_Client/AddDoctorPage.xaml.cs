using work_01_Client.DTOs;
using work_01_Client.Services;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace work_01_Client;

public partial class AddDoctorPage : ContentPage
{
    private readonly ApiService _apiService;
    public ObservableCollection<AppointmentViewModel> AppointmentsList { get; set; } = new ObservableCollection<AppointmentViewModel>();
    private string _base64Picture = "";

	public AddDoctorPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
        AppointmentsCollection.ItemsSource = AppointmentsList;
	}

    private async void SelectPicture_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Doctor Picture",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                _base64Picture = Convert.ToBase64String(imageBytes);
                
                // Show preview
                DoctorImagePreview.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Picture selection failed: {ex.Message}", "OK");
        }
    }

    private void AddAppointment_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PatientNameEntry.Text))
        {
            DisplayAlert("Error", "Please enter patient name", "OK");
            return;
        }

        var appointment = new AppointmentViewModel
        {
            PatientName = PatientNameEntry.Text,
            Phone = PatientPhoneEntry.Text ?? "",
            AppointmentDate = AppointmentDatePicker.Date ?? DateTime.Today
        };

        AppointmentsList.Add(appointment);

        // Clear fields
        PatientNameEntry.Text = string.Empty;
        PatientPhoneEntry.Text = string.Empty;
        AppointmentDatePicker.Date = DateTime.Today;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Error", "Please enter doctor name", "OK");
            return;
        }

        if (!decimal.TryParse(FeeEntry.Text, out decimal fee))
        {
            await DisplayAlert("Error", "Please enter valid visiting fee", "OK");
            return;
        }

        string appointmentJson = null;

        if (AppointmentsList.Count > 0)
        {
            appointmentJson = JsonSerializer.Serialize(AppointmentsList);
        }

        var newDoctor = new DoctorDTO
        {
            DoctorName = NameEntry.Text,
            VisitingFee = fee,
            InDhaka = InDhakaSwitch.IsToggled,
            PictureBase64 = _base64Picture,
            AppointmentJson = appointmentJson
        };

        var success = await _apiService.SaveDoctorAsync(newDoctor);

        if (success)
        {
            await DisplayAlert("Success", "Doctor saved successfully!", "OK");
            await Navigation.PopAsync(); // Go back to the list page
        }
    }
}