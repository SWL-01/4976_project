using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;
using _4976Project.Data;
using _4976Project.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}