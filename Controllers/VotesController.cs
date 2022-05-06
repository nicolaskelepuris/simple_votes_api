using Microsoft.AspNetCore.Mvc;
using VoteApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace VoteApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VotesController
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VotesController(IHttpContextAccessor httpContextAccessor, DataContext dataContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataContext = dataContext;
    }

    [HttpPost("{voteValue}")]
    public async Task Vote([FromRoute] VoteValue voteValue)
    {
        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        if (ip == null || !Enum.IsDefined(voteValue)) return;

        var vote = new Vote(ip, voteValue);
        _dataContext.Add(vote);
        await _dataContext.SaveChangesAsync();
    }

    [HttpGet]
    public async Task<Response> GetVotes()
    {
        var queries = new List<Task<int>>()
        {
            _dataContext.Set<Vote>().CountAsync(_ => _.Value == VoteValue.Willy),
            _dataContext.Set<Vote>().CountAsync(_ => _.Value == VoteValue.Other)
        };
        var results = await Task.WhenAll(queries);

        return new Response { Willy = results[0], Other = results[1] };
    }

    public class Response
    {
        public int Willy { get; set; }
        public int Other { get; set; }
    }
}
