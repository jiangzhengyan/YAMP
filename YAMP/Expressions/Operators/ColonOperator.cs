using System;

namespace YAMP
{
	class ColonOperator : Operator
	{
		public ColonOperator () : base(";", 1)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return new MatrixValue(left, right);
		}
	}
}
