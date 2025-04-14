using FixItNow.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace FixItNow.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "http://192.168.24.150:5074/api/User";


        public ApiService()
        {
            _client = new HttpClient();
        }

        public async Task<string> RegisterUserAsync(string fullName, string email, string password, string role, string gender)
        {
            try
            {
                // Step 1: Check if email already exists
                var checkUrl = $"http://192.168.24.150:5074/api/User/CheckEmail?email={Uri.EscapeDataString(email)}";
                var checkResponse = await _client.GetAsync(checkUrl);
                var checkContent = await checkResponse.Content.ReadAsStringAsync();

                var checkResult = JsonConvert.DeserializeObject<dynamic>(checkContent);

                if (checkResult.exists == true)
                {
                    return "Email already in use";
                }

                // Step 2: Proceed with registration
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

                var url = "http://192.168.24.150:5074/api/User/register";

                System.Diagnostics.Debug.WriteLine($"Sending POST to {url} with data: {json}");

                var response = await _client.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"Response StatusCode: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                {
                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in RegisterUserAsync: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }


        public async Task<User> LoginUserAsync(string email, string password)
        {
            try
            {
                var loginInfo = new
                {
                    Email = email,
                    PasswordHash = password
                };

                var json = JsonConvert.SerializeObject(loginInfo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://192.168.24.150:5074/api/User/login";
                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(responseContent);
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                var data = new { Email = email };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://192.168.24.150:5074/api/User/forgotpassword"; 
                var response = await _client.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ForgotPassword Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                var data = new
                {
                    Email = email,
                    NewPassword = newPassword
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("http://192.168.24.150:5074/api/User/resetpassword", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ResetPassword Exception: {ex.Message}");
                return false;
            }
        }



        public async Task<ApiResponse> SaveUserProfileAsync(UserProfileDto userProfile)
        {
            try
            {
                var json = JsonConvert.SerializeObject(userProfile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                System.Diagnostics.Debug.WriteLine($"➡️ Payload JSON: {json}");

                var url = "http://192.168.24.150:5074/api/UserProfile";  // Your API endpoint!
                var response = await _client.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"➡️ Response Status Code: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"➡️ Profile Save Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse
                    {
                        Success = true
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = $"Server returned {response.StatusCode}. Response: {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SaveUserProfileAsync Exception: {ex.Message}");
                return new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Exception occurred: {ex.Message}"
                };
            }
        }
    

        public async Task<ApiResponse> SaveServiceProviderProfileAsync(ServiceProvider serviceProvider)
        {
            try
            {
                var json = JsonConvert.SerializeObject(serviceProvider);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://192.168.24.150:5074/api/ServiceProvider/CreateServiceProvider";  
                var response = await _client.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse { Success = true };
                }

                return new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Server returned {response.StatusCode}: {responseContent}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }


        public async Task<bool> CheckIfServiceProviderProfileExists(int userId)
        {
            try
            {
                var url = $"http://192.168.24.150:5074/api/ServiceProvider/HasProfile/{userId}"; // Update with your API endpoint!
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return false;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<HasProfileResponse>(jsonResponse);

                return result.HasProfile;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking profile: {ex.Message}");
                return false;
            }
        }

        public class HasProfileResponse
        {
            public bool HasProfile { get; set; }
        }
        public async Task<ServiceProvider> GetServiceProviderProfileAsync(int userId)


        {
            try
            {
                var response = await _client.GetAsync($"http://192.168.24.150:5074/api/ServiceProvider/UserProfile/{userId}");


                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();

                var provider = JsonConvert.DeserializeObject<ServiceProvider>(json);

                return provider;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ GetServiceProviderProfileAsync Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<List<ServiceProvider>> GetServiceProvidersAsync()
        {
            var url = "http://192.168.24.150:5074/api/ServiceProvider";  
            try
            {
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // Deserialize to List<ServiceProvider>
                    var providers = JsonConvert.DeserializeObject<List<ServiceProvider>>(json);

                    return providers;
                }
                else
                {
                    // Optional: log error or display alert
                    return new List<ServiceProvider>();  // Return an empty list on error
                }
            }
            catch (System.Exception ex)
            {
                // Optional: log exception
                System.Diagnostics.Debug.WriteLine($"❌ GetServiceProvidersAsync Exception: {ex.Message}");
                return new List<ServiceProvider>();
            }



        }
        public async Task<List<ServiceProvider>> SearchServiceProvidersAsync(string keyword)
        {
            var url = $"http://192.168.24.150:5074/api/ServiceProvider/Search?keyword={keyword}";

            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var providers = JsonConvert.DeserializeObject<List<ServiceProvider>>(json);

                    return providers;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Search failed: {response.StatusCode}");
                    return new List<ServiceProvider>();
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SearchServiceProvidersAsync Exception: {ex.Message}");
                return new List<ServiceProvider>();
            }
        }

        public async Task<ServiceProvider> GetServiceProviderByIdAsync(int serviceProviderId)
        {
            try
            {
                var url = $"http://192.168.24.150:5074/api/ServiceProvider/{serviceProviderId}";
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Failed to fetch ServiceProvider: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var provider = JsonConvert.DeserializeObject<ServiceProvider>(json);

                return provider;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception in GetServiceProviderByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddServiceAsync(Service service)
        {
            try
            {
                var dto = new ServiceDto
                {
                    ServiceName = service.ServiceName,
                    ServiceDescription = service.ServiceDescription,
                    Price = service.Price,
                    ServiceProviderId = service.ServiceProviderId
                };

                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://192.168.24.150:5074/api/AdminServices";

                Debug.WriteLine($"➡️ Sending JSON to {url}: {json}");

                var response = await _client.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"➡️ Response Status Code: {response.StatusCode}");
                Debug.WriteLine($"➡️ Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ AddServiceAsync Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateServiceAsync(Service service)
        {
            try
            {
                var data = new
                {
                    ServiceName = service.ServiceName,
                    ServiceDescription = service.ServiceDescription,
                    Price = service.Price,
                    ServiceProviderId = service.ServiceProviderId
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"http://192.168.24.150:5074/api/AdminServices/{service.ServiceId}";


                System.Diagnostics.Debug.WriteLine($"➡️ Sending PUT request to: {url}");
                System.Diagnostics.Debug.WriteLine($"➡️ Payload JSON: {json}");
                System.Diagnostics.Debug.WriteLine($"🧪 ProviderId: {service.ServiceProviderId}, ServiceId: {service.ServiceId}");

                var response = await _client.PutAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"➡️ Response Status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"➡️ Response Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    string message;

                    try
                    {
                        var errorObj = JsonConvert.DeserializeObject<JObject>(responseContent);

                        if (errorObj["errors"] != null)
                        {
                            var errorMessages = errorObj["errors"]
                                .Children()
                                .SelectMany(e => e.Children())
                                .Select(e => e.ToString());

                            message = string.Join("\n", errorMessages);
                        }
                        else if (errorObj["title"] != null)
                        {
                            message = errorObj["title"]?.ToString();
                        }
                        else
                        {
                            message = responseContent;
                        }
                    }
                    catch
                    {
                        message = responseContent;
                    }

                    await Application.Current.MainPage.DisplayAlert("Update Failed", message, "OK");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ UpdateServiceAsync Exception: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
                return false;
            }
        }



        public async Task<List<Service>> GetServicesByProviderIdAsync(int providerId)
        {
            try
            {
                var url = $"http://192.168.24.150:5074/api/Service/ByProvider/{providerId}"; // ✅ Make sure this matches your backend endpoint

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var services = JsonConvert.DeserializeObject<List<Service>>(json);
                    return services;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Failed to load services: {response.StatusCode}");
                    return new List<Service>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception in GetServicesByProviderIdAsync: {ex.Message}");
                return new List<Service>();
            }
        }

        public async Task<List<Appointment>> GetAppointmentsByProviderIdAsync(int providerId)
        {
            try
            {
                var url = $"http://192.168.24.150:5074/api/Appointment/ByProvider/{providerId}"; // ✅ Match this with your backend route

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
                    return appointments;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Failed to load appointments: {response.StatusCode}");
                    return new List<Appointment>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception in GetAppointmentsByProviderIdAsync: {ex.Message}");
                return new List<Appointment>();
            }
        }
        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            try
            {
                var url = $"http://192.168.24.150:5074/api/Service/{serviceId}";  // 🔧 Update with your actual API URL.

                var response = await _client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Service with ID {serviceId} deleted successfully.");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Failed to delete service {serviceId}: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception deleting service: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> MarkAppointmentCompletedAsync(int appointmentId)
        {
            try
            {
                var url = $"http://192.168.24.150:5074/api/Appointments/{appointmentId}/Complete"; // Update URL according to your API route.

                // You can use PUT or POST depending on your API design.
                var response = await _client.PutAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Appointment {appointmentId} marked as completed.");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Failed to mark appointment complete: {response.StatusCode} {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception in MarkAppointmentCompletedAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            var response = await _client.GetAsync($"http://192.168.24.150:5074/api/AdminServices");
            if (!response.IsSuccessStatusCode) return new List<Service>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service>>(json);
        }


        public async Task<UserProfile> GetUserProfileByUserIdAsync(int userId)
        {
            var response = await _client.GetAsync($"http://192.168.24.150:5074/api/UserProfile/byuserid/{userId}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserProfile>(json);
        }




        public async Task<bool> BulkDeleteServicesAsync(List<int> serviceIds)
        {
            var json = JsonConvert.SerializeObject(serviceIds);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"http://192.168.24.150:5074/api/AdminServices/BulkDelete", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<byte[]> DownloadReportAsync()
        {
            var response = await _client.GetAsync($"http://192.168.24.150:5074/api/AdminServices/Download");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<string> BookAppointmentAsync(Appointment appointment)
        {
            try
            {
                var json = JsonConvert.SerializeObject(appointment, new JsonSerializerSettings
                {
                    Formatting = Formatting.None
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("http://192.168.24.150:5074/api/appointments", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? $"Success: {responseContent}"
                    : $"Error: {responseContent}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }





        public async Task<List<Appointment>> GetAppointmentsForProviderAsync(int providerId)
        {
            var response = await _client.GetAsync($"http://192.168.24.150:5074/api/appointments/provider/{providerId}");
            if (!response.IsSuccessStatusCode) return new List<Appointment>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Appointment>>(json);
        }
       
    }
}


