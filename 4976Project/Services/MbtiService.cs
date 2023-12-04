using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;
using _4976Project.Data;
using _4976Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace _4976Project.Services
{
    public class MbtiService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ApplicationDbContext _context;

        public MbtiService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _context = context;
        }

        public List<string> ExtractMBTITypes(string text)
        {
            var matches = Regex.Matches(text, @"\b(ESTJ|ENTJ|ESFJ|ENFJ|ISTJ|ISFJ|INTJ|INFJ|ESTP|ESFP|ENTP|ENFP|ISTP|ISFP|INTP|INFP)\b");
            var mbtiTypes = matches.Cast<Match>().Select(m => m.Value).ToList();
            return mbtiTypes;
        }

        public async Task SaveMbtiResults(string mbti)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == user.Identity.Name);
            if (userInDb != null)
            {
                userInDb.Mbti = mbti;
                // Save changes to database
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<string> SendMessage(string combinedMessage)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://chatgpt-api7.p.rapidapi.com/ask"),
                Headers = {
                    { "X-RapidAPI-Key", "8b1b2714d2msh19c8e1bbadddbffp18f8a6jsn3862b5350c90" },
                    { "X-RapidAPI-Host", "chatgpt-api7.p.rapidapi.com" },
                },
                Content = new StringContent("{\r \"query\": \"" + combinedMessage + "\"\r }")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(body);
                var mbtiType = ExtractMBTITypes(json["response"].ToString()).FirstOrDefault();
                return mbtiType;
            }
        }

        public async Task<string> getDescription(string combinedMessage)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://chatgpt-api7.p.rapidapi.com/ask"),
                Headers = {
                { "X-RapidAPI-Key", "8b1b2714d2msh19c8e1bbadddbffp18f8a6jsn3862b5350c90" },
                { "X-RapidAPI-Host", "chatgpt-api7.p.rapidapi.com" },
                },
                Content = new StringContent("{\r \"query\": \"" + combinedMessage + "\"\r }")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(body);
                return json["response"].ToString();
            }
        }

        public async Task<string> findMatch(string combinedMessage)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://chatgpt-api7.p.rapidapi.com/ask"),
                Headers = {
                { "X-RapidAPI-Key", "8b1b2714d2msh19c8e1bbadddbffp18f8a6jsn3862b5350c90" },
                { "X-RapidAPI-Host", "chatgpt-api7.p.rapidapi.com" },
                },
                Content = new StringContent("{\r \"query\": \"" + combinedMessage + "\"\r }")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(body);
                return json["response"].ToString();
            }
        }
    }
}