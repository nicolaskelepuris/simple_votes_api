namespace VoteApi.Entities;

public enum VoteValue
{
    Willy,
    Other
}

public class Vote
{
    public Vote(string id, VoteValue value)
    {
        Id = id;
        Value = value;
    }

    private Vote()
    {
    }

    public string Id { get; private set; }
    public VoteValue Value { get; private set; }
}
