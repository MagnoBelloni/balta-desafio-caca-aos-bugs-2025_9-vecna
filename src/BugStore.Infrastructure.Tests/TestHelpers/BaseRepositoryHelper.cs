using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Data.Repositories;

namespace BugStore.Infrastructure.Tests.TestHelpers
{
    /// <summary>
    /// Essa classe foi criada apenas para ajudar a testar uma classe abstrata
    /// </summary>
    /// <param name="context">DbContext do Entity Framework</param>
    public class BaseRepositoryHelper(AppDbContext context) : BaseRepository<Customer>(context);
}
