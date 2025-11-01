namespace BugStore.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler =
                    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            return services;
        }
    }
}
