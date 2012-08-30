using System;

namespace YAMP
{
	public class ScalarValue : Value
	{
		const double epsilon = 1e-12;
		
		double _real;
		double _imag;
		
		public ScalarValue () : this(0.0)
		{
		}
		
		public ScalarValue(double real) : this(real, 0.0)
		{
		}
		
		public ScalarValue(ScalarValue value) : this(value._real, value._imag)
		{
		}
		
		public ScalarValue(double real, double imag)
		{
			_real = real;
			_imag = imag;
		}
		
		public double Value 
		{
			get { return _real; }
			set { _real = value; }
		}
		
		public double ImaginaryValue 
		{
			get { return _imag; }
			set { _imag = value; }
		}
		
		public ScalarValue Clone()
		{
			return new ScalarValue(this);
		}
		
		public static ScalarValue I
		{
			get { return new ScalarValue(0.0, 1.0); }
		}
		
		public ScalarValue Abs()
		{
            _real = abs();
            _imag = 0.0;
			return this;
		}
		
		public ScalarValue Conjugate()
		{
            _imag *= -1.0;
			return this;
		}
		
		public override Value Add (Value right)
		{
			if(right is ScalarValue)
			{
				var r = right as ScalarValue;
                _real = _real + r._real;
                _imag = _imag + r._imag;
                return this;
            }
			
			throw new OperationNotSupportedException("+", right);
		}
		
		public override Value Subtract (Value right)
		{
			if(right is ScalarValue)
			{
				var r = right as ScalarValue;
                _real = _real - r._real;
                _imag = _imag - r._imag;
                return this;
            }
			
			throw new OperationNotSupportedException("-", right);
		}
		
		public override Value Multiply (Value right)
		{
			if(right is ScalarValue)
			{
				var r = right as ScalarValue;
				var re = _real * r._real - _imag * r._imag;
				_imag = _real * r._imag + _imag * r._real;
                _real = re;
                return this;
			}
			else if(right is VectorValue)
			{
				var r = right as VectorValue;
				return r.Multiply(this);
			}
			
			throw new OperationNotSupportedException("*", right);
		}
		
		public override Value Divide (Value right)
		{
			if(right is ScalarValue)
			{
				var r = (right as ScalarValue).Conjugate();
                var q = r._real * r._real + r._imag * r._imag;
                var re = _real * r._real - _imag * r._imag;
                _imag = _real * r._imag + _imag * r._real;
                _real = re / q;
				_imag = _imag / q;
				return this;
			}
			
			throw new OperationNotSupportedException("/", right);
		}
		
		public override Value Power (Value exponent)
		{
			if(exponent is ScalarValue)
			{
				var exp = exponent as ScalarValue;
				var theta = Math.Atan(_imag / _real);
				var L = _real / Math.Cos(theta);
				var phi = Ln();
                phi._real *= exp._imag;
                phi._imag *= exp._imag;
				var R = (I.Multiply(phi) as ScalarValue).Exp();
				var alpha = Math.Pow(L, exp._real);
				var beta = theta * exp._real;
                _real = Math.Cos(beta);
                _imag = Math.Sin(beta);
                var re = _real * R._real - _imag * R._imag;
                _imag = alpha * (_real * R._imag + _imag * R._real);
                _real = alpha * re;
				return this;
			}
			
			throw new OperationNotSupportedException("^", exponent);
		}
		
		public ScalarValue Cos()
		{
			var re = Math.Cos(_real) * Math.Cosh(_imag);
			var im = -Math.Sin(_real) * Math.Sinh(_imag);
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Sin()
		{
			var re = Math.Sin(_real) * Math.Cosh(_imag);
			var im = Math.Cos(_real) * Math.Sinh(_imag);
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Exp()
		{
			var f = Math.Exp(_real);
			var re = f * Math.Cos(_imag);
			var im = f * Math.Sin(_imag);
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Ln()
		{
			var re = Math.Log(abs());
			var im = arg();
			return new ScalarValue(re, im);
		}
		
		public override Value Faculty()
		{
			var re = faculty(_real);
			var im = faculty(_imag);
			return new ScalarValue(re, im);
		}
		
		double faculty(double r)
		{
			var k = (int)Math.Abs(r);
			var value = Math.Sign(r);
			
			while(k > 1)
			{
				value *= k;
				k--;
			}
			
			return value;
        }

        double abs()
        {
            return Math.Sqrt(_real * _real + _imag * _imag);
        }

        double arg()
        {
            return Math.Atan2(_imag, _real);
        }
		
		public override string ToString ()
		{
			return string.Format ("Scalar: {0}", ValueToString());
		}
		
		public string ValueToString()
		{
			if(Math.Abs(_imag) < epsilon)
				return string.Format(Tokens.NumberFormat, "{0}", Value);
			else if(Math.Abs(_real) < epsilon)
				return string.Format(Tokens.NumberFormat, "{0}i", ImaginaryValue);
			
			return string.Format (Tokens.NumberFormat, "{0}{2}{1}i", Value, ImaginaryValue, ImaginaryValue < 0 ? string.Empty : "+");
		}
		
		public override bool Equals (object obj)
		{
			if(obj is ScalarValue)
			{
				var sv = obj as ScalarValue;
				return sv._real == _real && sv._imag == _imag;
			}
			
			if(obj is double && _imag == 0.0)
				return (double)obj == _real;
			
			return false;
		}
		
		public override int GetHashCode ()
		{
			return (_real + _imag).GetHashCode();
		}
}
}
