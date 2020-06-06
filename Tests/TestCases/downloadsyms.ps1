param ($sqldkDownloadURL)

$msdlurl = "https://msdl.microsoft.com/download/symbols/"

### TestBlockResolution
mkdir -Force ".\TestBlockResolution" -ErrorAction Ignore
$localpath = ".\TestBlockResolution\kernelbase.pdb"
if (-not (test-path $localpath))
{
    Invoke-WebRequest -UseBasicParsing -uri ($msdlurl + "kernelbase.pdb/E26F9607943644BB8CDE6C806006A3F01/kernelbase.pdb") -OutFile $localpath
    dir $localpath
}

### SourceInformation
mkdir -Force ".\SourceInformation" -ErrorAction Ignore
$localpath = ".\SourceInformation\Wdf01000.pdb"
if (-not (test-path $localpath))
{
    Invoke-WebRequest -UseBasicParsing -uri ($msdlurl + "Wdf01000.pdb/C9EC293769A815AB22B5909C290FB9631/Wdf01000.pdb") -OutFile $localpath
    dir $localpath
}


### TestOrdinal
mkdir -Force ".\TestOrdinal" -ErrorAction Ignore
$localpath = ".\TestOrdinal\sqldk.pdb"
if (-not (test-path $localpath))
{
    Invoke-WebRequest -UseBasicParsing -uri ($msdlurl + "sqldk.pdb/6a1934433512464b8b8ed905ad930ee62/sqldk.pdb") -OutFile $localpath
    dir $localpath
}

$localpath = ".\TestOrdinal\sqldk.dll"
if (-not (test-path $localpath))
{
    try
    {
        Invoke-WebRequest -UseBasicParsing -uri $sqldkDownloadURL -OutFile $localpath -ErrorAction Ignore
    }
    catch
    {    
	}

    if (-not (test-path $localpath))
    {
        Write-Warning "You must manually download CU14 for SQL 2016 SP1 (KB 4488535) and extract the sqldk.dll from that install to TestBlockResolution\sqldk.dll"
    }
}

### ImportXEL
$localpath = ".\ImportXEL\XESpins_0_131627061603030000.xel"
mkdir -Force ".\ImportXEL" -ErrorAction Ignore
if (-not (test-path $localpath))
{
    Expand-Archive -Path "..\..\docs\SQLSat696\Demos\LOCK_HASH\XESpins_0_131627061603030000.zip" -DestinationPath ".\ImportXEL" 
}

# SQL Server 2016 SP1 SP1 - 13.0.4001.0 - x64 (KB3182545)
$outputFolder = '.\sqlsyms\13.0.4001.0\x64'
mkdir -f $outputFolder
if (-not (Test-Path "$outputFolder\sqldk.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqldk.pdb/1d3fa75eb35540e287b2e012d69785df2/sqldk.pdb' -OutFile "$outputFolder\sqldk.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\sqlmin.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqlmin.pdb/d38058f49e7c4d62970677e4315f1f1c2/sqlmin.pdb' -OutFile "$outputFolder\sqlmin.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\sqllang.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqllang.pdb/cb9e5b8e0483423cb122da4ad87534d52/sqllang.pdb' -OutFile "$outputFolder\sqllang.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\sqltses.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqltses.pdb/13cb00e6ed4d46789fadceb55abddfe92/sqltses.pdb' -OutFile "$outputFolder\sqltses.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\sqlaccess.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqlaccess.pdb/c6d08b108b154f8b8431f090dbaab1c92/sqlaccess.pdb' -OutFile "$outputFolder\sqlaccess.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\qds.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/qds.pdb/c1220065fb9e4e61919175ac9792a2bc2/qds.pdb' -OutFile "$outputFolder\qds.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\hkruntime.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/hkruntime.pdb/f0fd3061c4be4486b3308828ea99276e1/hkruntime.pdb' -OutFile "$outputFolder\hkruntime.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\hkengine.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/hkengine.pdb/f53682311a4e427ba43cc7908850cf9d1/hkengine.pdb' -OutFile "$outputFolder\hkengine.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\hkcompile.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/hkcompile.pdb/f42433ff7c4b4c52b53875da10d4684e1/hkcompile.pdb' -OutFile "$outputFolder\hkcompile.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\SQLOS.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/SQLOS.pdb/961e76a609b04a7c935cd8ad827f23381/SQLOS.pdb' -OutFile "$outputFolder\SQLOS.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)
if (-not (Test-Path "$outputFolder\sqlservr.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqlservr.pdb/e6e24f9a081b42e3b9e22e1f6414b9b22/sqlservr.pdb' -OutFile "$outputFolder\sqlservr.pdb" } # File version 2015.0130.4001.00 ((SQL16_PCU_Main).161028-1734)

copy "..\..\docs\SQLSat696\Demos\LOCK_HASH\base_addresses.txt" ".\ImportXEL\base_addresses.txt"

# SQL Server 2017 RTM CU15+GDR - 14.0.3192.2 - x64 (KB4505225)
$outputFolder = '.\sqlsyms\14.0.3192.2\x64'
mkdir -f $outputFolder
if (-not (Test-Path "$outputFolder\SqlDK.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/SqlDK.pdb/122fc135abf24465ba9e6be0a6274eb32/SqlDK.pdb' -OutFile "$outputFolder\SqlDK.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\sqlmin.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqlmin.pdb/207221dfd01a4ecda2e45a5be4afa8342/sqlmin.pdb' -OutFile "$outputFolder\sqlmin.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\sqllang.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqllang.pdb/7f9c184f5b2944cc8c1bdba07670f8412/sqllang.pdb' -OutFile "$outputFolder\sqllang.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\SqlTsEs.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/SqlTsEs.pdb/9559bc0f3b5e4cef8a84ce08601fe3df2/SqlTsEs.pdb' -OutFile "$outputFolder\SqlTsEs.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\sqlaccess.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqlaccess.pdb/24b12b3f43be4a27a4139c39849ef5e33/sqlaccess.pdb' -OutFile "$outputFolder\sqlaccess.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\qds.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/qds.pdb/efd30261c88042a2a49e19b21f90ef002/qds.pdb' -OutFile "$outputFolder\qds.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\hkruntime.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/hkruntime.pdb/cb6a1cd50b654d0a8474f4ed74255e6c1/hkruntime.pdb' -OutFile "$outputFolder\hkruntime.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\hkengine.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/hkengine.pdb/139c16954bd04111a21c3d8e834e5ea41/hkengine.pdb' -OutFile "$outputFolder\hkengine.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\hkcompile.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/hkcompile.pdb/e0243d6070c94969a2aabfc1c32fb0a61/hkcompile.pdb' -OutFile "$outputFolder\hkcompile.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\SQLOS.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/SQLOS.pdb/439bff18122840658f0e9240ffda29301/SQLOS.pdb' -OutFile "$outputFolder\SQLOS.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)
if (-not (Test-Path "$outputFolder\sqlservr.pdb")) { Invoke-WebRequest -uri 'https://msdl.microsoft.com/download/symbols/sqlservr.pdb/107813d2dfe94332aec5cba570dfa4082/sqlservr.pdb' -OutFile "$outputFolder\sqlservr.pdb" } # File version 2017.0140.3192.02 ((SQLServer2017-CU14).190615-0703)

#------------------------------------------------------------------------------
#    The MIT License (MIT)
#    
#    Copyright (c) 2020 Arvind Shyamsundar
#    
#    Permission is hereby granted, free of charge, to any person obtaining a copy
#    of this software and associated documentation files (the "Software"), to deal
#    in the Software without restriction, including without limitation the rights
#    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
#    copies of the Software, and to permit persons to whom the Software is
#    furnished to do so, subject to the following conditions:
#    
#    The above copyright notice and this permission notice shall be included in all
#    copies or substantial portions of the Software.
#    
#    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
#    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
#    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
#    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
#    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
#    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
#    SOFTWARE.
#
#    This sample code is not supported under any Microsoft standard support program or service. 
#    The entire risk arising out of the use or performance of the sample scripts and documentation remains with you. 
#    In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the scripts
#    be liable for any damages whatsoever (including, without limitation, damages for loss of business profits,
#    business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability
#    to use the sample scripts or documentation, even if Microsoft has been advised of the possibility of such damages.
#------------------------------------------------------------------------------