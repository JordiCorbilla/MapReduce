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

using System.Diagnostics;
using System.Text;

namespace Thundax.MapReduce
{
    /// <summary>
    /// Performance Handler
    /// </summary>
    public class PerformanceHandler
    {
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private readonly Writer _writer;

        /// <summary>
        /// Constructor
        /// </summary>
        public PerformanceHandler()
        {
            _writer = new Writer();
            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total"
            };


            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        /// <summary>
        /// Get Current CPU usage
        /// </summary>
        /// <returns></returns>
        public string GetCurrentCpuUsage()
        {
            return _cpuCounter.NextValue() + "%";
        }

        /// <summary>
        /// Get Available RAM
        /// </summary>
        /// <returns></returns>
        public string GetAvailableRam()
        {
            return _ramCounter.NextValue() + "MB";
        }

        /// <summary>
        /// Flush content to disk
        /// </summary>
        public void FlushToDisk()
        {
            StringBuilder value = new StringBuilder();
            value.Append(GetCurrentCpuUsage());
            value.Append(" ");
            value.Append(GetAvailableRam() + "\n");
            _writer.WriteToFile(@"resultPerformance.txt", value);
        }
    }
}
