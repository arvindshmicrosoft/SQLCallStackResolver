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

using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    /// <summary>
    /// Helper class which stores DLL export name and address (offset)
    /// </summary>
    public class ExportedSymbol
    {
        public string Name { get; set; }
        public uint Address { get; set; }

        /// <summary>
        /// Helper function to load a DLL and then lookup exported functions. For this we use CLRMD and specifically the PEHeader class
        /// </summary>
        /// <param name="DLLPath"></param>
        /// <returns></returns>
        public static Dictionary<int, ExportedSymbol> GetExports(string DLLPath)
        {
            // this is the placeholder for the final mapping of ordinal # to address map
            Dictionary<int, ExportedSymbol> exports = null;

            using (var dllStream = new FileStream(DLLPath, FileMode.Open, FileAccess.Read))
            {
                using (var dllImage = new PEImage(dllStream))
                {
                    var dir = dllImage.PEHeader.ExportTableDirectory;
                    var offset = dllImage.RvaToOffset(Convert.ToInt32(dir.RelativeVirtualAddress));

                    using (var mmf = MemoryMappedFile.CreateFromFile(dllStream,
                        null,
                        0,
                        MemoryMappedFileAccess.Read,
                        null,
                        HandleInheritability.None,
                        false))
                    {
                        using (var mmfAccessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read))
                        {
                            mmfAccessor.Read(offset, out ImageExportDirectory exportDirectory);

                            var count = exportDirectory.NumberOfFunctions;
                            exports = new Dictionary<int, ExportedSymbol>(count);

                            var namesOffset = exportDirectory.AddressOfNames != 0 ? dllImage.RvaToOffset(exportDirectory.AddressOfNames) : 0;
                            var ordinalOffset = exportDirectory.AddressOfOrdinals != 0 ? dllImage.RvaToOffset(exportDirectory.AddressOfOrdinals) : 0;
                            var functionsOffset = dllImage.RvaToOffset(exportDirectory.AddressOfFunctions);

                            var ordinalBase = (int)exportDirectory.Base;

                            for (uint funcOrdinal = 0; funcOrdinal < count; funcOrdinal++)
                            {
                                // read function address
                                var address = mmfAccessor.ReadUInt32(functionsOffset + funcOrdinal * 4);

                                if (0 != address)
                                {
                                    exports.Add((int)(ordinalBase + funcOrdinal), new ExportedSymbol
                                    {
                                        Name = string.Format(CultureInfo.CurrentCulture,
                                            "Ordinal{0}",
                                            ordinalBase + funcOrdinal),
                                        Address = address
                                    });
                                }
                            }
                        }
                    }
                }

                return exports;
            }
        }
    }
}