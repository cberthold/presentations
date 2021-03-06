// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

namespace TaxApp.Data.TaxesDb
{

    // SalesOrderLineItem
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.20.1.0")]
    public partial class SalesOrderLineItem
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public System.Guid SalesOrderId { get; set; } // SalesOrderId
        public int Quantity { get; set; } // Quantity
        public decimal Price { get; set; } // Price
        public decimal LineTotal { get; set; } // LineTotal
        public string Product { get; set; } // Product (length: 200)

        // Foreign keys
        public virtual SalesOrder SalesOrder { get; set; } // FK_SalesOrderLineItem_SalesOrder

        public SalesOrderLineItem()
        {
            Quantity = 0;
            Price = 0m;
            LineTotal = 0m;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
