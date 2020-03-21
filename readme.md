# Installation
Please refer to the Releases section for a ready-to-run set of binaries. As of release 1.5 there is no external installation required other than .NET Framework 4.6.1 or above. We now include the [msdia140.dll](https://blogs.msdn.microsoft.com/calvin_hsia/2016/07/30/whats-in-a-pdb-file-use-the-debug-interface-access-sdk/) and [XELite](https://www.nuget.org/packages/Microsoft.SqlServer.XEvent.XELite/) as part of the ZIP file with the rest of the binaries.

It may be necessary to register the msdia140.dll which is included in the ZIP to prevent .NET exceptions when running the tool. To do this, open an administrative command prompt and use <a href="https://support.microsoft.com/en-us/help/249873/how-to-use-the-regsvr32-tool-and-troubleshoot-regsvr32-error-messages/"> regsvr32 </a> to register this specific DLL such as:

	regsvr32 "C:\Tools\SQLCallstackResolver\SQLCallStackResolver.1.5\msdia140.dll"
	
# Usage
The tool comes with some pre-populated examples in the textboxes, just follow that example with your real-world stack(s).

The textbox on the left accepts the following types of input:

* Individual callstack extracted from XML, with DLL + offset notation
* Multiple callstacks in Histogram XML markup (multiple-instance case of above)
* Older format with just virtual addresses [1]
* dll!Ordinal### format [2]
* Output from SQLDumper (SQLDumpNNNN.TXT file) - at least the sections which have stack frames [5]

In all cases you must provided a symbol search path [3][4]

# Building
We welcome contributions - if you are interested to help, please fork the project, and submit your contributions via pull requests. Do note:

* You will need VS2015 installed to build this solution
* Once you load the SLN, fix the reference to msdia140typelib_clr0200.dll (found under C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow)
* Using Package Manager, add Nuget reference for CLRMD as described at https://www.nuget.org/packages/Microsoft.Diagnostics.Runtime

# Notes

[1] In this case you need to populate the Base Addresses with the output of the following query from the actual SQL instance when the XE was captured:

	select name, base_address from sys.dm_os_loaded_modules where name not like '%.rll'

[2] In this case, you need to press the Module Paths button where you will be prompted to enter the path to a folder containing the images of the DLLs involved. For example you can point to C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\Binn for SQL 2016 RTM CU1

[3] This has to be path to a folder or a set of such paths (can be UNC as well) each separated with a semicolon (;). Use the checkbox to specify if sub-folders need to be checked in each case. If multiple paths might contain matching PDBs, the first path from the left which contained the PDB wins. There is no means to know if the PDB is matched with the build that your are using - you need to ensure that the folder path(s) are correct! 

[4] To obtain public PDBs for major SQL releases, PowerShell scripts are available at https://github.com/arvindshmicrosoft/SQLCallStackResolver/wiki/Obtaining-symbol-files-(.PDB)-for-SQL-Server-Releases

[5] This is partial support at the moment - subsequently I will add a 'cleansing' option where it will strip out just the 'Short Stack Dump' sections and resolve the frames therein.
