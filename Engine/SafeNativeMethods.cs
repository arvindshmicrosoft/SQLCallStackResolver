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
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurityAttribute]
    internal class SafeNativeMethods
    {
        //Code adapted from Stack Exchange network post https://stackoverflow.com/questions/26514954/registration-free-com-interop-deactivating-activation-context-in-finalizer-thro
        //Authored by https://stackoverflow.com/users/3742925/aurora
        //Answered by https://stackoverflow.com/users/505088/david-heffernan

        private const uint ACTCTX_FLAG_RESOURCE_NAME_VALID = 0x008;

        private const UInt16 ISOLATIONAWARE_MANIFEST_RESOURCE_ID = 2;

        [DllImport("Kernel32.dll")]
        private extern static IntPtr CreateActCtx(ref ACTCTX actctx);
        [DllImport("Kernel32.dll")]
        private extern static bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);
        [DllImport("Kernel32.dll")]
        private extern static bool DeactivateActCtx(uint dwFlags, IntPtr lpCookie);
        [DllImport("Kernel32.dll")]
        private extern static bool ReleaseActCtx(IntPtr hActCtx);

        private struct ACTCTX
        {
            public int cbSize;
            public uint dwFlags;
            public string lpSource;
            public ushort wProcessorArchitecture;
            public ushort wLangId;
            public string lpAssemblyDirectory;
            public UInt16 lpResourceName;
            public string lpApplicationName;
            public IntPtr hModule;
        }

        [ThreadStatic]
        private static IntPtr m_cookie;

        [ThreadStatic]
        private static IntPtr m_hActCtx;

        internal static bool DestroyActivationContext()
        {
            if (m_cookie != IntPtr.Zero)
            {
                if (!DeactivateActCtx(0, m_cookie))
                    return false;

                m_cookie = IntPtr.Zero;

                if (!ReleaseActCtx(m_hActCtx))
                    return false;

                m_hActCtx = IntPtr.Zero;
            }

            return true;
        }

        internal static bool EstablishActivationContext()
        {
            ACTCTX info = new ACTCTX
            {
                cbSize = Marshal.SizeOf(typeof(ACTCTX)),
                dwFlags = ACTCTX_FLAG_RESOURCE_NAME_VALID,
                lpSource = System.Reflection.Assembly.GetExecutingAssembly().Location,
                lpResourceName = ISOLATIONAWARE_MANIFEST_RESOURCE_ID
            };

            m_hActCtx = CreateActCtx(ref info);

            if (m_hActCtx == new IntPtr(-1))
                return false;

            m_cookie = IntPtr.Zero;

            if (!ActivateActCtx(m_hActCtx, out m_cookie))
                return false;

            return true;
        }
    }
}
