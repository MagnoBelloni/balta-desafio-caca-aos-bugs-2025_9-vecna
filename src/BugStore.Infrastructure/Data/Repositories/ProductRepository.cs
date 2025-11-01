using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;

namespace BugStore.Infrastructure.Data.Repositories;

public class ProductRepository(AppDbContext context) : BaseRepository<Product>(context), IProductRepository;
