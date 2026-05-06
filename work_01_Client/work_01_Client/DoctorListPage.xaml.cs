using work_01_Client.DTOs;
using work_01_Client.Services;
using System.Collections.ObjectModel;

namespace work_01_Client;

public partial class DoctorListPage : ContentPage
{
    private readonly ApiService _apiService;
    public ObservableCollection<DoctorDTO> Doctors { get; set; } = new ObservableCollection<DoctorDTO>();

	public DoctorListPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
        BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDoctors();
    }

    private async Task LoadDoctors()
    {
        var doctors = await _apiService.GetDoctorsAsync();
        Doctors.Clear();
        foreach (var doctor in doctors)
        {
            Doctors.Add(doctor);
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new AddDoctorPage());
    }
}