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
    public async Task<ApiResponse<Response>> GetVotes()
    {
        var queryResult = await _dataContext.Set<Vote>()
            .GroupBy(_ => _.Value)
            .Select(_ => new
            {
                Vote = _.Key,
                Count = _.Count()
            })
            .ToListAsync();

        var willy = queryResult.FirstOrDefault(_ => _.Vote == VoteValue.Willy)?.Count ?? 0;
        var other = queryResult.FirstOrDefault(_ => _.Vote == VoteValue.Other)?.Count ?? 0;

        return new ApiResponse<Response>(new Response { Willy = willy, Other = other });
    }

    public class ApiResponse<T> where T : class
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public class Response
    {
        public int Willy { get; set; }
        public int Other { get; set; }
    }
}
