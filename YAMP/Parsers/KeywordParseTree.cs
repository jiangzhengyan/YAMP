﻿/*
	Copyright (c) 2012, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

namespace YAMP
{
	/// <summary>
	/// The class used to (additionally) detect keywords in statements.
	/// </summary>
	class KeywordParseTree : StatementParseTree
    {
        #region Members

        bool hasStarted;

        #endregion

        #region ctor

        public KeywordParseTree(QueryContext context, string input, int line) : base(context, input, line)
		{
		}

        #endregion

        #region Methods

        protected override Expression FindExpression(string input)
        {
            if (!hasStarted)
            {
                hasStarted = true;
                var keyword = ParseKeyword(input);

                if (keyword != null)
                    return new KeywordExpression(Query, keyword);
            }

            return base.FindExpression(input);
        }

        Keyword ParseKeyword(string input)
        {
            var tokens = ParseTree.Get(Tokens.Letter, input);
            var keyword = Elements.Instance.FindKeyword(Query, tokens);
            return keyword;
        }

        #endregion
    }
}
