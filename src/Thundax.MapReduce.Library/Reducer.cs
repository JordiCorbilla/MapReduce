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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thundax.MapReduce
{
    /// <summary>
    ///Class that mimics the MapReduce functionality
    ///It will load the text in chunks of 100 words and
    ///process them in different threads and updating
    ///a global dictionary
    /// </summary>
    public class Reducer
    {
        private readonly ConcurrentDictionary<string, int> _distinctWordList;
        private readonly BlockingCollection<string> _wordChunks;
        /// <summary>
        /// Number of words
        /// </summary>
        public int NumWords { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Reducer()
        {
            _distinctWordList = new ConcurrentDictionary<string, int>();
            var concurrentBag = new ConcurrentBag<string>();
            _wordChunks = new BlockingCollection<string>(concurrentBag);
            NumWords = 0;
        }

        /// <summary>
        /// Return a list of chunks
        ///Basic idea is that the file is being divided in chunks of 100 words and
        ///then in parallel processed to increase performance
        /// </summary>
        private static IEnumerable<string> CreateChunks(string text, int chunkSize)
        {
            List<string> chunks = new List<string>();
            int offset = 0;
            while (offset < text.Length)
            {
                int size = Math.Min(chunkSize, text.Length - offset);
                chunks.Add(text.Substring(offset, size));
                offset += size;
            }
            return chunks;
        }

        //Method to clean up the words we are reading.
        //I'm just filtering for digits or letters.
        private void CleanUpWord(string[] words)
        {
            StringBuilder[] list = new StringBuilder[words.Length];

            for (int i = 0; i < words.Length; i++)
                list[i] = new StringBuilder();

            List<Task> tasks = new List<Task>();

            int index = 0;
            //This will generate as many tasks needed for the group of words
            foreach (string word in words)
            {
                int i = index;
                tasks.Add(Task.Factory.StartNew(() => {
                        LetterOrDigit(word, list[i]);
                    }));
                index++;
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var t in list)
            {
                if (t.Length > 0)
                {
                    _wordChunks.Add(t.ToString());
                    t.Clear();
                }
            }          
        }

        private static void LetterOrDigit(string word, StringBuilder sb)
        {
            //We could add more filtering here as I'm just considering letters or digits.
            foreach (var c in word.Where(char.IsLetterOrDigit))
            {
                sb.Append(c);
            }
        }

        //Parse every chunk of text and perform a clean up operation.
        //I'm replacing new lines also \r\n with spaces for better recognition.
        private void ParseWords(string text)
        {
            //We could also limit the parallelism here for testing -> new ParallelOptions { MaxDegreeOfParallelism = 1 }
            Parallel.ForEach(CreateChunks(text, 100), chunkBlock =>
            {   
                //split the block into words
                chunkBlock = chunkBlock.Replace(Environment.NewLine, " ");
                CleanUpWord(chunkBlock.Split(' '));
            });

            //After a collection has been marked as complete for adding, adding to the collection is 
            //not permitted and attempts to remove from the collection will not wait when the collection is empty.
            _wordChunks.CompleteAdding();
        }

        //This will add or update the dictionary with the number of references.
        //It will use a thread safe delegate to increment the value by 1 otherwise it will be defaulted to 1.
        private void UpdateDictionary()
        {
            Parallel.ForEach(_wordChunks.GetConsumingEnumerable(), 
                word => { _distinctWordList.AddOrUpdate(word, 1, 
                    (key, oldValue) => Interlocked.Increment(ref oldValue)); });
        }

        /// <summary>
        ///Main method to reduce the text and add all the words in a concurrent dictionary with a pair of key, value
        ///Each word will be recorded as key and when it's found, the value will be increased using a thread safe increment.
        /// </summary>
        /// <param name="text"></param>
        public void MapReduce(string text)
        {  
            //Create background process to map input data to words
            ThreadPool.QueueUserWorkItem(delegate {
                ParseWords(text);
            });

            //Update dictionary with key, value
            UpdateDictionary();
        }

        /// <summary>
        ///Method for display purposes using a sorted list.
        ///Also to count the whole list of words.
        ///This is just for testing and display
        /// </summary>
        /// <returns></returns>
        public StringBuilder SortedResults()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, int> kvp in _distinctWordList.OrderBy(key => key.Key))
            {
                sb.AppendLine(kvp.Key + ": " + kvp.Value);
                NumWords += kvp.Value;
            }
            return sb;
        }
    }
}
