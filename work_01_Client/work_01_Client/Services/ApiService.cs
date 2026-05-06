using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using work_01_Client.DTOs;

namespace work_01_Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // Android Emulator-এর জন্য localhost হচ্ছে 10.0.2.2। আপনার API-এর পোর্ট (যেমন: 5000 বা 7000) এখানে বসান।
#if WINDOWS
        private const string BaseUrl = "http://localhost:5227/api/";
#else
        private const string BaseUrl = "http://10.0.2.2:5227/api/";
#endif

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // ১. সব ডাক্তারদের লিস্ট নিয়ে আসা (GET)
        public async Task<List<DoctorDTO>> GetDoctorsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<DoctorDTO>>("doctors");
                return response ?? new List<DoctorDTO>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"ডাটা আনতে সমস্যা হয়েছে: {ex.Message}", "OK");
                return new List<DoctorDTO>();
            }
        }

        // ২. নতুন ডাক্তার ও অ্যাপয়েন্টমেন্ট সেভ করা (POST)
        public async Task<bool> SaveDoctorAsync(DoctorDTO doctorDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("doctors", doctorDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"সেভ করা যায়নি: {ex.Message}", "OK");
                return false;
            }
        }
    }
}
