# MS-SQLParserInsertToMerge
Tool for translating INSERT to MERGE
# Fuctional
Fixes the problem of not being able to generate scripts in the Insert Or Update format in SQL Server Management Studio
# How to use:
1.Generate script in SQL Server Management Studio with that parametrs
![Параметры скрипта](https://user-images.githubusercontent.com/34218775/161938659-65135bcb-1976-4b6c-bea7-e7d2898f0a39.jpg)

2.Drag and drop file with script to application **or** open application and write file name with extension (input.sql) (The file must be in the same folder)
3.As a result, the created file with name - **MergeScript.sql** will be placed in the folder with the application
# Problems
1. Crashes may occur with some unspecified data formats from column values
2. There is no way to control the order of script generation in SQL Server Management Studio without unnecessary clutter of the interface, and therefore some dependent data from one set may not be added.
**Solution**: Run the script multiple times
