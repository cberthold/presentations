using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryTellingComplete.Tests
{
    public class BasicTaxCalculatorTests
    {
        [Fact]
        public void GivenInvoiceSubTotal_WhenCalculateTaxIsCalled_ShouldMatchExpectedTax()
        {
            // assemble

            var context = new InvoiceContext()
            {
                Invoice = new ConsoleApp1.Invoice
                {
                    SubTotal = 100,
                }
            };

            const decimal EXPECTED_TAX = 6m;
            
            // subject under test should always be last assembled in most cases
            var calculator = CreateCalculator();

            // apply
            calculator.CalculateTax(context);

            // assert
            Assert.Equal(EXPECTED_TAX, context.Invoice.Tax);
        }

        #region factory method

        private static BasicTaxCalculator CreateCalculator()
        {
            return new BasicTaxCalculator();
        }

        #endregion
    }
}
