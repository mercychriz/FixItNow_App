using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FixITNowWebApp.Models;
using System.Diagnostics;
using System.Collections.Generic;
using FixITNowWebApp.DTOs;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mime;


namespace FixITNowWebApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "http://192.168.24.150:5074/api";

        public ApiService()
        {
            _client = new HttpClient();
        }

        // ✅ Register a new user
        public async Task<string> RegisterUserAsync(string fullName, string email, string password, string role, string gender)
        {
            try
            {
                var checkUrl = $"{BaseUrl}/User/CheckEmail?email={Uri.EscapeDataString(email)}";
                var checkResponse = await _client.GetAsync(checkUrl);
                var checkContent = await checkResponse.Content.ReadAsStringAsync();
                var checkResult = JsonConvert.DeserializeObject<dynamic>(checkContent);

                if (checkResult.exists == true)
                {
                    return "Email already in use";
                }

                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    PasswordHash = password,
                    Role = role,
                    Gender = gender
                };

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{BaseUrl}/User/register";
                var response = await _client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"➡️ RegisterUserAsync Response: {responseContent}");

                return response.IsSuccessStatusCode ? "Success" : responseContent;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RegisterUserAsync Exception: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        // ✅ Login
        public async Task<User> LoginUserAsync(string email, string password)
        {
            try
            {
                var loginData = new
                {
                    Email = email,
                    PasswordHash = password
                };

                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{BaseUrl}/User/login";
                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(result);
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoginUserAsync Exception: {ex.Message}");
                return null;
            }
        }

        // ✅ Forgot Password
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { Email = email });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BaseUrl}/User/forgotpassword", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ForgotPasswordAsync Exception: {ex.Message}");
                return false;
            }
        }

        // ✅ Reset Password
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { Email = email, NewPassword = newPassword });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BaseUrl}/User/resetpassword", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ResetPasswordAsync Exception: {ex.Message}");
                return false;
            }
        }

        // ✅ Save User Profile
        public async Task<ApiResponse> SaveUserProfileAsync(UserProfileDto userProfile)
        {
            try
            {
                var json = JsonConvert.SerializeObject(userProfile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BaseUrl}/UserProfile", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"➡️ SaveUserProfileAsync Response: {responseContent}");

                return response.IsSuccessStatusCode
                    ? new ApiResponse { Success = true }
                    : new ApiResponse { Success = false, ErrorMessage = $"Server returned {response.StatusCode}: {responseContent}" };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ SaveUserProfileAsync Exception: {ex.Message}");
                return new ApiResponse { Success = false, ErrorMessage = $"Exception: {ex.Message}" };
            }
        }

        public async Task<UserProfileDto> GetUserProfileByUserIdAsync(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"http://192.168.24.150:5074/api/UserProfile/byuserid/{userId}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserProfileDto>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetUserProfileByUserIdAsync Exception: {ex.Message}");
                return null;
            }
        }
        // ✅ GET ALL SERVICE PROVIDERS
        public async Task<List<ServiceProvider>> GetServiceProvidersAsync()
        {
            try
            {
                var url = "http://192.168.24.150:5074/api/ServiceProvider";
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new List<ServiceProvider>();

                var json = await response.Content.ReadAsStringAsync();
                var providers = JsonConvert.DeserializeObject<List<ServiceProvider>>(json);
                return providers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetServiceProvidersAsync Exception: {ex.Message}");
                return new List<ServiceProvider>();
            }
        }
        public async Task<ServiceProvider> GetServiceProviderProfileByUserIdAsync(int userId)
        {
            try
            {
                var url = $"{BaseUrl}/ServiceProvider/UserProfile/{userId}";
                Debug.WriteLine($"📡 Calling API: {url}");

                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("❌ API call failed.");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ServiceProvider>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Exception: {ex.Message}");
                return null;
            }
        }



        public async Task<ApiResponse> SaveServiceProviderProfileAsync(ServiceProvider serviceProvider)
        {
            try
            {
                var json = JsonConvert.SerializeObject(serviceProvider);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{BaseUrl}/ServiceProvider/CreateServiceProvider";
                var response = await _client.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? new ApiResponse { Success = true }
                    : new ApiResponse { Success = false, ErrorMessage = responseContent };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        //landing page for service providers


        public async Task<List<Service>> GetServicesByProviderIdAsync(int serviceProviderId)
        {
            try
            {
                var url = $"{BaseUrl}/ServiceProvider/{serviceProviderId}/services";
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new List<Service>();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Service>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetServicesByProviderIdAsync Exception: {ex.Message}");
                return new List<Service>();
            }
        }


        //appointment
        public async Task<List<Appointment>> GetAppointmentsByProviderIdAsync(int serviceProviderId)
        {
            try
            {
                var url = $"{BaseUrl}/Appointment/byprovider/{serviceProviderId}";
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new List<Appointment>();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Appointment>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetAppointmentsByProviderIdAsync Exception: {ex.Message}");
                return new List<Appointment>();
            }
        }

        public async Task<bool> AddServiceAsync(ServiceDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BaseUrl}/AdminServices", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"🔁 AddServiceAsync Response: {response.StatusCode} | {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ AddServiceAsync Exception: {ex.Message}");
                return false;
            }
        }

        // ✅ Update Service
        public async Task<ServiceDto> GetServiceByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{BaseUrl}/AdminServices/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ServiceDto>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ GetServiceByIdAsync Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateServiceAsync(int id, ServiceDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"http://192.168.24.150:5074/api/AdminServices/{id}", content);

                var resContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"🟡 PUT Response: {response.StatusCode} | {resContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ UpdateServiceAsync Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<ServiceProvider> GetServiceProviderByIdAsync(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/ServiceProvider/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceProvider>(json);
        }




    }
}

