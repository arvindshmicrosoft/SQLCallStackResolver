[![](https://github.com/arvindshmicrosoft/SQLCallStackResolver/workflows/Build%20SQLCallStackResolver/badge.svg)](https://github.com/arvindshmicrosoft/SQLCallStackResolver/actions)
# Installation
Please refer to the Releases section for a ready-to-run set of binaries. Release 2.0 is self-contained, XCOPY / UnZip and run - no external installation required other than .NET Framework 4.7.2 or above. We now include the [msdia140.dll](https://blogs.msdn.microsoft.com/calvin_hsia/2016/07/30/whats-in-a-pdb-file-use-the-debug-interface-access-sdk/) and [XELite](https://www.nuget.org/packages/Microsoft.SqlServer.XEvent.XELite/) as part of the ZIP file with the rest of the binaries. Note that SQLCallStackResolver 2.0 is released as purely for 64-bit Intel/AMD family (x64) OS.

Release 2.0 also uses registration-free COM activation of msdia140.dll, which is included in the ZIP. There is no longer a need to explicitly register this DLL.

Note: DIA, and associated necessary Visual C++ runtime dependency DLLs (msvcp140.dll, vcruntime140.dll and vcruntime140_1.dll are redistributable components of Visual Studio 2019 subject to terms as published [here](https://docs.microsoft.com/en-us/visualstudio/releases/2019/redistribution).

# Usage
The tool comes with a pre-populated example in the textboxes, just follow that example with your real-world stack(s). The textbox on the left accepts the following types of input:

* Individual callstack extracted from XML, with DLL + offset notation
* Multiple callstacks in Histogram XML markup (multiple-instance case of above)
* Older format with just virtual addresses [1]
* dll!Ordinal### format [2]
* Output from SQLDumper (SQLDumpNNNN.TXT file) - at least the sections which have stack frames [5]

In all cases you must provided a symbol search path [3][4]

# Community
[![Join the chat at https://gitter.im/SQLCallStackResolver/community](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/SQLCallStackResolver/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

# Contributing
We welcome contributions - if you are interested to help, please fork the project, and submit your contributions via pull requests. Do note:

* You will need Visual Studio 2019+ installed to build this solution.
* Once you load the SLN, you might need to fix the reference to msdia140typelib_clr0200.dll (found under <<VisualStudioFolder>>\Team Tools\Performance Tools\Plugins\ or <<VisualStudioFolder>>\Common7\IDE\Extensions\TestPlatform). You can also get this file by installing [Performance Tools for Visual Studio 2019](https://visualstudio.microsoft.com/downloads/#performance-tools-for-visual-studio-2019)
* Access to NuGet is needed to fetch and restore package dependencies.
* Tests are implemented using [xUnit](https://xunit.net/docs/getting-started/netfx/visual-studio#run-tests-visualstudio). Make sure you follow instructions and ensure that the tests are passing before submitting a PR.
* When a PR is submitted, there is a GitHub Actions workflow which will build the project and run tests. PRs cannot merge till the workflow succeeds.

# Notes
[1] In this case you need to populate the Base Addresses with the output of the following query from the actual SQL instance when the XE was captured:

	select name, base_address from sys.dm_os_loaded_modules where name not like '%.rll'

[2] In this case, you need to press the Module Paths button where you will be prompted to enter the path to a folder containing the images of the DLLs involved. For example you can point to C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\Binn for SQL 2016 RTM CU1

[3] This has to be path to a folder or a set of such paths (can be UNC as well) each separated with a semicolon (;). Use the checkbox to specify if sub-folders need to be checked in each case. If multiple paths might contain matching PDBs, the first path from the left which contained the PDB wins. There is no means to know if the PDB is matched with the build that your are using - you need to ensure that the folder path(s) are correct!

[4] To obtain public PDBs for major SQL releases, PowerShell scripts are available at https://github.com/arvindshmicrosoft/SQLCallStackResolver/wiki/Obtaining-symbol-files-(.PDB)-for-SQL-Server-Releases

[5] This is partial support at the moment - subsequently I will add a 'cleansing' option where it will strip out just the 'Short Stack Dump' sections and resolve the frames therein.
