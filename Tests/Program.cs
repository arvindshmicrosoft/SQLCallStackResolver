//------------------------------------------------------------------------------
//<copyright company="Microsoft">
//    The MIT License (MIT)
//    
//    Copyright (c) 2017 Microsoft
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
//</copyright>
//------------------------------------------------------------------------------
namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            if (!TestBlockResolution())
                Console.WriteLine("FAIL: TestBlockResolution");
            else
                Console.WriteLine("PASS: TestBlockResolution");

            if (!TestOrdinal())
                Console.WriteLine("FAIL: TestOrdinal");
            else
                Console.WriteLine("PASS: TestOrdinal");

        }

        private static bool TestBlockResolution()
        {
            var csr = new StackResolver();
            var ret = csr.ResolveCallstacks("Return Addr: 00007FF830D4CDA4 Module(KERNELBASE+000000000009CDA4)",
                @"..\..\Tests\TestCases\TestBlockResolution",
                false,
                null,
                false,
                false,
                false,
                false,
                true);

            return ret.Trim() == "KERNELBASE!SignalObjectAndWait+147716";
        }

        private static bool TestOrdinal()
        {
            var csr = new StackResolver();
            var dllPaths = new System.Collections.Generic.List<string>();
            dllPaths.Add(@"..\..\Tests\TestCases\TestOrdinal");

            var ret = csr.ResolveCallstacks("sqldk!Ordinal298+00000000000004A5",
                @"..\..\Tests\TestCases\TestOrdinal",
                false,
                dllPaths,
                false,
                false,
                false,
                false,
                true);

            return ret.Trim() == "sqldk!SOS_Scheduler::SwitchContext+941";
        }
    }
}
