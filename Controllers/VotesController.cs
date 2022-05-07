using Microsoft.AspNetCore.Mvc;
using VoteApi.Entities;
using Microsoft.EntityFrameworkCore;
using VoteApi.UseCases;

namespace VoteApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VotesController
{
    private readonly DataContext _dataContext;
    private readonly ILoadVotes _loadVotes;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VotesController(IHttpContextAccessor httpContextAccessor, DataContext dataContext, ILoadVotes loadVotes)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataContext = dataContext;
        _loadVotes = loadVotes;
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
    public async Task<ApiResponse<LoadVotesResponse>> GetVotes()
    {
        var votes = await _loadVotes.LoadAsync();

        return new ApiResponse<LoadVotesResponse>(votes);
    }

    public class ApiResponse<T> where T : class
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
