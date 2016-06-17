# SsisComponents
This is a library of custom SSIS components that I've found useful when developing ETL solutions.

## Components

### TrimStart
Allows for trimming strings and white space from the beginning of a target column.  A source column can also be specified as a substring to be trimmed from the beginning of the target column.

### TrimAndToUpper
Allows for full Trim of target column as well as ToUpper.  Either or both may be performed.  Resulting strings should be directed to new output columns.

## Build Instructions
1. Ensure that you have SQL Server Data Tools for SQL 2012 installed (https://www.microsoft.com/en-us/download/details.aspx?id=36843).  This will install needed libraries.  If you're targeting a different version of SQL Server, install SSDT for that version and update the references in the projects.
2. Sign each project with a password protected strong name.  This is required for installation into the GAC, which unfortunately the SSIS requires.
3. Build the solution.
4. Install the resulting DLLs of the project into the GAC using the proper gacutil for the version of the .Net framework you're targeting.  The default for the project is v4.0.  Refer to Sample Post Build Scripts.txt in the SsisComponents.Base project for example usage.
5. Install the same DLLs from step 4 into the appropriate DTS subfolder of your SQL Server installation folder.  Refer to Sample Post Build Scripts.txt in the SsisComponents.Base project for example paths.
