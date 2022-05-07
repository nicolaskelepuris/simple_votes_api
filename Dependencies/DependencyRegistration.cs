using VoteApi.UseCases;

namespace VoteApi.Dependencies
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ISaveVote, SaveVote>();
            services.AddScoped<ILoadVotes, LoadVotes>();

            return services;
        }
    }
}