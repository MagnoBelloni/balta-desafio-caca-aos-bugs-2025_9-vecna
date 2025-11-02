using AutoFixture;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Tests.Data.Repositories
{
    public class BaseRepositoryTests
    {
        private readonly Fixture fixture;
        private readonly AppDbContext appDbContext;
        private readonly BaseRepositoryHelper baseRepository;

        public BaseRepositoryTests()
        {
            fixture = new Fixture();

            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            appDbContext = new AppDbContext(options);
            baseRepository = new BaseRepositoryHelper(appDbContext);
        }

        [Fact]
        public async Task GetOneAsNoTrackingAsync_Should_ReturnEntity()
        {
            var entityName = "Fulano";

            var entity = fixture.Build<Customer>()
                .With(x => x.Name, entityName)
                .Create();

            await baseRepository.AddAsync(entity, CancellationToken.None);

            var result = await baseRepository.GetOneAsNoTrackingAsync(x => x.Name == entityName, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(entityName);
        }

        [Fact]
        public async Task GetOneAsNoTrackingAsync_Should_ReturnNull_When_EntityNotFound()
        {
            var entityName = "Fulano";

            var result = await baseRepository.GetOneAsNoTrackingAsync(x => x.Name == entityName, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPagedAsync_Should_ReturnEntitiesPaginated()
        {
            var entityName = "Fulano";
            var expectedNumberOfEntities = 5;

            var entities = fixture.Build<Customer>()
                .With(x => x.Name, entityName)
                .CreateMany(expectedNumberOfEntities);

            foreach (var entity in entities)
            {
                await baseRepository.AddAsync(entity, CancellationToken.None);
            }

            IOrderedQueryable<Customer> OrderBy(IQueryable<Customer> q) => q.OrderByDescending(c => c.Name);

            var result = await baseRepository.GetPagedAsync(_ => true, 1, expectedNumberOfEntities, OrderBy, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().NotBeEmpty();
            result.TotalCount.Should().Be(expectedNumberOfEntities);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnEntities()
        {
            var entityName = "Fulano";
            var expectedNumberOfEntities = 5;

            var entities = fixture.Build<Customer>()
                .With(x => x.Name, entityName)
                .CreateMany(expectedNumberOfEntities);

            foreach (var entity in entities)
            {
                await baseRepository.AddAsync(entity, CancellationToken.None);
            }

            var result = await baseRepository.GetAllAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Count().Should().Be(expectedNumberOfEntities);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnEmptyList_When_NoEntitiesFound()
        {
            var result = await baseRepository.GetAllAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsyncWithPredicate_Should_ReturnEntities()
        {
            var entityName = "Fulano";
            var expectedNumberOfEntities = 5;

            var entities = fixture.Build<Customer>()
                .With(x => x.Name, entityName)
                .CreateMany(expectedNumberOfEntities);

            foreach (var entity in entities)
            {
                await baseRepository.AddAsync(entity, CancellationToken.None);
            }

            var result = await baseRepository.GetAllAsync(_ => true, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Count().Should().Be(expectedNumberOfEntities);
        }

        [Fact]
        public async Task GetAllAsyncWithPredicate_Should_ReturnEmptyList_When_NoEntitiesFound()
        {
            var result = await baseRepository.GetAllAsync(_ => true, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnEntity()
        {
            var entityId = Guid.NewGuid();
            var entityName = "Fulano";

            var entity = fixture.Build<Customer>()
                .With(x => x.Id, entityId)
                .With(x => x.Name, entityName)
                .Create();

            await baseRepository.AddAsync(entity, CancellationToken.None);

            var result = await baseRepository.GetByIdAsync(entityId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(entityName);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_When_EntityNotFound()
        {
            var result = await baseRepository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsNoTrackingAsync_Should_ReturnEntity()
        {
            var entityId = Guid.NewGuid();
            var entityName = "Fulano";

            var entity = fixture.Build<Customer>()
                .With(x => x.Id, entityId)
                .With(x => x.Name, entityName)
                .Create();

            await baseRepository.AddAsync(entity, CancellationToken.None);

            var result = await baseRepository.GetByIdAsNoTrackingAsync(entityId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be(entityName);
        }

        [Fact]
        public async Task GetByIdAsNoTrackingAsync_Should_ReturnNull_When_EntityNotFound()
        {
            var result = await baseRepository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_Should_AddEntity()
        {
            var entityId = Guid.NewGuid();

            var entity = fixture.Build<Customer>()
                .With(x => x.Id, entityId)
                .Create();

            await baseRepository.AddAsync(entity, CancellationToken.None);

            var result = await baseRepository.GetByIdAsNoTrackingAsync(entityId, CancellationToken.None);
            result.Should().NotBeNull();
            result.Id.Should().Be(entityId);
        }

        [Fact]
        public async Task UpdateAsync_Should_AddEntity()
        {
            var entityId = Guid.NewGuid();
            var name = "Fulano";
            var expectedUpdatedName = "Ciclano";

            var entity = fixture.Build<Customer>()
                .With(x => x.Id, entityId)
                .With(x => x.Name, name)
                .Create();

            await baseRepository.AddAsync(entity, CancellationToken.None);

            var entityToUpdate = await baseRepository.GetByIdAsync(entityId, CancellationToken.None);
            entityToUpdate!.Name = expectedUpdatedName;

            await baseRepository.UpdateAsync(entityToUpdate, CancellationToken.None);

            var result = await baseRepository.GetByIdAsNoTrackingAsync(entityId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(entityId);
            result.Name.Should().Be(expectedUpdatedName);
        }

        [Fact]
        public async Task DeleteAsync_Should_AddEntity()
        {
            var entityId = Guid.NewGuid();

            var entity = fixture.Build<Customer>()
                .With(x => x.Id, entityId)
                .Create();

            await baseRepository.AddAsync(entity, CancellationToken.None);
            await baseRepository.DeleteAsync(entityId, CancellationToken.None);

            var result = await baseRepository.GetByIdAsNoTrackingAsync(entityId, CancellationToken.None);
            result.Should().BeNull();
        }
    }
}
