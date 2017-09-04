using System;

namespace Rocks.Helpers.Internal
{
    /// <summary>
    /// Port from .NET Framework
    /// </summary>
    internal class DbProviderFactoryConfigSection
    {
        private Type factType;
        private string name;
        private string invariantName;
        private string description;
        private string assemblyQualifiedName;

        public string Name
        {
            get { return this.name; }
        }

        public string InvariantName
        {
            get { return this.invariantName; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public string AssemblyQualifiedName
        {
            get { return this.assemblyQualifiedName; }
        }

        public DbProviderFactoryConfigSection(Type FactoryType, string FactoryName, string FactoryDescription)
        {
            try
            {
                this.factType = FactoryType;
                this.name = FactoryName;
                this.invariantName = this.factType.Namespace.ToString();
                this.description = FactoryDescription;
                this.assemblyQualifiedName = this.factType.AssemblyQualifiedName.ToString();
            }
            catch
            {
                this.factType = (Type) null;
                this.name = string.Empty;
                this.invariantName = string.Empty;
                this.description = string.Empty;
                this.assemblyQualifiedName = string.Empty;
            }
        }

        public DbProviderFactoryConfigSection(string FactoryName, string FactoryInvariantName,
            string FactoryDescription, string FactoryAssemblyQualifiedName)
        {
            this.factType = (Type) null;
            this.name = FactoryName;
            this.invariantName = FactoryInvariantName;
            this.description = FactoryDescription;
            this.assemblyQualifiedName = FactoryAssemblyQualifiedName;
        }

        public bool IsNull()
        {
            return this.factType == (Type) null && this.invariantName == string.Empty;
        }
    }
}