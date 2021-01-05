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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Xunit;

    /// <summary>
    /// Class implementing xUnit tests.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Validate that "block symbols" in a PDB are resolved correctly.
        /// </summary>
        [Fact]
        public void BlockResolution()
        {
            using (var csr = new StackResolver())
            {
                var pdbPath = @"..\..\Tests\TestCases\TestBlockResolution";

                var ret = csr.ResolveCallstacks(
                    "Return Addr: 00007FF830D4CDA4 Module(KERNELBASE+000000000009CDA4)",
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    true,
                    false,
                    false,
                    null);

                Assert.Equal("KERNELBASE!SignalObjectAndWait+147716", ret.Trim());
            }
        }

        /// <summary>
        /// Test the resolution of OrdinalNNN symbols to their actual names.
        /// </summary>
        [Fact]
        public void OrdinalBasedSymbol()
        {
            using (var csr = new StackResolver())
            {
                var dllPaths = new List<string>
            {
                @"..\..\Tests\TestCases\TestOrdinal",
            };

                var ret = csr.ResolveCallstacks(
                    "sqldk!Ordinal298+00000000000004A5",
                    @"..\..\Tests\TestCases\TestOrdinal",
                    false,
                    dllPaths,
                    false,
                    false,
                    false,
                    false,
                    true,
                    false,
                    false,
                    null);

                Assert.Equal("sqldk!SOS_Scheduler::SwitchContext+941", ret.Trim());
            }
        }

        /// <summary>
        /// Test the resolution of a "regular" symbol with input specifying a hex offset into module.
        /// </summary>
        [Fact]
        public void RegularSymbolHexOffset()
        {
            using (var csr = new StackResolver())
            {
                var ret = csr.ResolveCallstacks(
                    "sqldk+0x40609\r\nsqldk+40609",
                    @"..\..\Tests\TestCases\TestOrdinal",
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    true,
                    false,
                    false,
                    null);

                var expectedSymbol = "sqldk!MemoryClerkInternal::AllocatePagesWithFailureMode+644";

                Assert.Equal(
                    expectedSymbol +
                    Environment.NewLine +
                    expectedSymbol, ret.Trim());
            }
        }

        /// <summary>
        /// Test the resolution of a "regular" symbol with virtual address as input.
        /// </summary>
        [Fact]
        public void RegularSymbolVirtualAddress()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesGood = @"c:\mssql\binn\sqldk.dll 00000001`00400000";

                Assert.True(csr.ProcessBaseAddresses(moduleAddressesGood));

                var ret = csr.ResolveCallstacks(
                    "0x000000010042249f",
                    @"..\..\Tests\TestCases\TestOrdinal",
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    true,
                    false,
                    false,
                    null);

                var expectedSymbol = "sqldk!Spinlock<244,2,1>::SpinToAcquireWithExponentialBackoff+349";

                Assert.Equal(expectedSymbol, ret.Trim());
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputEmptyString()
        {
            using (var csr = new StackResolver())
            {
                Assert.True(csr.ProcessBaseAddresses(string.Empty));
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputJunkString()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesBad = @"hello wor1213ld";
                Assert.False(csr.ProcessBaseAddresses(moduleAddressesBad));
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputColHeaders()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesColHeader = File.ReadAllText(@"..\..\Tests\TestCases\ImportXEL\xe_wait_base_addresses.txt");

                var loadstatus = csr.ProcessBaseAddresses(moduleAddressesColHeader);
                Assert.True(loadstatus);
                Assert.Equal(122, csr.LoadedModules.Count);

                var sqllang = csr.LoadedModules.Where(m => m.ModuleName == "sqllang").First();
                Assert.NotNull(sqllang);

                Assert.Equal(0x00007FFACB6F0000UL, sqllang.BaseAddress);
                Assert.Equal(0x00007FFAD089FFFFUL, sqllang.EndAddress);
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputFullPathSingleLine()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesGood = @"c:\mssql\binn\sqldk.dll 0000000100400000";

                Assert.True(csr.ProcessBaseAddresses(moduleAddressesGood));
                Assert.Single(csr.LoadedModules);
                Assert.Equal("sqldk", csr.LoadedModules[0].ModuleName);
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputSingleLineBacktick()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesGoodBacktick = @"c:\mssql\binn\sqldk.dll 00000001`00400000";
                Assert.True(csr.ProcessBaseAddresses(moduleAddressesGoodBacktick));
                Assert.Single(csr.LoadedModules);
                Assert.Equal("sqldk", csr.LoadedModules[0].ModuleName);
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputModuleNameOnlySingleLine()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesGoodModuleNameOnly0x = @"sqldk.dll 0000000100400000";

                Assert.True(csr.ProcessBaseAddresses(moduleAddressesGoodModuleNameOnly0x));
                Assert.Single(csr.LoadedModules);
                Assert.Equal("sqldk", csr.LoadedModules[0].ModuleName);
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputModuleNameOnlySingleLine0x()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesGoodModuleNameOnly0x = @"sqldk.dll 0x0000000100400000";

                Assert.True(csr.ProcessBaseAddresses(moduleAddressesGoodModuleNameOnly0x));
                Assert.Single(csr.LoadedModules);
                Assert.Equal("sqldk", csr.LoadedModules[0].ModuleName);
            }
        }

        /// <summary>
        /// Check the processing of module base address information.
        /// </summary>
        [Fact]
        public void ModuleLoadAddressInputFullPathsTwoModules()
        {
            using (var csr = new StackResolver())
            {
                var moduleAddressesGood = @"c:\mssql\binn\sqldk.dll 0000000100400000
                                            c:\mssql\binn\sqllang.dll 0000000105600000";

                Assert.True(csr.ProcessBaseAddresses(moduleAddressesGood));
                Assert.Equal(2, csr.LoadedModules.Count);
                Assert.Equal("sqldk", csr.LoadedModules[0].ModuleName);
                Assert.Equal("sqllang", csr.LoadedModules[1].ModuleName);
            }
        }

        /// <summary>
        /// Test the resolution of a "regular" symbol with input specifying a hex offset into module
        /// but do not include the resolved symbol's offset in final output.
        /// </summary>
        [Fact]
        public void RegularSymbolHexOffsetNoOutputOffset()
        {
            using (var csr = new StackResolver())
            {
                var dllPaths = new List<string>
            {
                @"..\..\Tests\TestCases\TestOrdinal",
            };

                var ret = csr.ResolveCallstacks(
                    "sqldk+0x40609",
                    @"..\..\Tests\TestCases\TestOrdinal",
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    null);

                var expectedSymbol = "sqldk!MemoryClerkInternal::AllocatePagesWithFailureMode";

                Assert.Equal(expectedSymbol, ret.Trim());
            }
        }

        /// <summary>
        /// Check whether symbol details for a given binary are correct.
        /// </summary>
        [Fact]
        public void TestGetSymDetails()
        {
            var dllPaths = new List<string>
            {
                @"..\..\Tests\TestCases\TestOrdinal",
            };

            var ret = StackResolver.GetSymbolDetailsForBinaries(dllPaths, true);

            Assert.Single(ret);
            Assert.Equal("https://msdl.microsoft.com/download/symbols/sqldk.pdb/6a1934433512464b8b8ed905ad930ee62/sqldk.pdb", ret[0].DownloadURL);
            Assert.True(ret[0].DownloadVerified);
            Assert.Equal("2015.0130.4560.00 ((SQL16_SP1_QFE-CU).190312-0204)", ret[0].FileVersion);
        }

        /// <summary>
        /// Make sure that caching PDB files is working. To do this we must use XEL input to trigger multiple worker threads.
        /// </summary>
        [Fact]
        public void SymbolFileCaching()
        {
            using (var csr = new StackResolver())
            {
                var ret = csr.ExtractFromXEL(
                    new[] { @"..\..\Tests\TestCases\ImportXEL\xe_wait_completed_0_132353446563350000.xel" },
                    false);

                Assert.Equal(550, ret.Item1);

                var status = csr.ProcessBaseAddresses(File.ReadAllText(@"..\..\Tests\TestCases\ImportXEL\xe_wait_base_addresses.txt"));
                Assert.True(status);
                Assert.Equal(122, csr.LoadedModules.Count);

                var pdbPath = @"..\..\Tests\TestCases\sqlsyms\14.0.3192.2\x64";

                var symres = csr.ResolveCallstacks(
                    ret.Item2,
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    true,
                    null);

                Assert.Contains(
                    @"sqldk!XeSosPkg::wait_completed::Publish
sqldk!SOS_Scheduler::UpdateWaitTimeStats
sqldk!SOS_Task::PostWait
sqllang!SOS_Task::Sleep
sqllang!YieldAndCheckForAbort
sqllang!OptimizerUtil::YieldAndCheckForMemoryAndAbort
sqllang!OptTypeVRSetArray::IFindSet
sqllang!CConstraintProp::FEquivalent
sqllang!CJoinEdge::FConstrainsColumnSolvably
sqllang!CStCollOuterJoin::CardForColumns
sqllang!CStCollGroupBy::CStCollGroupBy
sqllang!CCardFrameworkSQL12::CardDistinct
sqllang!CCostUtils::CalcLoopJoinCachedInfo
sqllang!CCostUtils::PcctxLoopJoinHelper
sqllang!COpArg::PcctxCalculateNormalizeCtx
sqllang!CTask_OptInputs::Perform
sqllang!CMemo::ExecuteTasks
sqllang!CMemo::PerformOptimizationStage
sqllang!CMemo::OptimizeQuery
sqllang!COptContext::PexprSearchPlan
sqllang!COptContext::PcxteOptimizeQuery
sqllang!COptContext::PqteOptimizeWrapper
sqllang!PqoBuild
sqllang!CStmtQuery::InitQuery",
                    symres.Trim(),
                    StringComparison.CurrentCulture);
            }
        }

        /// <summary>
        /// Validate that source information is retrieved correctly.
        /// This test uses symbols for a Windows Driver Kit module, Wdf01000.sys,
        /// because private PDBs for that module are legitimately available on the
        /// Microsoft public symbols servers.
        /// https://github.com/microsoft/Windows-Driver-Frameworks/releases if interested.
        /// </summary>
        [Fact]
        public void SourceInformation()
        {
            using (var csr = new StackResolver())
            {
                var pdbPath = @"..\..\Tests\TestCases\SourceInformation";

                var ret = csr.ResolveCallstacks(
                    "Wdf01000+17f27",
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    true,
                    false,
                    true,
                    false,
                    false,
                    null);

                Assert.Equal(
                    "Wdf01000!FxPkgPnp::PowerPolicyCanChildPowerUp+143\t(minkernel\\wdf\\framework\\shared\\inc\\private\\common\\FxPkgPnp.hpp:4127)",
                    ret.Trim());
            }
        }

        /// <summary>
        /// Validate that source information is retrieved correctly.
        /// This test uses symbols for a Windows Driver Kit module, Wdf01000.sys,
        /// because private PDBs for that module are legitimately available on the
        /// Microsoft public symbols servers.
        /// https://github.com/microsoft/Windows-Driver-Frameworks/releases if interested.
        /// </summary>
        [Fact]
        public void SourceInformationLineInfoOff()
        {
            using (var csr = new StackResolver())
            {
                var pdbPath = @"..\..\Tests\TestCases\SourceInformation";

                var ret = csr.ResolveCallstacks(
                    "Wdf01000+17f27",
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    true,
                    false,
                    false,
                    null);

                Assert.Equal(
                    "Wdf01000!FxPkgPnp::PowerPolicyCanChildPowerUp+143",
                    ret.Trim());
            }
        }

        /// <summary>
        /// Validate that source information is retrieved correctly when "re-looking up"
        /// a symbol based on input which was already symbolized (but missing source info).
        /// This test uses symbols for a Windows Driver Kit module, Wdf01000.sys,
        /// because private PDBs for that module are legitimately available on the
        /// Microsoft public symbols servers.
        /// https://github.com/microsoft/Windows-Driver-Frameworks/releases if interested.
        /// </summary>
        [Fact]
        public void RelookupSourceInformation()
        {
            using (var csr = new StackResolver())
            {
                var pdbPath = @"..\..\Tests\TestCases\SourceInformation";

                var ret = csr.ResolveCallstacks(
                    "Wdf01000!FxPkgPnp::PowerPolicyCanChildPowerUp+143",
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    true,
                    true,
                    true,
                    false,
                    false,
                    null);

                Assert.Equal(
                    "Wdf01000!FxPkgPnp::PowerPolicyCanChildPowerUp+143\t(minkernel\\wdf\\framework\\shared\\inc\\private\\common\\FxPkgPnp.hpp:4127)",
                    ret.Trim());
            }
        }

        /// <summary>
        /// Validate importing callstack events from XEL files into histogram buckets.
        /// </summary>
        [Fact]
        public void ImportBinResolveXELEvents()
        {
            using (var csr = new StackResolver())
            {
                var ret = csr.ExtractFromXEL(
                    new[] { @"..\..\Tests\TestCases\ImportXEL\XESpins_0_131627061603030000.xel" },
                    true);

                Assert.Equal(4, ret.Item1);

                var xmldoc = new XmlDocument() { XmlResolver = null };
                bool isXMLdoc = false;
                try
                {
                    using (var sreader = new StringReader(ret.Item2))
                    {
                        using (var reader = XmlReader.Create(sreader, new XmlReaderSettings() { XmlResolver = null }))
                        {
                            xmldoc.Load(reader);
                        }
                    }

                    isXMLdoc = true;
                }
                catch (XmlException)
                {
                    // do nothing because this is not a XML doc
                }

                Assert.True(isXMLdoc);

                var slotNodes = xmldoc.SelectNodes("/HistogramTarget/Slot");

                Assert.Equal(4, slotNodes.Count);

                int eventCountFromXML = 0;
                foreach (XmlNode slot in slotNodes)
                {
                    eventCountFromXML += int.Parse(slot.Attributes["count"].Value, CultureInfo.CurrentCulture);
                }

                Assert.Equal(3051540, eventCountFromXML);

                csr.ProcessBaseAddresses(File.ReadAllText(@"..\..\Tests\TestCases\ImportXEL\base_addresses.txt"));

                Assert.Equal(156, csr.LoadedModules.Count);

                var pdbPath = @"..\..\Tests\TestCases\sqlsyms\13.0.4001.0\x64";

                var symres = csr.ResolveCallstacks(
                    ret.Item2,
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    true,
                    false,
                    true,
                    false,
                    false,
                    null);

                Assert.Contains(
                    @"sqldk!XeSosPkg::spinlock_backoff::Publish+425
sqldk!SpinlockBase::Sleep+182
sqlmin!Spinlock<143,7,1>::SpinToAcquireWithExponentialBackoff+363
sqlmin!lck_lockInternal+2042
sqlmin!MDL::LockGenericLocal+382
sqlmin!MDL::LockGenericIdsLocal+101
sqlmin!CMEDCacheEntryFactory::GetProxiedCacheEntryById+263
sqlmin!CMEDProxyDatabase::GetOwnerByOwnerId+122
sqllang!CSECAccessAuditBase::SetSecurable+427
sqllang!CSECManager::_AccessCheck+151
sqllang!CSECManager::AccessCheck+2346
sqllang!FHasEntityPermissionsWithAuditState+1505
sqllang!FHasEntityPermissions+165
sqllang!CSQLObject::FPostCacheLookup+2562
sqllang!CSQLSource::Transform+2194
sqllang!CSQLSource::Execute+944
sqllang!CStmtExecProc::XretLocalExec+622
sqllang!CStmtExecProc::XretExecExecute+1153
sqllang!CXStmtExecProc::XretExecute+56
sqllang!CMsqlExecContext::ExecuteStmts<1,1>+1037
sqllang!CMsqlExecContext::FExecute+2718
sqllang!CSQLSource::Execute+2435
sqllang!process_request+3681
sqllang!process_commands_internal+735",
                    symres,
                    StringComparison.CurrentCulture);
            }
        }

        /// <summary>
        /// Validate importing individual callstack events from XEL files.
        /// </summary>
        [Fact]
        public void ImportIndividualXELEvents()
        {
            using (var csr = new StackResolver())
            {
                var ret = csr.ExtractFromXEL(
                    new[] { @"..\..\Tests\TestCases\ImportXEL\xe_wait_completed_0_132353446563350000.xel" },
                    false);

                Assert.Equal(550, ret.Item1);
            }
        }

        /// <summary>
        /// Validate importing "single-line" callstack (such as when the input is copy-pasted from SSMS).
        /// </summary>
        [Fact]
        public void SingleLineCallStack()
        {
            using (var csr = new StackResolver())
            {
                csr.ProcessBaseAddresses(File.ReadAllText(@"..\..\Tests\TestCases\ImportXEL\base_addresses.txt"));

                Assert.Equal(156, csr.LoadedModules.Count);

                var pdbPath = @"..\..\Tests\TestCases\sqlsyms\13.0.4001.0\x64";

                var callStack = @"callstack	0x00007FFEABD0D919  0x00007FFEABC4D45D  0x00007FFEAC0F7EE0  0x00007FFEAC0F80CF  0x00007FFEAC1EE447  0x00007FFEAC1EE6F5  0x00007FFEAC1D48B0  0x00007FFEAC71475A  0x00007FFEA9A708F1  0x00007FFEA9991FB9  0x00007FFEA9993D21  0x00007FFEA99B59F1  0x00007FFEA99B5055  0x00007FFEA99B2B8F  0x00007FFEA9675AD1  0x00007FFEA9671EFB  0x00007FFEAA37D83D  0x00007FFEAA37D241  0x00007FFEAA379F98  0x00007FFEA96719CA  0x00007FFEA9672933  0x00007FFEA9672041  0x00007FFEA967A82B  0x00007FFEA9681542  ";

                var symres = csr.ResolveCallstacks(
                    callStack,
                    pdbPath,
                    false,
                    null,
                    false,
                    true,
                    true,
                    false,
                    true,
                    false,
                    false,
                    null);

                Assert.Equal(
                    @"callstack
sqldk!XeSosPkg::spinlock_backoff::Publish+425
sqldk!SpinlockBase::Sleep+182
sqlmin!Spinlock<143,7,1>::SpinToAcquireWithExponentialBackoff+363
sqlmin!lck_lockInternal+2042
sqlmin!MDL::LockGenericLocal+382
sqlmin!MDL::LockGenericIdsLocal+101
sqlmin!CMEDCacheEntryFactory::GetProxiedCacheEntryById+263
sqlmin!CMEDProxyDatabase::GetOwnerByOwnerId+122
sqllang!CSECAccessAuditBase::SetSecurable+427
sqllang!CSECManager::_AccessCheck+151
sqllang!CSECManager::AccessCheck+2346
sqllang!FHasEntityPermissionsWithAuditState+1505
sqllang!FHasEntityPermissions+165
sqllang!CSQLObject::FPostCacheLookup+2562
sqllang!CSQLSource::Transform+2194
sqllang!CSQLSource::Execute+944
sqllang!CStmtExecProc::XretLocalExec+622
sqllang!CStmtExecProc::XretExecExecute+1153
sqllang!CXStmtExecProc::XretExecute+56
sqllang!CMsqlExecContext::ExecuteStmts<1,1>+1037
sqllang!CMsqlExecContext::FExecute+2718
sqllang!CSQLSource::Execute+2435
sqllang!process_request+3681
sqllang!process_commands_internal+735",
                    symres.Trim());
            }
        }

        /// <summary>
        /// Test for inline frame resolution.
        /// This test uses symbols for a Windows Driver Kit module, Wdf01000.sys,
        /// because private PDBs for that module are legitimately available on the
        /// Microsoft public symbols servers.
        /// https://github.com/microsoft/Windows-Driver-Frameworks/releases if interested.
        /// </summary>
        [Fact]
        public void InlineFrameResolution()
        {
            using (var csr = new StackResolver())
            {
                var pdbPath = @"..\..\Tests\TestCases\SourceInformation";

                var ret = csr.ResolveCallstacks(
                    "Wdf01000+17f27",
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    true,
                    false,
                    true,
                    true,
                    false,
                    null);

                Assert.Equal(
                    @"(Inline Function) Wdf01000!Mx::MxLeaveCriticalRegion+12	(minkernel\wdf\framework\shared\inc\primitives\km\MxGeneralKm.h:198)
(Inline Function) Wdf01000!FxWaitLockInternal::ReleaseLock+62	(minkernel\wdf\framework\shared\inc\private\common\FxWaitLock.hpp:305)
(Inline Function) Wdf01000!FxEnumerationInfo::ReleaseParentPowerStateLock+62	(minkernel\wdf\framework\shared\inc\private\common\FxPkgPnp.hpp:510)
Wdf01000!FxPkgPnp::PowerPolicyCanChildPowerUp+143	(minkernel\wdf\framework\shared\inc\private\common\FxPkgPnp.hpp:4127)",
                    ret.Trim());
            }
        }

        /// <summary>
        /// Test for inline frame resolution without source lines included
        /// This test uses symbols for a Windows Driver Kit module, Wdf01000.sys,
        /// because private PDBs for that module are legitimately available on the
        /// Microsoft public symbols servers.
        /// https://github.com/microsoft/Windows-Driver-Frameworks/releases if interested.
        /// </summary>
        [Fact]
        public void InlineFrameResolutionNoSourceInfo()
        {
            using (var csr = new StackResolver())
            {
                var pdbPath = @"..\..\Tests\TestCases\SourceInformation";

                var ret = csr.ResolveCallstacks(
                    "Wdf01000+17f27",
                    pdbPath,
                    false,
                    null,
                    false,
                    false,
                    false,
                    false,
                    true,
                    true,
                    false,
                    null);

                Assert.Equal(
                    @"(Inline Function) Wdf01000!Mx::MxLeaveCriticalRegion+12
(Inline Function) Wdf01000!FxWaitLockInternal::ReleaseLock+62
(Inline Function) Wdf01000!FxEnumerationInfo::ReleaseParentPowerStateLock+62
Wdf01000!FxPkgPnp::PowerPolicyCanChildPowerUp+143",
                    ret.Trim());
            }
        }
    }
}
