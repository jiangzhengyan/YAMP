﻿using System;

namespace YAMP
{
	[Description("Formats some text using placeholders like {0} etc. and returns the resulted string.")]
	[Kind(PopularKinds.System)]
	class PrintfFunction : ArgumentFunction
	{
		[Description("Formats some text using placeholders like {0} etc. and returns the resulted string.")]
		[Example("printf(\"{2} The result of {0} + {1} is {3}\", 2.0, 7i, \"Hi!\", 2.0 + 7i)", "Evaluates the arguments and includes them in the string given by the first argument.")]
		[Arguments(1)]
		public StringValue Function(StringValue text, ArgumentsValue args)
		{
			var content = string.Format(text.Value, args.ToArray());
			return new StringValue(content);
		}
	}
}
