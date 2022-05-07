using Microsoft.EntityFrameworkCore;
using VoteApi.Entities;

namespace VoteApi.UseCases;

public interface ILoadVotes
{
    Task<LoadVotesResponse> LoadAsync();
}

public class LoadVotesResponse
{
    public int Willy { get; set; }
    public int Other { get; set; }
}

public class LoadVotes : ILoadVotes
{
    private readonly DataContext _dataContext;

    public LoadVotes(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<LoadVotesResponse> LoadAsync()
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

        return new LoadVotesResponse { Willy = willy, Other = other };
    }
}
