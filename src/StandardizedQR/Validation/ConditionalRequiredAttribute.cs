using System;

namespace StandardizedQR.Validation
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ConditionalRequiredAttribute : Attribute
    {
        public ConditionalRequiredAttribute(string dependsOnPropertyName)
        {
            DependsOnPropertyName = dependsOnPropertyName;
        }

        public string DependsOnPropertyName { get; }
    }
}
