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
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class SymSrvHelpers
    {
        static int processId = Process.GetCurrentProcess().Id;

        private static bool InitSymSrv(string symPath)
        {
            return SafeNativeMethods.SymInitialize((IntPtr)processId,
                symPath,
                false);
        }

        private static bool CleanupSymSrv()
        {
            return SafeNativeMethods.SymCleanup((IntPtr)processId);
        }

        private static string GetLocalSymbolFolderForModule(string pdbFilename,
            string pdbGuid,
            int pdbAge
            )
        {
            const int MAX_PATH = 260;
            StringBuilder outPath = new StringBuilder(MAX_PATH);

            var guid = Guid.Parse(pdbGuid);
            int rawsize = Marshal.SizeOf(guid);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.StructureToPtr(guid, buffer, false);

            bool success = SafeNativeMethods.SymFindFileInPath((IntPtr)processId,
                null,
                pdbFilename,
                buffer,
                pdbAge,
                0,
                8, //SSRVOPT_GUIDPTR
                outPath,
                IntPtr.Zero,
                IntPtr.Zero);

            if (!success)
            {
                return String.Empty;
            }

            return outPath.ToString();
        }

        public static List<string> GetFolderPathsForPDBs(string symPath,
            List<Symbol> syms)
        {
            var retval = new List<string>();

            if (!InitSymSrv(symPath))
            {
                return retval;
            }

            foreach(var sym in syms)
            {
                var path = GetLocalSymbolFolderForModule(sym.PDBName,
                    sym.PDBGuid,
                    sym.PDBAge);

                if (!string.IsNullOrEmpty(path))
                {
                    retval.Add(Path.GetDirectoryName(path));
                }
            }

            CleanupSymSrv();

            return retval;
        }
    }
}
