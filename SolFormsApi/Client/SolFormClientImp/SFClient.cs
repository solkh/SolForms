using SolForms.Models.Questions;
using SolForms.Models;
using Microsoft.Extensions.Options;
using SolFormsApi.Options;

namespace SolFormsApi.Client.SolFormClientImp
{
    public class SFClient : ISFClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly FormEndPointsOptions _options;
        
        public SFClient(string apiUrl, IOptions<FormEndPointsOptions> options)
        {
            _options = options.Value;
            _apiUrl = apiUrl + $"/{_options.BaseUrl}";
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            };            
        }
        public async Task<TEntity?> Get<TEntity>(Guid id)
        {
            var suffix = GetUrlSuffix<TEntity>();
            var response = await _httpClient.GetAsync($"{_apiUrl}/{suffix}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TEntity>();
        }
        public async Task<TEntity?> GetAll<TEntity>()
        {
            var suffix = GetUrlSuffix<TEntity>();
            var response = await _httpClient.GetAsync($"{_apiUrl}/{suffix}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TEntity>();
        }

        public async Task Post<TEntity>(TEntity data)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/SolForms", data);
            response.EnsureSuccessStatusCode();
        }
        public async Task<TResponse?> Post<TResponse, T>(T data)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/SolForms", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
        }

        public async Task Put<T>(T data)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/SolForms", data);
            response.EnsureSuccessStatusCode();
        }
        public async Task<TResponse?> Put<TResponse, T>(T data)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/SolForms", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
        }

        public async Task Delete<T>(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/SolForms");
            response.EnsureSuccessStatusCode();
        }

        private string GetUrlSuffix<TEntity>()
        {            
            if (typeof(TEntity) == typeof(SolForm))
                return _options.Forms;
            else if (typeof(TEntity) == typeof(SFSection))
                return _options.Section;
            else if (typeof(TEntity) == typeof(SFQuestion))
                return _options.Question;
            else if (typeof(TEntity) == typeof(SFOption))
                return _options.Option;
            else if (typeof(TEntity) == typeof(SFShowCondition))
                return _options.Condition;
            else if (typeof(TEntity) == typeof(SFSubmission))
                return _options.Submissions;
            else if (typeof(TEntity) == typeof(SFAnswer))
                return _options.Answer;
            return string.Empty;
        }
    }
}
