using System;

namespace StandardizedQR.Validation
{
    /// <summary>
    /// Defines metadata for the EMV(R) Co QR Code Specification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EmvSpecificationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmvSpecificationAttribute"/> attribute with a unique identifier.
        /// </summary>
        /// <param name="id">The value to set in the <see cref="Id"/> property.</param>
        public EmvSpecificationAttribute(int id)
        {
            Id = id;
        }

        /// <summary>
        /// The ID is coded as a two-digit numeric value, with a value ranging from "00" to "99".
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The length is coded as a two-digit numeric value, with a value ranging from "01" to "99".
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property represents a rood node.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is parent; otherwise, <c>false</c>.
        /// </value>
        public bool IsParent { get; set; }
    }
}
