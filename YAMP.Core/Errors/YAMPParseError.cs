﻿namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// Any error during parsing will be noted as an instance of this class.
    /// </summary>
    public abstract class YAMPParseError
    {
        #region ctor

        /// <summary>
        /// Creates a new parse error.
        /// </summary>
        /// <param name="line">The line of the error.</param>
        /// <param name="column">The column of the error.</param>
        /// <param name="message">The message for the error.</param>
        /// <param name="args">The arguments for formatting the message.</param>
        public YAMPParseError(Int32 line, Int32 column, String message, params Object[] args)
        {
            Line = line;
            Column = column;
            Message = string.Format(message, args);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message for this error.
        /// </summary>
        public String Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the line for this error.
        /// </summary>
        public Int32 Line
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the column for this error.
        /// </summary>
        public Int32 Column
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the block responsible for the parse error.
        /// </summary>
        public Block Part
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the length of the error in characters.
        /// </summary>
        public Int32 Length
        {
            get
            {
                if (Part != null)
                {
                    return Part.Length;
                }

                return 1;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts to error to a string.
        /// </summary>
        /// <returns>The string with the error.</returns>
        public override String ToString()
        {
            return String.Format("Line {0:000}, Pos. {1:000} : {2}", Line, Column, Message);
        }

        #endregion
    }
}
