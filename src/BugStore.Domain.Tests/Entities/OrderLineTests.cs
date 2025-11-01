using AutoFixture;
using BugStore.Domain.Entities;
using FluentAssertions;

namespace BugStore.Domain.Tests.Entities
{
    public class OrderLineTests
    {
        private readonly Fixture fixture;

        public OrderLineTests()
        {

            fixture = new Fixture();
        }

        [Fact]
        public void Total_ShouldCalculateBasedOnQuantity()
        {
            var quantity = 5;
            var price = 5.99M;

            var product = fixture.Build<Product>()
                .With(x => x.Price, price)
                .Create();

            var orderLine = new OrderLine(quantity, Guid.NewGuid(), product);
            orderLine.Total.Should().Be(quantity * price);
        }
    }
}
