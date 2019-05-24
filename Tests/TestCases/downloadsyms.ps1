$msdlurl = "https://msdl.microsoft.com/download/symbols/"

### TestBlockResolution
$localpath = ".\TestBlockResolution\kernelbase.pdb"
if (-not (test-path $localpath))
{
    Invoke-WebRequest -UseBasicParsing -uri ($msdlurl + "kernelbase.pdb/E26F9607943644BB8CDE6C806006A3F01/kernelbase.pdb") -OutFile $localpath
}

### TestOrdinal
$localpath = ".\TestOrdinal\sqldk.pdb"
if (-not (test-path $localpath))
{
    Invoke-WebRequest -UseBasicParsing -uri ($msdlurl + "sqldk.pdb/6a1934433512464b8b8ed905ad930ee62/sqldk.pdb") -OutFile $localpath
}

$localpath = ".\TestOrdinal\sqldk.dll"
if (-not (test-path $localpath))
{
    Write-Warning "You must manually download CU14 for SQL 2016 SP1 (KB 4488535) and extract the sqldk.dll from that install to TestBlockResolution\sqldk.dll"
}

#------------------------------------------------------------------------------
#<copyright company="Microsoft">
#    The MIT License (MIT)
#    
#    Copyright (c) 2017 Microsoft
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
#</copyright>
#------------------------------------------------------------------------------