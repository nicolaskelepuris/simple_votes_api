using Microsoft.EntityFrameworkCore;
using VoteApi.Entities;

namespace VoteApi.UseCases
{
    public interface ISaveVote
    {
        Task VoteAsync(VoteValue voteValue);
    }

    public class SaveVote : ISaveVote
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SaveVote(IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
        }

        public async Task VoteAsync(VoteValue voteValue)
        {
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            if (ip == null || !Enum.IsDefined(voteValue) || await AlreadyVoted(ip)) return;

            var vote = new Vote(ip, voteValue);
            _dataContext.Add(vote);
            await _dataContext.SaveChangesAsync();
        }

        private async Task<bool> AlreadyVoted(string ip)
        {
            return await _dataContext.Set<Vote>().AnyAsync(_ => _.Id == ip);
        }
    }
}