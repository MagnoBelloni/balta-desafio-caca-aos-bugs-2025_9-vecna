using AutoFixture;
using BugStore.Domain.Entities;
using FluentAssertions;

namespace BugStore.Domain.Tests.Entities
{
    public class OrderTests
    {
        private readonly Fixture fixture;

        public OrderTests()
        {

            fixture = new Fixture();
        }

        [Fact]
        public void TotalAmount_Should_BeCalculatedBaseOnOrderLinesTotal()
        {
            var productQuantity1 = 5;
            var priceProduct1 = 5.99M;

            var product1 = fixture.Build<Product>()
                .With(x => x.Price, priceProduct1)
                .Create();

            var orderLine1 = new OrderLine(productQuantity1, Guid.NewGuid(), product1);
            var expectedTotal = productQuantity1 * priceProduct1;

            var productQuantity2 = 15;
            var priceProduct2 = 2.50M;
            var product2 = fixture.Build<Product>()
                .With(x => x.Price, priceProduct2)
                .Create();

            var orderLine2 = new OrderLine(productQuantity2, Guid.NewGuid(), product2);
            expectedTotal += productQuantity2 * priceProduct2;

            var order = new Order(Guid.NewGuid(), [orderLine1, orderLine2]);

            order.TotalAmount.Should().Be(expectedTotal);
        }
    }
}
