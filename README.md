# MapReduce
**Data parallel text processing with MapReduce.**

![](https://img.shields.io/badge/license-MIT-red.svg) ![](https://img.shields.io/badge/NET-4.8-red.svg)

This is one my solution for this common problem using MapReduce.
The best way to process any task is to split it in several chunks and divide the work amongst several **'workers'** in a distributed way and then compose the results in a later stage. 

For this problem I am using **MapReduce approach** using **LINQ** and **.NET 4.6**.

Processing the file sequentially and keeping the results in a **dictionary <key, value>** is an easy task but with a really bad performance. Using MapReduce strategy we could split the work and then combine the results in a concurrent dictionary:

![](http://3.bp.blogspot.com/-kp8P4JNTZGs/Vf6rVGiuweI/AAAAAAAAFHk/uwvz_faeJjY/s640/examplemapreduce.png)

![](https://3.bp.blogspot.com/-I1VeJ2WgIpI/V0wHmC0c3TI/AAAAAAAAFeU/J35AcpRlL_EM9eleNGVJwUQmeWs6vE7fQCLcB/s1600/aa2.png)

*Nuget Package:*
You can download this package from Nuget in your VS solution.
https://www.nuget.org/packages/Thundax.MapReduce.dll/

*Example usage:*
```C#
using Thundax.MapReduce;

Reducer reducer = new Reducer();
try
{
    string readText = File.ReadAllText(args[0]);
    Console.WriteLine("Starting reduction");
    reducer.MapReduce(readText);
    Console.WriteLine("Reduction completed");
    File.WriteAllText("Results.txt", reducer.SortedResults().ToString());
    Console.WriteLine("Done!, processing {0:D} words", reducer.Numwords);
    Console.WriteLine("Please review Results.txt");

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);                
}
```

**Benchmark testing:**

- This solution can count 500k workds in 400ms using 1 processor with 4 cores.
- Download the version 1.0 from here. https://app.box.com/s/jfh684sgqtp0it856fvcv6186sssn3m3
- or via Nuget https://www.nuget.org/packages/Thundax.MapReduce.dll/

**Licence**
-------

    The MIT License (MIT)
    
    Copyright (c) 2015 Jordi Corbilla
    
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
