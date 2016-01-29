using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This class evaluates and creates number 012345 expressions.
    /// </summary>
	class NumberExpression : Expression
    {
        #region Fields

        ScalarValue value;

        #endregion

        #region ctor

        public NumberExpression()
		{
		}

        public NumberExpression(ScalarValue content)
        {
            value = content;
        }

		public NumberExpression(ParseEngine engine) : base(engine)
		{
		}

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
		{
            return value.Clone();
		}

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var chars = engine.Characters;
            var ch = chars[start];
            var isreal = false;

            if(ParseEngine.IsNumber(ch) || (ch == '.' && start < chars.Length && ParseEngine.IsNumber(chars[start + 1])))
            {
                var index = start;
                var exp = new NumberExpression(engine);
                var number = 0.0;
                var pow = 0;
                
                if (ch != '.') 
                {
                    number += ToDoubleNumber(chars[index++]);

                    while (index < chars.Length && ParseEngine.IsNumber(ch = chars[index]))
                    {
                        number *= 10.0;
                        number += ToDoubleNumber(ch);
                        index++;
                    }
                }
                
                if (ch == '.')
                {
                    isreal = true;
                    index++;

                    if (index < chars.Length && ParseEngine.IsNumber(chars[index]))
                    {
                        do
                        {
                            number *= 10.0;
                            number += ToDoubleNumber(chars[index++]);
                            pow++;
                        }
                        while (index < chars.Length && ParseEngine.IsNumber(ch = chars[index]));
                    }
                }

                if (ch == 'e' || ch == 'E')
                {
                    isreal = true;
                    var epow = 0;
                    var sign = 1;
                    index++;

                    if (index < chars.Length && (chars[index] == '+' || chars[index] == '-'))
                    {
                        sign = chars[index] == '-' ? -1 : +1;
                        index++;
                    }

                    while (index < chars.Length && ParseEngine.IsNumber(ch = chars[index]))
                    {
                        epow *= 10;
                        epow += ToInt32Number(ch);
                        index++;
                    }

                    pow -= epow * sign;
                }

                var value = number / Math.Pow(10.0, pow);

                if (ch == 'i')
                {
                    exp.value = new ScalarValue(0.0, value);
                    index++;
                }
                else if (isreal)
                    exp.value = new ScalarValue(value);
                else
                    exp.value = new ScalarValue((int)value);

                exp.Length = index - start;
                engine.SetPointer(index);
                return exp;
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            if(value.Re != 0.0)
                return value.Re.ToString().Replace(',', '.');

            return value.Im.ToString().Replace(',', '.') + "i";
        }

        #endregion

        #region Helpers

        static double ToDoubleNumber(char p)
        {
            return (double)p - 48.0;
        }

        static int ToInt32Number(char p)
        {
            return (int)p - 48;
        }

        #endregion
    }
}

