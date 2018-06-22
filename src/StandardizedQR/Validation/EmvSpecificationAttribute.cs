using System;

namespace StandardizedQR.Validation
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EmvSpecificationAttribute : Attribute
    {
        public EmvSpecificationAttribute(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public int Length { get; set; }
    }
}
