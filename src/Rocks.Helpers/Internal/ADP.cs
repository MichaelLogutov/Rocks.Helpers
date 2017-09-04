using System;
using System.Configuration;

namespace Rocks.Helpers.Internal
{
    /// <summary>
    /// Port from .NET Framework
    /// </summary>
    internal static class ADP
    {
        internal static ArgumentException Argument(string error)
        {
            var argumentException = new ArgumentException(error);
            return argumentException;
        }
        
        internal static ArgumentNullException ArgumentNull(string parameter)
        {
            var argumentNullException = new ArgumentNullException(parameter);
            return argumentNullException;
        }
        
        internal static ConfigurationException Configuration(string message)
        {
            var configurationException = (ConfigurationException) new ConfigurationErrorsException(message);
            return configurationException;
        }
        
        internal static InvalidOperationException InvalidOperation(string error)
        {
            var operationException = new InvalidOperationException(error);
            return operationException;
        }
        
        internal static bool IsEmpty(string str)
        {
            if (str != null)
            {
                return str.Length == 0;
            }
            return true;
        }
        
        internal static void CheckArgumentLength(string value, string parameterName)
        {
            CheckArgumentNull(value, parameterName);
            if (value.Length == 0)
            {
                throw Argument("String cannot be empty: " + parameterName);
            }
        }
        
        internal static void CheckArgumentNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw ArgumentNull(parameterName);
            }
        }
        
        internal static ArgumentException ConfigProviderNotFound()
        {
            return Argument("Provider not found");
        }
        
        internal static InvalidOperationException ConfigProviderInvalid()
        {
            return InvalidOperation("Invalid provider");
        }
        
        internal static ConfigurationException ConfigProviderNotInstalled()
        {
            return Configuration("Provider not installed");
        }
        
        internal static ConfigurationException ConfigProviderMissing()
        {
            return Configuration("Provider is missing");
        }
    }
}