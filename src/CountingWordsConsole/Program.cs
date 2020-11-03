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

using Thundax.MapReduce;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CountingWordsConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            PrintHeader();
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter the file you want to process.");
                Console.WriteLine("Usage: CountingWordsConsole <filename>");
                Console.WriteLine(@"");
                Console.ResetColor();
                return 1;
            }

            StateTimeClass stateObj = new StateTimeClass
            {
                Canceled = false, Handler = new PerformanceHandler(), Value = 1
            };
            System.Threading.TimerCallback timerDelegate = TimerTask;

            // Create a timer that calls a procedure every 200ms
            // Note: There is no Start method; the timer starts running as soon as 
            // the instance is created.
            System.Threading.Timer timerItem = new System.Threading.Timer(timerDelegate, stateObj, 100, 100);

            // Save a reference for Dispose.
            stateObj.Reference = timerItem;

            Reducer reducer = new Reducer();
            try
            {
                SystemDetails.ShowCpuDetails();
                Stopwatch sw = new Stopwatch();
                string readText = File.ReadAllText(args[0]);
                Console.WriteLine("Starting reduction");
                sw.Start();
                reducer.MapReduce(readText);
                sw.Stop();
                Console.WriteLine("Reduction completed");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                File.WriteAllText("Results.txt", reducer.SortedResults().ToString());
                Console.WriteLine($"Done!, processing {reducer.NumWords.ToString("N", CultureInfo.InvariantCulture)} words");
                Console.WriteLine("Please review Results.txt");
                Console.ResetColor();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }

            // Request Dispose of the timer object.
            stateObj.Canceled = true;

            return 0;
        }

        private static void TimerTask(object status)
        {
            StateTimeClass state = (StateTimeClass)status;
            // Use the interlocked class to increment the counter variable.
            System.Threading.Interlocked.Increment(ref state.Value);
            Debug.WriteLine("Launched new thread  " + DateTime.UtcNow);
            state.Handler.FlushToDisk();
            if (state.Canceled)
            // Dispose Requested.
            {
                state.Reference.Dispose();
                Debug.WriteLine("Done  " + DateTime.UtcNow);
            }
        }

        private static void PrintHeader()
        {
            //ASCII art from http://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"___  ___           ______         _                ");
            Console.WriteLine(@"|  \/  |           | ___ \       | |               ");
            Console.WriteLine(@"| .  . | __ _ _ __ | |_/ /___  __| |_   _  ___ ___ ");
            Console.WriteLine(@"| |\/| |/ _` | '_ \|    // _ \/ _` | | | |/ __/ _ \");
            Console.WriteLine(@"| |  | | (_| | |_) | |\ \  __/ (_| | |_| | (_|  __/");
            Console.WriteLine(@"\_|  |_/\__,_| .__/\_| \_\___|\__,_|\__,_|\___\___|");
            Console.WriteLine(@"             | |                                   ");
            Console.WriteLine(@"             |_|                                   ");
            Console.WriteLine(@" approach by Jordi Corbilla, 2016");
            Console.WriteLine(@"");
            Console.ResetColor();
        }
    }
   
}
