using Common;
using Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PatternsComplete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var givenOrder = ShoppingCart.GivenOrder();
            Invoice invoice = default;

            //var processor = CreateProcessor();
            var processor = CreateProcessorDependencyInjection();
            invoice = processor.ProcessOrder(givenOrder);
            Console.WriteLine($"SubTotal: {invoice.SubTotal}");
            Console.WriteLine($"Tax: {invoice.Tax}");
            Console.WriteLine($"Total: {invoice.Total}");
        }

        private static InvoiceProcessor CreateProcessor()
        {
            var createInvoiceService = new InvoiceCreator();
            var subTotalService = new SubtotalCalculator();
            var taxService = new BasicTaxCalculator();
            var totalService = new TotalCalculator();
            var processor = new InvoiceProcessor(createInvoiceService, subTotalService, taxService, totalService);
            return processor;
        }

        private static InvoiceProcessor CreateProcessorDependencyInjection()
        {
            var services = new ServiceCollection();

            //var createInvoiceService = new InvoiceCreator();
            services.AddTransient<ICreateInvoice, InvoiceCreator>();
            //var subTotalService = new SubtotalCalculator();
            services.AddTransient<ICalculateSubtotal, SubtotalCalculator>();
            //var taxService = new BasicTaxCalculator();
            services.AddTransient<ICalculateTax>(svc =>
            {
                var basic = new BasicTaxCalculator();
                var taxFree = new ProductATaxFreeTaxCalculator();
                var composite = new CompositeTaxCalculator(new ICalculateTax[] { basic, taxFree });
                return composite;
            });
            //var totalService = new TotalCalculator();
            services.AddTransient<ICalculateTotal, TotalCalculator>();

            //var processor = new InvoiceProcessor(createInvoiceService, subTotalService, taxService, totalService);
            services.AddTransient<InvoiceProcessor>();
            var sp = services.BuildServiceProvider();
            var processor = sp.GetRequiredService<InvoiceProcessor>();
            return processor;
        }
    }

    public class InvoiceProcessor
    {
        private readonly ICreateInvoice createInvoiceService;
        private readonly ICalculateSubtotal subTotalService;
        private readonly ICalculateTax taxService;
        private readonly ICalculateTotal totalService;

        public InvoiceProcessor(
            ICreateInvoice createInvoiceService,
            ICalculateSubtotal subTotalService,
            ICalculateTax taxService,
            ICalculateTotal totalService)
        {
            this.createInvoiceService = createInvoiceService;
            this.subTotalService = subTotalService;
            this.taxService = taxService;
            this.totalService = totalService;
        }

        public Invoice ProcessOrder(ShoppingCart cart)
        {
            // we create a context to store the interesting parts of our story
            var context = new InvoiceContext();

            // the first major component is the cart
            context.Cart = cart;

            // our story has 4 major steps to it
            // 1. Create Invoice
            CreateInvoice(context);
            // 2. Calculate Subtotal
            CalculateSubtotal(context);
            // 3. Calculate Tax
            CalculateTax(context);
            // 4. Calculate Total
            CalculateTotal(context);

            return context.Invoice;
        }

        private void CreateInvoice(InvoiceContext context)
        {
            createInvoiceService.CreateInvoice(context);
        }

        private void CalculateSubtotal(InvoiceContext context)
        {
            subTotalService.CalculateSubtotal(context);
        }

        private void CalculateTax(InvoiceContext context)
        {
            taxService.CalculateTax(context);
        }

        private void CalculateTotal(InvoiceContext context)
        {
            totalService.CalculateTotal(context);
        }
    }

}
