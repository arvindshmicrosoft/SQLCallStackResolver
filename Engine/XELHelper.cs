//------------------------------------------------------------------------------
//    The MIT License (MIT)
//    
//    Copyright (c) Arvind Shyamsundar
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
//    This sample code is not supported under any Microsoft standard support program or service. 
//    The entire risk arising out of the use or performance of the sample scripts and documentation remains with you. 
//    In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the scripts
//    be liable for any damages whatsoever (including, without limitation, damages for loss of business profits,
//    business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability
//    to use the sample scripts or documentation, even if Microsoft has been advised of the possibility of such damages.
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    using Microsoft.SqlServer.XEvent.XELite;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class XELHelper
    {
        /// <summary>
        /// Read a XEL file, consume all callstacks, optionally bucketize them, and in all cases,
        /// return the information as equivalent XML
        /// </summary>
        /// <param name="parent">Parent StackResolver instance, used for progress reporting and cancellation constructs</param>
        /// <param name="xelFiles">List of paths to XEL files to read</param>
        /// <param name="bucketize">Boolean, whether to combine identical callstack patterns into the same "bucket"</param>
        /// <returns>A tuple with the count of events and XML equivalent of the histogram corresponding to these events</returns>
        internal static Tuple<int, string> ExtractFromXEL(StackResolver parent,
            string[] xelFiles,
            bool bucketize)
        {
            Contract.Requires(xelFiles != null);

            parent.cancelRequested = false;

            var callstackSlots = new Dictionary<string, long>();
            var callstackRaw = new Dictionary<string, string>();
            var xmlEquivalent = new StringBuilder();

            // the below feels quite hacky. Unfortunately till such time that we have strong typing in XELite I believe this is unavoidable
            var relevantKeyNames = new string[] { "callstack", "call_stack", "stack_frames" };

            foreach (var xelFileName in xelFiles)
            {
                if (File.Exists(xelFileName))
                {
                    parent.StatusMessage = $@"Reading {xelFileName}...";

                    var xeStream = new XEFileEventStreamer(xelFileName);

                    xeStream.ReadEventStream(
                        () =>
                        {
                            return Task.CompletedTask;
                        },
                        evt =>
                        {
                            var allStacks = (from actTmp in evt.Actions
                                             where relevantKeyNames.Contains(actTmp.Key.ToLower(CultureInfo.CurrentCulture))
                                             select actTmp.Value as string)
                                                .Union(
                                                from fldTmp in evt.Fields
                                                where relevantKeyNames.Contains(fldTmp.Key.ToLower(CultureInfo.CurrentCulture))
                                                select fldTmp.Value as string);

                            foreach (var callStackString in allStacks)
                            {
                                if (string.IsNullOrEmpty(callStackString))
                                {
                                    continue;
                                }

                                if (bucketize)
                                {
                                    lock (callstackSlots)
                                    {
                                        if (!callstackSlots.ContainsKey(callStackString))
                                        {
                                            callstackSlots.Add(callStackString, 1);
                                        }
                                        else
                                        {
                                            callstackSlots[callStackString]++;
                                        }
                                    }
                                }
                                else
                                {
                                    var evtId = string.Format(CultureInfo.CurrentCulture,
                                        "File: {0}, Timestamp: {1}, UUID: {2}:",
                                        xelFileName,
                                        evt.Timestamp.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.CurrentCulture),
                                        evt.UUID);

                                    lock (callstackRaw)
                                    {
                                        if (!callstackRaw.ContainsKey(evtId))
                                        {
                                            callstackRaw.Add(evtId, callStackString);
                                        }
                                        else
                                        {
                                            callstackRaw[evtId] += $"{Environment.NewLine}{callStackString}";
                                        }
                                    }
                                }
                            }

                            return Task.CompletedTask;
                        },
                        CancellationToken.None).Wait();
                }
            }

            parent.StatusMessage = "Finished reading file(s), finalizing output...";

            int finalEventCount;

            if (bucketize)
            {
                xmlEquivalent.AppendLine("<HistogramTarget>");
                parent.globalCounter = 0;

                foreach (var item in callstackSlots.OrderByDescending(key => key.Value))
                {
                    xmlEquivalent.AppendFormat(CultureInfo.CurrentCulture,
                        "<Slot count=\"{0}\"><value>{1}</value></Slot>",
                        item.Value,
                        item.Key);

                    xmlEquivalent.AppendLine();

                    parent.globalCounter++;
                    parent.PercentComplete = (int)((double)parent.globalCounter / callstackSlots.Count * 100.0);
                }

                xmlEquivalent.AppendLine("</HistogramTarget>");

                finalEventCount = callstackSlots.Count;
            }
            else
            {
                xmlEquivalent.AppendLine("<Events>");
                parent.globalCounter = 0;

                var hasOverflow = false;

                foreach (var item in callstackRaw.OrderBy(key => key.Key))
                {
                    if (xmlEquivalent.Length < int.MaxValue * 0.90)
                    {
                        xmlEquivalent.AppendFormat(CultureInfo.CurrentCulture,
                            "<event key=\"{0}\"><action name='callstack'><value>{1}</value></action></event>",
                            item.Key,
                            item.Value);

                        xmlEquivalent.AppendLine();
                    }
                    else
                    {
                        hasOverflow = true;
                    }

                    parent.globalCounter++;
                    parent.PercentComplete = (int)((double)parent.globalCounter / callstackRaw.Count * 100.0);
                }

                if (hasOverflow) xmlEquivalent.AppendLine("<!-- WARNING: output was truncated due to size limits -->");

                xmlEquivalent.AppendLine("</Events>");

                finalEventCount = callstackRaw.Count;
            }

            parent.StatusMessage = $@"Finished processing {xelFiles.Length} XEL files";

            return new Tuple<int, string>(finalEventCount, xmlEquivalent.ToString());
        }
    }
}
