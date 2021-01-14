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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class Preprocessors
    {
        /// <summary>
        /// Find out distinct module names in a given stack. This is used to later load PDBs and optionally DLLs
        /// </summary>
        /// <param name="callStack"></param>
        /// <returns></returns>
        internal static List<string> EnumModuleNames(string[] callStack)
        {
            List<string> uniqueModuleNames = new List<string>();
            var reconstructedCallstack = new StringBuilder();
            foreach (var frame in callStack)
            {
                reconstructedCallstack.AppendLine(frame);
            }

            // using the ".dll!0x" to locate the module names
            var rgxModuleName = new Regex(@"(?<module>\w+)((\.(dll|exe))*(!(?<symbolizedfunc>.+))*)*\s*\+(0[xX])*");
            var matchedModuleNames = rgxModuleName.Matches(reconstructedCallstack.ToString());

            foreach (Match moduleMatch in matchedModuleNames)
            {
                var actualModuleName = moduleMatch.Groups["module"].Value;

                if (!uniqueModuleNames.Contains(actualModuleName))
                {
                    uniqueModuleNames.Add(actualModuleName);
                }
            }

            return uniqueModuleNames;
        }
    }
}
