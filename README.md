# StigsUtils

Stig Schmidt Nielsson's collection of C# utility code.

## Nuget package and versioning

StigsUtils will become available as a nuget package using semantic versioning.

StigsUtils use [MinVer](https://github.com/adamralph/minver)
to automate simplify versioning at build and pack time, and this is done by
adding git tags, which then are used to calculate the version using git height (
number of commits since last tag) and commit hash.

[This blog post](https://rehansaeed.com/the-easiest-way-to-version-nuget-packages/)
has additional info and examples.

### How to version

MinVer uses git tags to version, so to create a new version, add a tag and build
like so:

```ps
git tag -a 0.0.1 -m "Initial"
git push --tags
dotnet build
```

The `-a` switch means an annotated tag that exists as its own object as opposed
to light weight tags.

This results in assembly attributes being applied to the assembly built for the
project using SemVer, for example:

```cs
[assembly: AssemblyVersion("0.0.0.0")]
[assembly: AssemblyFileVersion("0.0.1.0")]
[assembly: AssemblyInformationalVersion("0.0.1+362b09133bfbad28ef8a015c634efdb35eb17122")]
```

When dotnet pack is then executed to create the Nuget package these assembly
attributes are used to version the Nuget package.

Note that when using SemVer for .Net :

- AssemblyVersion (the version actually used by the CLR when including packages)
  should always be `{major}.0.0.0`
- AssemblyFileVersion (no runtime effect, shown in windows when hovering over
  the dll) should be always be `{major}.{minor}.{patch}.0`
- AssemblyInformationalVersion (no runtime effect, shown as product version)
  Is just a string with no required format, but should be the full generated
  SemVer version, and in MinVer's case include pre-release tag and version as
  well as git height and git sha. 

