using Microsoft.AspNetCore.Mvc;
using VoteApi.Entities;
using VoteApi.UseCases;

namespace VoteApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VotesController
{
    private readonly ISaveVote _saveVote;
    private readonly ILoadVotes _loadVotes;

    public VotesController(ISaveVote saveVote, ILoadVotes loadVotes)
    {
        _saveVote = saveVote;
        _loadVotes = loadVotes;
    }

    [HttpPost("{voteValue}")]
    public async Task Vote([FromRoute] VoteValue voteValue)
    {
        await _saveVote.VoteAsync(voteValue);
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
