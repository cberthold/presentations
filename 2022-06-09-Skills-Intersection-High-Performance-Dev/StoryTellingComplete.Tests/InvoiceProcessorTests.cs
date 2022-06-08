using Common;
using Common.Interfaces;
using ConsoleApp1;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace StoryTellingComplete.Tests
{
    public class InvoiceProcessorTests
    {
        #region mocks

        private readonly Mock<ICreateInvoice> createInvoiceMock = new Mock<ICreateInvoice>();
        private readonly Mock<ICalculateSubtotal> subTotalMock = new Mock<ICalculateSubtotal>();
        private readonly Mock<ICalculateTax> taxMock = new Mock<ICalculateTax>();
        private readonly Mock<ICalculateTotal> totalMock = new Mock<ICalculateTotal>();
        private readonly CalculatorStubs stub = new CalculatorStubs();

        private static readonly List<string> expectedMethodOrder = new List<string>
        {
            nameof(ICreateInvoice.CreateInvoice),
            nameof(ICalculateSubtotal.CalculateSubtotal),
            nameof(ICalculateTax.CalculateTax),
            nameof(ICalculateTotal.CalculateTotal),
        };

        #endregion


        [Fact]
        public void GivenAShoppingCartOrder_WhenCallingProcessOrder_ShouldCallCalculateMethodsInOrder_UsingMocks()
        {
            // assemble
            var order = ShoppingCart.GivenOrder();
            var methods = new List<string>();

            // setup mocks to capture calls
            createInvoiceMock.Setup(a => a.CreateInvoice(It.IsAny<InvoiceContext>()))
                .Callback(() => methods.Add(nameof(ICreateInvoice.CreateInvoice)));
            subTotalMock.Setup(a => a.CalculateSubtotal(It.IsAny<InvoiceContext>()))
                .Callback(() => methods.Add(nameof(ICalculateSubtotal.CalculateSubtotal)));
            taxMock.Setup(a => a.CalculateTax(It.IsAny<InvoiceContext>()))
                .Callback(() => methods.Add(nameof(ICalculateTax.CalculateTax)));
            totalMock.Setup(a => a.CalculateTotal(It.IsAny<InvoiceContext>()))
                .Callback(() => methods.Add(nameof(ICalculateTotal.CalculateTotal)));

            // subject under test should always be last assembled in most cases
            var processor = CreateInvoiceProcessorUsingMocks();


            // apply
            var invoice = processor.ProcessOrder(order);

            // assert
            Assert.Equal(expectedMethodOrder, methods);
        }

        [Fact]
        public void GivenAShoppingCartOrder_WhenCallingProcessOrder_ShouldCallCalculateMethodsInOrder_UsingStubs()
        {
            // assemble
            var order = ShoppingCart.GivenOrder();

            // subject under test should always be last assembled in most cases
            var processor = CreateInvoiceProcessorUsingStubs();


            // apply
            var invoice = processor.ProcessOrder(order);

            // assert
            Assert.Equal(expectedMethodOrder, stub.MethodsCalled);
        }

        #region Factory methods

        private InvoiceProcessor CreateInvoiceProcessorUsingMocks()
        {
            return new InvoiceProcessor(
                createInvoiceMock.Object,
                subTotalMock.Object,
                taxMock.Object,
                totalMock.Object);
        }

        private InvoiceProcessor CreateInvoiceProcessorUsingStubs()
        {
            return new InvoiceProcessor(
                stub,
                stub,
                stub,
                stub);
        }

        #endregion

    }


    public class CalculatorStubs : ICreateInvoice, ICalculateSubtotal, ICalculateTax, ICalculateTotal
    {
        public List<string> MethodsCalled { get; } = new List<string>();

        public Invoice Invoice { get; } = new Invoice();

        public void CalculateSubtotal(InvoiceContext context)
        {
            MethodsCalled.Add(nameof(CalculateSubtotal));
        }

        public void CalculateTax(InvoiceContext context)
        {
            MethodsCalled.Add(nameof(CalculateTax));
        }

        public void CalculateTotal(InvoiceContext context)
        {
            MethodsCalled.Add(nameof(CalculateTotal));
        }

        public void CreateInvoice(InvoiceContext context)
        {
            MethodsCalled.Add(nameof(CreateInvoice));
        }
    }
}
