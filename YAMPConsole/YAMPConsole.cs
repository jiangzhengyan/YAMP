﻿/*
	Copyright (c) 2012-2013, Florian Rappl.
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
using System.Text;
using YAMP;

namespace YAMPConsole
{
    static class YAMPConsole
    {
        public static void Run()
        {
            var buffer = new StringBuilder();
            var context = Parser.PrimaryContext;
            Console.WriteLine("Enter your own statements now (exit with the command 'exit'):");
            Console.WriteLine();
            var query = string.Empty;

            Parser.UseScripting = true;
            Parser.AddCustomFunction("G", v => new ScalarValue((v as ScalarValue).Value * Math.PI));
            Parser.AddCustomConstant("R", 2.53);

            while (true)
            {
                buffer.Remove(0, buffer.Length);
                Console.Write(">> ");

                while(true)
                {
                    query = Console.ReadLine();

                    if (query.Length != 0 && query[query.Length - 1] == '\\')
                    {
                        buffer.Append(query.Substring(0, query.Length - 1)).Append("\n");
                        continue;
                    }
                    else
                        buffer.Append(query);

                    break;
                }

                query = buffer.ToString();

                if (query.Equals("exit"))
                    break;
                else
                {
                    try
                    {
                        var result = context.Run(query);

                        if (result.Output != null)
                        {
                            Console.WriteLine(result.Result);
                            Console.Write(result.Parser);
                        }
                    }
                    catch (YAMPParseException parseex)
                    {
                        Console.WriteLine(parseex.Message);
                        Console.WriteLine("---");
                        Console.Write(parseex.ToString());
                        Console.WriteLine("---");
                    }
                    catch (YAMPRuntimeException runex)
                    {
                        Console.WriteLine("An exception during runtime occured:");
                        Console.WriteLine(runex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}