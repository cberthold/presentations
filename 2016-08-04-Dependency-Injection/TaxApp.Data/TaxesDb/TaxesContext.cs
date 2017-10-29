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

    using System.Linq;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.20.1.0")]
    public partial class TaxesContext : System.Data.Entity.DbContext, ITaxesContext
    {
        public System.Data.Entity.DbSet<Address> Addresses { get; set; } // Address
        public System.Data.Entity.DbSet<SalesOrder> SalesOrders { get; set; } // SalesOrder
        public System.Data.Entity.DbSet<SalesOrderLineItem> SalesOrderLineItems { get; set; } // SalesOrderLineItem
        public System.Data.Entity.DbSet<StateTax> StateTaxes { get; set; } // StateTax

        static TaxesContext()
        {
            System.Data.Entity.Database.SetInitializer<TaxesContext>(null);
        }

        public TaxesContext()
            : base("Name=Taxes")
        {
            InitializePartial();
        }

        public TaxesContext(string connectionString)
            : base(connectionString)
        {
            InitializePartial();
        }

        public TaxesContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
            InitializePartial();
        }

        public TaxesContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializePartial();
        }

        public TaxesContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializePartial();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool IsSqlParameterNull(System.Data.SqlClient.SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as System.Data.SqlTypes.INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == System.DBNull.Value);
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new SalesOrderConfiguration());
            modelBuilder.Configurations.Add(new SalesOrderLineItemConfiguration());
            modelBuilder.Configurations.Add(new StateTaxConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AddressConfiguration(schema));
            modelBuilder.Configurations.Add(new SalesOrderConfiguration(schema));
            modelBuilder.Configurations.Add(new SalesOrderLineItemConfiguration(schema));
            modelBuilder.Configurations.Add(new StateTaxConfiguration(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(System.Data.Entity.DbModelBuilder modelBuilder);
    }
}
// </auto-generated>