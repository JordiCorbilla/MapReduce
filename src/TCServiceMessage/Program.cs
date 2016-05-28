//The MIT License (MIT)

//Copyright (c) 2015 Jordi Corbilla

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

//######################################
// Counting words using MapReduce Approach 
// http://en.wikipedia.org/wiki/MapReduce
// @Author: Jordi Corbilla
// @version: 1.0
// @date: 23/03/2015
//######################################

using System;

namespace TCServiceMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length < 1)
            {
                Console.WriteLine("Please specify the output.dcvr file path.");
                Console.WriteLine("Example:");
                Console.WriteLine(@"TCServiceMessage C:\temp\output.dcvr");
            }
            else
            {
                Console.WriteLine("*******************************************");
                Console.WriteLine(@"##teamcity[importData type='dotNetCoverage' tool='dotcover' path='" + args[0] + "']");
            }
        }
    }
}
