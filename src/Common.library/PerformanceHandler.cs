﻿//The MIT License (MIT)

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

namespace Common.library
{
    public class PerformanceHandler
    {
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        private Writer _writer;

        public PerformanceHandler()
        {
            _writer = new Writer();
            _cpuCounter = new PerformanceCounter();

            _cpuCounter.CategoryName = "Processor";
            _cpuCounter.CounterName = "% Processor Time";
            _cpuCounter.InstanceName = "_Total";

            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public string getCurrentCpuUsage()
        {
            return _cpuCounter.NextValue() + "%";
        }

        public string getAvailableRAM()
        {
            return _ramCounter.NextValue() + "MB";
        }

        public void flushToDisk()
        {
            StringBuilder Value = new StringBuilder();
            Value.Append(getCurrentCpuUsage());
            Value.Append(" ");
            Value.Append(getAvailableRAM() + "\n");
            _writer.WriteToFile(@"resultPerformance.txt", Value);
        }
    }
}