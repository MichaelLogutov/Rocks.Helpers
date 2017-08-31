using System.Data;
using System.Globalization;

namespace Rocks.Helpers.Internal
{
    /// <summary>
    /// Port from .NET Framework
    /// </summary>
    internal static class DbProviderFactoriesConfigurationHandler
    {
        internal static DataTable CreateProviderDataTable()
        {
            var dataColumn1 = new DataColumn("Name", typeof(string)) {ReadOnly = true};
            var dataColumn2 = new DataColumn("Description", typeof(string)) {ReadOnly = true};
            var dataColumn3 = new DataColumn("InvariantName", typeof(string)) {ReadOnly = true};
            var dataColumn4 = new DataColumn("AssemblyQualifiedName", typeof(string)) {ReadOnly = true};
            var dataColumnArray = new[]
            {
                dataColumn3
            };
            var columns = new[]
            {
                dataColumn1,
                dataColumn2,
                dataColumn3,
                dataColumn4
            };
            var dataTable = new DataTable("DbProviderFactories")
            {
                Locale = CultureInfo.InvariantCulture,
                PrimaryKey = dataColumnArray
            };
            dataTable.Columns.AddRange(columns);
            return dataTable;
        }
    }
}