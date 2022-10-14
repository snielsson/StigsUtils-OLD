# Stigs Utils

## TODO:

- get test utils from work
- get test utils from work
- get $profile from work, especially the covtest function.

## Interesting libs:

- https://github.com/andrewlock/StronglyTypedId

## Misc. build and project configuration notes:

### Provide access to internal code for test projects:

Project provide test projects access to internal code by adding configurations
like these to the .csproj file:

Using derived test project name:

```xml
    <ItemGroup>
        <InternalsVisibleTo Include="$(AssemblyName).Tests" />
    </ItemGroup>   
```

Using explicit test project name:

```xml
    <ItemGroup>
        <InternalsVisibleTo Include="StigsUtils.SomeProj.Tests" />
    </ItemGroup>   
```

### Dump git log when building project:

Add this to .csproj file to get a dump of the git log when building project:

```xml
    <Target Name="GenerateBuildInfo" AfterTargets="Build">
        <MakeDir Directories="$(ProjectDir)$(ProjectName)" />
        <Exec ContinueOnError="true" IgnoreExitCode="false" Command="  git log &gt; $(ProjectDir)$(ProjectName)/.gitlog.txt" />
    </Target>    
```

# IDEAS for utils:

## Stigs.Utils.Services

- TimeService
- LogService
- ContextService
- AdminService
- CacheService
- MessageService
- HttpService
- NotificationService
- ExcelService

# Guidelines and conventions

### Abbreviations

Abbreviations should be used sparringly and only used for parameters and arguments and not in class and method, and if possible be limited to
the below list of common abbriviations and recommended use.

#### Common Abbriviations

|Abbreviation| Recommended use
|
| ex         | Used for exception instances in catch statements and for exception parameters in methods.
| i, j		 | Used for index variables in loops.
| msg		 | Used to name objects representing some kind of message or message func.
| sb		 | Used to name an instance of a StringBuilder.
| x,y,z		 | Used to name parameters in lambda functions to keep then short and read like mathematical functions.
| _t         | Used to represent an instance of ITimeService.
| _fs         | Used to represent an instance of IFileSystemService.
| ioc        | Typically stands for 'Inversion Of Control'.
| db		| Typically stands for 'Database'.
| conn		| Typically stands for 'Connection'.

Abbreviations should be used sparringly, but a number of

### Naming Conventions

#### Conversion methods
Names of conversion methods used to change or convert one object to another type are typically starts with `To` or `As`. `To` is used when the method is expected to work, and and exception is thrown if it does not work.
`As` is used if the method may fail without throwing an exception.

Example:

```c#
/// this method retuns null if x is not a multiple of y.
int? AsMultiplesOf(int x,int y)

/// this method retuns null if x is not a multiple of y.
int? AsMultiplesOf(int x,int y)
```

#### Common injected dependencies

A number of commonly injected dependencies are typically named as shown in the
table below, where the constructor argument used to initialize the private variable referring to the dependency, does not use the leading underscore. For example, when injecting an `ITimeService`, the constructor argument is named `t` while the private variable referencing the dependency instance is named `_t`.

| Variable name | Type of injected dependency
|
| _t | `ITimeService`.
| _l | `ILogService`.
| _fs | `IFileSystemService`.


#### Ensure... methods

A method have having a name starting with 'Ensure' should check some condition
if the condition is not met, then attempt to ensure that the condition is met.

One example could be a method named `EnsureDirectoryCreated` which checks if
a given path is a directory and if not, attempts to create the directory.

# Reference documentation

### ContextService - rename to AppService ?

ContextService collects a number of fundamental cross cutting services in a
single easy to use service interface providing context info like current request
id, user id, etc. as well as additional helper methods for logging and time etc.

Convention is to inject an IContextService named ctx.

### AdminService

Maybe not the best name. is AppService better ? or maybe rename context service
to appService and then add the admin fucntionlity ?

Functionality needed to create static HTML content for below listed routes.

I imagine using handlebars.net to create templates.

#### Getting Git Log at build time

Add a target to the main project file of a startup project, which creates
a `.buildinfo` directory in the outdir creates a txt file with the git commit
log of the source being build.

```xml
    <Target Name="GenerateBuildInfo" AfterTargets="Build">
        <!-- Dump information into out dir, for use by admin enpoints. -->
        <MakeDir Directories="$(OutDir).buildinfo" />
        <Exec ContinueOnError="true" IgnoreExitCode="false" Command="git log &gt; $(OutDir).buildinfo/git_commit_log.txt" />
    </Target>
```

#### Admin routes

- GET {{prefix}} : Index admin page with summary info and links to details (see
  next routes)
- GET {{prefix}}/version : Detailed info about version and latest git log.
- GET {{prefix}}/log?{{querystring}}:
  - Endpoint for queryieng log (maybe predefined queries can be added to Index
    admin page ?)
- GET|PUT {{prefix}}/log/config
  - Show and edit log configuration.
- GET|PUT {{prefix}}/config : show and edit current configuration.

Default prefix is '/admin'.

### StigsUtils.ExcelService

- separate Nuget package.
- support for reading and writing Excel files.
- Convert Excel document to and from JSON object
  - top level objects with basic
  ```json
  {
    "FileInfo": {},
    "Sheets": {
      "Sheet1": { 
        "Cells": {
          "A1" : {},
          "B1" :  {}}
  
        },
        "Row": [ [{}]    
          
        ],
        "Csv": ""
      } 
    }
  }
  ```
- Convert Excel doc to set of csv files, one for each sheet in doc.

### LogService

- Must provide a query message to get log entries.


- All kinds of useful extension methods.
- TimeService to control system clock and timers, including UtcDateTime and
  TimeGuid.

- TaggingLoggger, ie. Log.Info("dasdadda",("))


- LRU cache with expiry time.

- Support for pr. assembly and environment specific configurations files, using
  standard .Net 5 config support
- Support for "ServiceContext"
- Logging extensions using ServiceContext data as part of structured logging.

- General/Generic Static Html Page renderer to quickly dump HTML files.
  - Stigs.Utils.Html project
    - Use HandleBars.Net and therefore a new project.


- MessageService framework with default logging and validation of all messages.
- ActionQueue for background work.
- CacheService providing a hierarchy of LRU caches with diagnostics and stats

- ValueType base class to mitigate Primitive Obsession anti-pattern.
  - get from Thomas Levesq blog.

- Dump of log to JSON and HTML file to make it easy to add logging endpoints.

- Build actions to create file during builds with latest Git history.
- Default ApiService endpoints: Ping, get configuration, get logs, get version,
  git git history.
- Default RazorPages to provide GET /api/status and /api/logs
  - status: SemVer Version, Configuration, Git History, link to logs.
- use tailwind.css to make it look nice.

- Sound default setup of Serilog (or roll my own ?)

- Collections:

  - Population: List of doubles with Mean, Mode, Median, Variance, StdDev

  - Mapping
    - Mapping<TKey,TVal>
    - Mapping<TKey1,TKey2,TVal>
    - Mapping<TKey1,TKey2,TKey3>,TVal>
    - Mapping<TKey1,TKey2,TKey3,TKey4>,TVal>
    - Mapping<TKey1>,<IEnumerable<TVal>>
    - Mapping<TKey1,TKey2>,<IEnumerable<TVal>>
    - Mapping<TKey1,TKey2,TKey3>,<IEnumerable<TVal>>
    - Mapping<TKey1,TKey2,TKey3,TKey4>,<IEnumerable<TVal>>


- Consider a "master collection" :
  - circular buffer
  - indices

-   



