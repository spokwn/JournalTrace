# JournalTrace
Parses NTFS Journal entries in a simple user interface

# New filters

1. **Support for Advanced Filters**: 
   - **Inclusions (`&&`)**: Match multiple conditions in a column.
   - **Exclusions (`!!`)**: Exclude specific values from the filter.
   - **OR Conditions (`||`)**: Allow alternative matches in a column.
2. **Multi-Column Filters**: Dynamically parse `FilterOption:Filter` syntax to target specific columns.

## Benefits
- Provides a better control on what we're looking for.
- Increases the customizability of a search.

### Example Usage
- **Basic Search**: `.exe`, this will search by the currently active filtering (the default is name) by if it contains ".exe"
- **Inclusions**: `name:rundll32&&.pf` this will search any entry on a file that contains "rundll32" and ".pf"
- **Exclusions**: `name:.exe!!svchost` this will search any entry on a file that contains ".exe" but doesnt contain "svchost"
- **OR Conditions**: `name:.exe||.dll` this will search any entry on a file that contains either ".exe" or ".dll" 
- **Multiple Column Filter**: `name:.pf;reason:delete` this will search any file that contains ".pf" and its reason contains "delete"

Its not required to declare the filter option(or column), using Inclusions, Exclusions or OR conditions.

## License
This project is subject to the [GNU General Public License v3.0](LICENSE). 

## Licensed content
### OpenByFiledId
[OpenByFileId](https://github.com/nolanblew/openbyfileid) is a wrapper of the OpenByFileId Win32 API in C#. We use this to resolve file identifiers that otherwise wouldn't be readable.

### StCroixSkippers
[StCroixSkippers](https://www.dreamincode.net/forums/blog/1017-stcroixskippers) wrote the wrapper of the UsnJournal Win32 API to C#. Without this wrapper the project wouldn't even see the light of day. 
