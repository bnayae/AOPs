using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicAOP.DTOs
{
    class AopContext
    {
        public AopContext(string implementationName, string contractName, string methodName)
        {
            ImplementationName = implementationName;
            ContractName = contractName;
            MethodName = methodName;
        }

        public string ImplementationName { get; }
        public string ContractName { get; }
        public string MethodName { get; }
    }
}
