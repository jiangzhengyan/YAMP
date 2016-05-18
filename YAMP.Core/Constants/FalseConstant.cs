﻿namespace YAMP
{
    /// <summary>
    /// Gets the value for false.
    /// </summary>
    [Description("FalseConstantDescription")]
    [Kind(PopularKinds.Constant)]
    class FalseConstant : BaseConstant
    {
        public override Value Value
        {
            get { return ScalarValue.False; }
        }
    }
}
