using System;
using System.Collections.Generic;

namespace StandardizedQR.Utils
{
    /// <summary>
    /// Defines a Tag/Length/Value combo
    /// </summary>
    internal class Tlv
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tlv"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="length">The length.</param>
        /// <param name="value">The value.</param>
        public Tlv(string tag, int length, string value)
        {
            Tag = Int32.Parse(tag);
            Length = length;
            Value = value;

            ChildNodes = new HashSet<Tlv>();
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public int Tag { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public ICollection<Tlv> ChildNodes { get; set; }
    }
}
