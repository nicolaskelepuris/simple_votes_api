using Microsoft.AspNetCore.Mvc;
using VoteApi.Entities;

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
}
