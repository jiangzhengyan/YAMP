namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    /// <summary>
    /// The class for representing a string value.
    /// </summary>
	public sealed class StringValue : Value, IFunction
	{
		#region Fields

		readonly String _value;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public StringValue()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance and sets the value.
        /// </summary>
        /// <param name="value">The string where this value is based on.</param>
        public StringValue(String value)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="str">The given character array.</param>
        public StringValue(Char[] str)
            : this(new String(str))
        {
        }

        #endregion

		#region Properties

        /// <summary>
        /// Gets the string value.
        /// </summary>
		public String Value
		{
			get { return _value; }
		}

        /// <summary>
        /// Gets the length of string value.
        /// </summary>
		public Int32 Length
		{
			get { return _value.Length; }
		}

        /// <summary>
        /// Gets the number of lines in the string value.
        /// </summary>
        public Int32 Lines
		{
			get { return _value.Split('\n').Length; }
		}

		#endregion

        #region Register Operator

        /// <summary>
        /// Registers the allowed operations.
        /// </summary>
        protected override void RegisterOperators()
        {
            RegisterPlus(typeof(StringValue), typeof(Value), Add);
            RegisterPlus(typeof(Value), typeof(StringValue), Add);
        }

        /// <summary>
        /// Performs the addition str + x or x + str.
        /// </summary>
        /// <param name="left">An arbitrary value.</param>
        /// <param name="right">Another arbitrary value.</param>
        /// <returns>The result of the operation.</returns>
        public static StringValue Add(Value left, Value right)
        {
            return new StringValue(left.ToString() + right.ToString());
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Returns a copy of this string value instance.
        /// </summary>
        /// <returns>The cloned string value.</returns>
        public override Value Copy()
        {
            return new StringValue(_value);
        }

        /// <summary>
        /// Converts the given value into binary data.
        /// </summary>
        /// <returns>The bytes array containing the data.</returns>
        public override Byte[] Serialize()
		{
            using (var ms = Serializer.Create())
            {
                ms.Serialize(_value);
                return ms.Value;
            }
		}

        /// <summary>
        /// Creates a new string value from the binary content.
        /// </summary>
        /// <param name="content">The data which contains the content.</param>
        /// <returns>The new instance.</returns>
		public override Value Deserialize(Byte[] content)
		{
            var value = String.Empty;

            using (var ds = Deserializer.Create(content))
            {
                value = ds.GetString();
            }

			return new StringValue(value);
		}

        #endregion

        #region Conversions

        /// <summary>
        /// Explicit conversion from a string to a scalar.
        /// </summary>
        /// <param name="value">The stringvalue that will be casted.</param>
        /// <returns>The scalar with Re = sum over all characters and Im = length of the string.</returns>
        public static explicit operator ScalarValue(StringValue value)
        {
            var sum = 0.0;

            foreach (var c in value.Value)
            {
                sum += (Double)c;
            }

            return new ScalarValue(sum, value.Length);
        }

        #endregion

        #region Comparison
        /// <summary>
        /// Is the given object equal to this.
        /// </summary>
        /// <param name="obj">The compare object.</param>
        /// <returns>A boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is StringValue)
            {
                var sv = obj as StringValue;
                return sv._value == _value;
            }

            if (obj is string || obj == null)
                return (string)obj == _value;

            return false;
        }

        /// <summary>
        /// Computes the hashcode of the value inside.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return _value == null ? 0 : _value.GetHashCode();
        }
        #endregion

        #region Index

        /// <summary>
        /// Gets the 1-based character of the string.
        /// </summary>
        /// <param name="index">The 1-based character (1 == first character) index.</param>
        /// <returns>The character at the position.</returns>
        public Char this[Int32 index]
        {
            get
            {
                if (index < 1 || index > _value.Length)
                {
                    throw new ArgumentOutOfRangeException("Access in string out of bounds.");
                }

                return _value[index - 1];
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the string content of this instance.
        /// </summary>
        /// <param name="context">The context of the invocation.</param>
        /// <returns>The value of the string.</returns>
        public override String ToString (ParseContext context)
		{
			return _value;
		}

		#endregion

        #region Functional behavior

        /// <summary>
        /// If invoked like a function the function reacts like this.
        /// </summary>
        /// <param name="context">The context of invocation.</param>
        /// <param name="argument">The argument(s) that have been given.</param>
        /// <returns>The subset of the string.</returns>
        public Value Perform(ParseContext context, Value argument)
        {
            if (argument is NumericValue)
            {
                var idx = BuildIndex(argument, Length);
                var str = new Char[idx.Length];

                for (var i = 0; i < idx.Length; i++)
                {
                    str[i] = _value[idx[i]];
                }

                return new StringValue(str);
            }

            throw new YAMPArgumentWrongTypeException(argument.Header, new [] { "Matrix", "Scalar" }, "String");
        }

        #endregion
    }
}

