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
using System.Diagnostics;
using System.IO;

namespace CountingWordsConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the file you want to process.");
                Console.WriteLine("Usage: CountingWordsConsole <filename>");
                return 1;
            }

            Reducer reducer = new Reducer();
            try
            {
                Stopwatch sw = new Stopwatch();
                string readText = File.ReadAllText(args[0]);
                Console.WriteLine("Starting reduction");
                sw.Start();
                reducer.MapReduce(readText);
                sw.Stop();
                Console.WriteLine("Reduction completed");
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                File.WriteAllText("Results.txt", reducer.SortedResults().ToString());
                Console.WriteLine("Done!, processing {0:D} words", reducer.Numwords);
                Console.WriteLine("Please review Results.txt");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }
               
            return 0;
        }
    }
   
}
