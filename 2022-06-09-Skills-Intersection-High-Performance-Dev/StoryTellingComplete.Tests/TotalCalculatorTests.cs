using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoryTellingComplete.Tests
{
    public class TotalCalculatorTests
    {
        [Fact]
        public void GivenInvoiceSubTotalAndTax_WhenCalculateTotalIsCalled_ShouldMatchExpectedTotal()
        {
            // assemble

            var context = new InvoiceContext()
            {
                Invoice = new ConsoleApp1.Invoice
                {
                    SubTotal = 100,
                    Tax = 10,
                }
            };

            const decimal EXPECTED_TOTAL = 110m;
            
            // subject under test should always be last assembled in most cases
            var calculator = CreateCalculator();

            // apply
            calculator.CalculateTotal(context);

            // assert
            Assert.Equal(EXPECTED_TOTAL, context.Invoice.Total);
        }

        #region factory method

        private static TotalCalculator CreateCalculator()
        {
            return new TotalCalculator();
        }

        #endregion
    }
}
