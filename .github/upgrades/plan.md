# .NET 10.0 Migration Plan
## Gestion des Vacataires - .NET Framework 4.8 to .NET 10.0 LTS

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Project-by-Project Migration Plans](#project-by-project-migration-plans)
5. [Risk Management](#risk-management)
6. [Testing & Validation Strategy](#testing--validation-strategy)
7. [Complexity & Effort Assessment](#complexity--effort-assessment)
8. [Source Control Strategy](#source-control-strategy)
9. [Success Criteria](#success-criteria)

---

## Executive Summary

### Project Overview

**Application**: Gestion des Vacataires  
**Type**: Windows Forms Desktop Application  
**Current Framework**: .NET Framework 4.8  
**Target Framework**: .NET 10.0 (Long Term Support)  
**Repository**: https://github.com/force-putsh/Gestion-des-Vacataires  
**Migration Branch**: upgrade-to-NET10

### Scope

This plan covers the migration of a single Windows Forms application that manages personnel schedules ("Gestion des Vacataires"). The application includes:

- **2 Forms**: Welcome/Login (`Acceuil`), Dashboard (`Dashbord`)
- **2 User Controls**: Dashboard component (`UCDashbord`), Schedule filter (`Filtre Emploi de Temps`)
- **Data Access**: Entity Framework 6.2.0 with SQL Server
- **Localization**: French language resources
- **Database**: SQL Server 2022 (PT62\SQL2022, Database: Gestion_Etudiants)

### Discovered Metrics

- **Project Count**: 1 (single application)
- **Dependency Depth**: 0 (no project dependencies)
- **Package Count**: 15 NuGet packages
  - 4 require updates
  - 4 require removal (built into .NET 10)
  - 3 deprecated (require replacement)
  - 4 compatible (minor updates recommended)
- **Code Files**: ~15-20 C# files
- **Forms/Controls**: 4 UI components
- **Risk Level**: Medium (Windows Forms designer, database providers)

### Complexity Assessment

**Classification**: **Simple Solution**

**Justification**:
- Single project with no inter-project dependencies
- Clear, linear upgrade path
- All packages have .NET 10-compatible versions
- Well-documented migration patterns for Windows Forms + EF6
- No circular dependencies or complex architecture

### Selected Strategy

**All-At-Once Strategy**

**Rationale**:
- Single project makes atomic upgrade feasible
- No coordination needed across multiple projects
- Faster completion time (1-2 weeks vs multi-phase approach)
- Lower complexity - all changes in one coordinated operation
- Clear testing boundary - entire application at once

**Strategy Characteristics**:
- Upgrade project file, packages, and code in single operation
- Test as complete unit after migration
- Single commit for entire upgrade (preferred)
- No intermediate multi-targeting states

### Critical Issues

**High Priority**:
1. **Database Provider Migration** (Breaking Change)
   - Replace `System.Data.SqlClient` ? `Microsoft.Data.SqlClient`
   - Update all using statements and connection code
   - Update Entity Framework provider configuration

2. **Windows Forms Designer Compatibility**
   - Designer-generated code must work in .NET 10
   - Forms must open in Visual Studio 2022+ designer
   - Localized resources (French `.resx`) must load correctly

3. **Package Updates** (11 of 15 packages)
   - EntityFramework: 6.2.0 ? 6.5.1
   - MySql.Data: 8.0.28 ? 9.1.0+
   - 4 packages to remove (built into .NET 10)

**Medium Priority**:
1. Application entry point modernization (`Program.cs`)
2. Configuration file cleanup (App.config)
3. WPF interop verification (PresentationCore/Framework references)

### Timeline

**Estimated Duration**: 1-2 weeks (single developer)

**Remaining Iterations**: 2-3 detail iterations
- Iteration 2.1: Foundation sections (Dependency Analysis, Migration Strategy)
- Iteration 2.2: Project details and packages
- Iteration 2.3: Risk, Testing, Success Criteria

### Recommended Approach

**Incremental Within All-At-Once**:
1. ? Convert project to SDK-style format
2. ? Update all packages atomically
3. ? Fix all compilation errors in one pass
4. ? Test entire application as unit
5. ? Single commit when all validations pass

---

## Migration Strategy

### Approach Selection

**Selected Strategy**: **All-At-Once Strategy**

### Justification

**Why All-At-Once is Appropriate**:

1. **Single Project** - Only one project to upgrade (no coordination complexity)
2. **No Dependencies** - No inter-project dependencies to manage
3. **Small Codebase** - ~15-20 source files (manageable in single operation)
4. **Clear Compatibility** - All packages have .NET 10 versions available
5. **Fast Completion** - Atomic upgrade completes in 1-2 weeks vs extended timeline
6. **Lower Overhead** - No need for multi-targeting or incremental states

**Advantages for This Solution**:
- Fastest path to .NET 10
- Single testing cycle
- No intermediate broken states
- Clean migration without temporary code
- All benefits realized immediately

### All-At-Once Strategy Rationale

**Core Principle**: Upgrade the entire project, all packages, and all code in a single coordinated operation.

**Execution Approach**:
- All project file changes together
- All package updates together
- All code fixes together
- Build and validate as complete unit
- Single commit when complete (preferred)

**Atomic Operation Scope**:
- Project file conversion (SDK-style)
- Target framework update (net10.0-windows)
- All package updates/removals/additions
- All using statement updates
- All code compatibility fixes
- Configuration file updates

### Dependency-Based Ordering

**Not Applicable** - Single project has no project dependencies.

**Within-Project Ordering**:
1. **Foundation First** - Project file and target framework
2. **Dependencies Next** - Package updates and migrations
3. **Configuration** - App.config and provider updates
4. **Code Changes** - Using statements and API updates
5. **Build Validation** - Compile and fix errors
6. **Testing** - Functional validation

### Parallel vs Sequential Execution

**Sequential Within Atomic Upgrade**:

Given the single project scope, operations proceed sequentially within the atomic upgrade:

```
Project File Conversion
    ?
Target Framework Update
    ?
Package Migration (packages.config ? PackageReference)
    ?
Package Updates/Removals/Additions (atomic batch)
    ?
Configuration Updates (App.config)
    ?
Code Updates (using statements, Program.cs)
    ?
Build & Fix Compilation Errors
    ?
Rebuild & Verify 0 Errors
    ?
Testing & Validation
```

**No Parallel Opportunities** - Single project executes linearly.

### Phase Definitions

#### Phase 0: Preparation (Optional - Prerequisites)

**Goal**: Ensure environment ready for migration

**Activities**:
- ? Verify .NET 10 SDK installed (`dotnet --list-sdks`)
- ? Verify Visual Studio 2022 17.12+ or VS 2025 installed
- ? Create backup/tag current state (`git tag v1.0-netframework48`)
- ? Ensure on upgrade branch (`upgrade-to-NET10`)
- ? Document current functionality (screenshots of forms)

**Deliverables**:
- Environment ready
- Backup created
- Baseline documented

**Skip If**: Environment already verified during assessment phase

#### Phase 1: Atomic Upgrade

**Goal**: Migrate project to .NET 10 in single coordinated operation

**Operations** (performed as atomic batch):

1. **Project File Conversion**
   - Convert from legacy format to SDK-style
   - Update TargetFramework: `v4.8` ? `net10.0-windows`
   - Add `<UseWindowsForms>true</UseWindowsForms>`
   - Remove explicit file inclusions (use SDK auto-globbing)
   - Keep OutputType as `WinExe`

2. **Package Migration**
   - Delete `packages.config` file
   - Migrate to `<PackageReference>` in .csproj
   - Update all package versions atomically:
     - EntityFramework: 6.2.0 ? 6.5.1
     - MySql.Data: 8.0.28 ? 9.1.0
     - Newtonsoft.Json: 13.0.1 ? 13.0.3
     - BouncyCastle: 1.8.5 ? 2.4.0
     - Google.Protobuf: 3.14.0 ? 3.28.2
     - K4os.Compression.LZ4: 1.2.6 ? 1.3.8
     - K4os.Compression.LZ4.Streams: 1.2.6 ? 1.3.8
     - K4os.Hash.xxHash: 1.0.6 ? 1.0.8
   - Remove built-in packages:
     - ? System.Buffers
     - ? System.Memory
     - ? System.Numerics.Vectors
     - ? System.Runtime.CompilerServices.Unsafe
   - Remove deprecated packages:
     - ? System.Data.SqlClient
   - Add new packages:
     - ? Microsoft.Data.SqlClient 5.2.2
     - ? System.Configuration.ConfigurationManager 9.0.0

3. **Configuration Updates (App.config)**
   - Remove `<startup>` section
   - Update EntityFramework provider: `System.Data.SqlClient` ? `Microsoft.Data.SqlClient`
   - Update connection string provider: `System.Data.SqlClient` ? `Microsoft.Data.SqlClient`
   - Remove `<runtime><assemblyBinding>` section (not needed in .NET 10)
   - Keep `<entityFramework>` section for EF6 compatibility

4. **Code Updates**
   - Update `Program.cs`:
     - Replace `Application.EnableVisualStyles()` + `Application.SetCompatibleTextRenderingDefault(false)`
     - With: `ApplicationConfiguration.Initialize()`
   - Update all files with `using System.Data.SqlClient;`:
     - Replace with `using Microsoft.Data.SqlClient;`
   - Review for any hardcoded provider names in code

5. **Build & Fix Errors**
   - Run `dotnet restore`
   - Run `dotnet build`
   - Fix all compilation errors discovered
   - Address breaking changes from package updates
   - Rebuild to verify 0 errors

**Deliverables**:
- Solution builds with 0 errors
- All packages compatible with .NET 10
- Configuration migrated
- Code updated for new providers

**Success Criteria**:
- ? Project file is valid SDK-style format
- ? `dotnet build` succeeds
- ? 0 compilation errors
- ? All packages restored successfully
- ? No deprecated packages remain

#### Phase 2: Test Validation

**Goal**: Verify application functions correctly on .NET 10

**Operations**:

1. **Designer Validation**
   - Open each form in Visual Studio designer
   - Verify all controls visible and positioned
   - Check for designer errors

2. **Database Connectivity Testing**
   - Test connection to SQL Server (PT62\SQL2022)
   - Verify Entity Framework context initializes
   - Test basic CRUD operations

3. **Functional Testing**
   - Test `Acceuil` form (welcome/login)
   - Test `Dashbord` form (dashboard display)
   - Test `UCDashbord` user control
   - Test `Filtre Emploi de Temps` (schedule filter)
   - Verify all navigation works
   - Test data loading and display

4. **Localization Testing**
   - Verify French resources load correctly
   - Check all localized strings display
   - Test embedded resources

5. **Performance Testing**
   - Measure application startup time
   - Compare database query performance
   - Monitor memory usage

6. **Issue Resolution**
   - Fix any bugs discovered during testing
   - Address performance issues
   - Verify fixes don't break other functionality

**Deliverables**:
- All tests pass
- No regressions identified
- Performance acceptable
- Documentation updated

**Success Criteria**:
- ? All forms open in designer
- ? Database connectivity works
- ? All functional tests pass
- ? French localization works
- ? Performance ? baseline
- ? 0 critical bugs

### Risk-Aware Execution

**High-Risk Operations** (require extra caution):

1. **Project File Conversion** - Can break solution loading
   - Mitigation: Keep backup, use version control, validate syntax

2. **Package Updates** - Can introduce breaking changes
   - Mitigation: Update to known-compatible versions, test incrementally

3. **Database Provider Change** - Breaking change
   - Mitigation: Update all references consistently, test thoroughly

4. **Form Designer** - May not load after migration
   - Mitigation: Test each form, have screenshots for reference

**All-At-Once Risk Factors**:
- Higher initial risk (all changes at once)
- Larger testing surface initially
- Potential for more errors to fix simultaneously
- Requires good error handling and debugging skills

**Mitigation for All-At-Once**:
- Comprehensive testing plan
- Version control for easy rollback
- Systematic error resolution
- Clear success criteria at each step

### Testing Checkpoints

**After Project File Conversion**:
- [ ] Solution loads in Visual Studio 2022+
- [ ] Project file syntax is valid
- [ ] TargetFramework is `net10.0-windows`

**After Package Updates**:
- [ ] `dotnet restore` succeeds
- [ ] No package conflicts
- [ ] All packages compatible with net10.0-windows

**After Code Changes**:
- [ ] Code compiles without errors
- [ ] No deprecated API warnings
- [ ] Using statements correct

**After Build Success**:
- [ ] Application starts
- [ ] No runtime errors during startup
- [ ] Main form loads

**Before Production**:
- [ ] All functional tests pass
- [ ] Performance acceptable
- [ ] Documentation complete
- [ ] Stakeholder approval

### Rollback Strategy

**If Migration Fails**:

1. **Immediate Rollback**:
   ```
   git checkout master
   git log upgrade-to-NET10  # Review what was attempted
   ```

2. **Partial Rollback** (if some progress salvageable):
   ```
   git reset --hard <last-good-commit>
   ```

3. **Analyze Failure**:
   - Review compilation errors
   - Check package compatibility issues
   - Verify environment setup
   - Consult migration documentation

4. **Retry with Fixes**:
   - Address identified issues
   - Attempt migration again
   - Use more granular commits for checkpoints

**Rollback Decision Points**:
- If > 50 compilation errors after initial build
- If critical packages incompatible
- If forms won't load in designer at all
- If database connectivity completely broken
- If timeline exceeds 2x estimate

### Source Control Strategy for All-At-Once

**Recommended Approach**: Single commit when all validations pass

**Commit Strategy**:

**Option 1: Single Commit (Preferred)**
```
git add .
git commit -m "Migrate to .NET 10.0 LTS

- Convert project to SDK-style format
- Update target framework to net10.0-windows
- Update all NuGet packages to .NET 10 compatible versions
- Replace System.Data.SqlClient with Microsoft.Data.SqlClient
- Update Program.cs entry point to use ApplicationConfiguration.Initialize()
- Update App.config for .NET 10 compatibility
- All tests pass, 0 regressions

Closes #[issue-number]"
```

**Benefits**:
- Clean history - single logical change
- Easy to revert if issues found later
- Clear before/after state

**Option 2: Checkpoint Commits (If issues encountered)**
```
Commit 1: "Convert project to SDK-style format"
Commit 2: "Update all NuGet packages"
Commit 3: "Update code for .NET 10 compatibility"
Commit 4: "Fix compilation errors"
Commit 5: "Update tests and documentation"
```

**Benefits**:
- Can revert to specific checkpoint
- Easier to identify what change caused an issue
- More granular history

**Recommendation**: Start with Option 1 (single commit). If issues arise during migration, switch to Option 2 (checkpoints) for remaining work.

### Timeline

**Phase 0: Preparation** - 2-4 hours (if needed)
**Phase 1: Atomic Upgrade** - 1-2 days (8-16 hours)
**Phase 2: Test Validation** - 2-3 days (16-24 hours)

**Total Estimated Duration**: 1-2 weeks (single developer)

---

## Detailed Dependency Analysis

### Project Structure

**Single Project Solution**:

```
Gestion des Vacataires.sln
??? Gestion des Vacataires.csproj (Windows Forms App)
    ??? Forms: Acceuil.cs, Dashbord.cs
    ??? User Controls: UCDashbord.cs, Filtre Emploi de Temps.cs
    ??? Data: InfoEmploiDeTemps.cs
    ??? Resources: Multiple .resx files (French localization)
    ??? Configuration: App.config
```

**Project Type**: WinExe (Windows Forms Application)  
**Current Target**: .NET Framework 4.8  
**Proposed Target**: net10.0-windows

### Dependency Graph

**No Project Dependencies**: This is a standalone application with no references to other projects in the solution.

**External Dependencies**:

```
Gestion des Vacataires.csproj
?
??? Data Access
?   ??? EntityFramework 6.2.0 ? 6.5.1
?   ??? EntityFramework.SqlServer (included in EF package)
?   ??? System.Data.SqlClient 4.8.3 ? Microsoft.Data.SqlClient 5.2.x ?? BREAKING
?   ??? MySql.Data 8.0.28 ? 9.1.0+ ?? CRITICAL UPDATE
?
??? Serialization
?   ??? Newtonsoft.Json 13.0.1 ? 13.0.3
?   ??? Google.Protobuf 3.14.0 ? 3.28.x
?
??? Security
?   ??? BouncyCastle 1.8.5 ? 2.4.0
?
??? Compression (MySQL dependencies)
?   ??? K4os.Compression.LZ4 1.2.6 ? 1.3.8
?   ??? K4os.Compression.LZ4.Streams 1.2.6 ? 1.3.8
?   ??? K4os.Hash.xxHash 1.0.6 ? 1.0.8
?
??? System Packages (REMOVE - Built into .NET 10)
?   ??? System.Buffers 4.5.1 ? REMOVE
?   ??? System.Memory 4.5.4 ? REMOVE
?   ??? System.Numerics.Vectors 4.5.0 ? REMOVE
?   ??? System.Runtime.CompilerServices.Unsafe 5.0.0 ? REMOVE
?
??? Configuration (ADD for App.config support)
    ??? System.Configuration.ConfigurationManager ? ADD
```

### Migration Order

**All-At-Once Approach**: Since there's only one project, migration order is straightforward:

**Phase 0: Preparation** (if needed)
- Verify .NET 10 SDK installed
- Ensure Visual Studio 2022 17.12+ or VS 2025
- Backup current state

**Phase 1: Atomic Upgrade** (single coordinated operation)
1. Convert project file to SDK-style format
2. Update TargetFramework to net10.0-windows
3. Migrate packages.config to PackageReference
4. Update all package versions atomically
5. Remove built-in packages
6. Add Microsoft.Data.SqlClient (replace System.Data.SqlClient)
7. Add System.Configuration.ConfigurationManager
8. Update App.config (provider names, remove startup section)
9. Update Program.cs (ApplicationConfiguration.Initialize)
10. Update all `using System.Data.SqlClient` ? `using Microsoft.Data.SqlClient`
11. Build and fix all compilation errors
12. Verify solution builds with 0 errors

**Phase 2: Test Validation**
1. Test all forms open in designer
2. Test database connectivity
3. Execute functional tests
4. Verify French localization
5. Performance testing

### Critical Path

**No dependencies to manage** - single project can be upgraded atomically.

**Critical Sequence Within Atomic Upgrade**:
1. Project file conversion MUST complete before package updates
2. Package updates MUST complete before code changes
3. Code changes MUST complete before build
4. Build MUST succeed before testing

### Circular Dependencies

**None** - Single project solution has no circular dependencies.

### Package Dependency Considerations

**EntityFramework 6.5.1** requires:
- Microsoft.Data.SqlClient (for SQL Server provider)
- .NET 10 compatible runtime

**MySql.Data 9.1.0+** requires:
- Updated compression libraries (K4os.*)
- .NET 10 compatible runtime

**Microsoft.Data.SqlClient** replaces:
- System.Data.SqlClient (deprecated in modern .NET)

**Built-in packages to remove**:
- System.Buffers, System.Memory, System.Numerics.Vectors, System.Runtime.CompilerServices.Unsafe are now part of .NET 10 runtime

### External System Dependencies

**Database**: SQL Server 2022 (PT62\SQL2022)
- Database: Gestion_Etudiants
- Connection must work with Microsoft.Data.SqlClient
- Connection string remains same (just provider name changes)

**Development Environment**:
- Visual Studio 2022 17.12+ or Visual Studio 2025
- .NET 10 SDK
- Windows 10 1809+ or Windows 11

**Runtime Environment**:
- .NET 10 Desktop Runtime (Windows Forms support)
- Windows OS (Windows Forms is Windows-only)

---

## Project-by-Project Migration Plans

### Project: Gestion des Vacataires.csproj

#### Current State

**Project Type**: Windows Forms Application (WinExe)  
**Current Target Framework**: .NET Framework 4.8 (v4.8)  
**Project File Format**: Legacy (non-SDK style)  
**Package Management**: packages.config  
**Lines of Code**: ~1,500-2,000 (estimated)

**Dependencies**:
- EntityFramework 6.2.0
- MySql.Data 8.0.28
- System.Data.SqlClient 4.8.3
- Newtonsoft.Json 13.0.1
- BouncyCastle 1.8.5
- Google.Protobuf 3.14.0
- K4os.Compression.LZ4 1.2.6
- K4os.Compression.LZ4.Streams 1.2.6
- K4os.Hash.xxHash 1.0.6
- System.Buffers 4.5.1
- System.Memory 4.5.4
- System.Numerics.Vectors 4.5.0
- System.Runtime.CompilerServices.Unsafe 5.0.0
- EntityFramework.fr 6.2.0 (French localization)

**Key Components**:
- Forms: `Acceuil.cs`, `Dashbord.cs`
- User Controls: `UCDashbord.cs`, `Filtre Emploi de Temps.cs`
- Data: `InfoEmploiDeTemps.cs`
- Resources: Multiple `.resx` files (French)
- Configuration: `App.config`
- Entry Point: `Program.cs`

**Database Connection**:
```
Server: PT62\SQL2022
Database: Gestion_Etudiants
Provider: System.Data.SqlClient (to be replaced)
```

#### Target State

**Target Framework**: .NET 10.0 Windows (`net10.0-windows`)  
**Project File Format**: SDK-style  
**Package Management**: PackageReference (in .csproj)

**Updated Dependencies**:
- EntityFramework 6.5.1 ?
- MySql.Data 9.1.0 ?
- Microsoft.Data.SqlClient 5.2.2 ? (replaces System.Data.SqlClient)
- Newtonsoft.Json 13.0.3 ?
- BouncyCastle 2.4.0 ?
- Google.Protobuf 3.28.2 ?
- K4os.Compression.LZ4 1.3.8 ?
- K4os.Compression.LZ4.Streams 1.3.8 ?
- K4os.Hash.xxHash 1.0.8 ?
- System.Configuration.ConfigurationManager 9.0.0 ? (new)
- EntityFramework.fr 6.5.1 ?

**Removed Dependencies** (now built-in):
- System.Buffers ?
- System.Memory ?
- System.Numerics.Vectors ?
- System.Runtime.CompilerServices.Unsafe ?

#### Migration Steps

##### Step 1: Prerequisites

**Verify Environment**:
```powershell
# Check .NET 10 SDK installed
dotnet --list-sdks
# Should show: 10.x.x

# Check Visual Studio version
# Visual Studio 2022 17.12+ or Visual Studio 2025 required
```

**Backup**:
```powershell
cd "C:\Users\vngounou\source\repos\Gestion-des-Vacataires"
git checkout master
git tag v1.0-netframework48
git checkout upgrade-to-NET10
```

**Document Current State**:
- Take screenshots of all forms
- Document current functionality
- Note any known issues

##### Step 2: Project File Conversion (SDK-Style)

**Current Project File Structure**:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    <OutputType>WinExe</OutputType>
    ...
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="..." />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="..." />
  </ItemGroup>
  ...
</Project>
```

**Target SDK-Style Project File**:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <Nullable>disable</Nullable>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="EntityFramework" Version="6.5.1" />
    <PackageReference Include="EntityFramework.fr" Version="6.5.1" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.2" />
    <PackageReference Include="MySql.Data" Version="9.1.0" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="BouncyCastle.Cryptography" Version="2.4.0" />
    <PackageReference Include="Google.Protobuf" Version="3.28.2" />
    <PackageReference Include="K4os.Compression.LZ4" Version="1.3.8" />
    <PackageReference Include="K4os.Compression.LZ4.Streams" Version="1.3.8" />
    <PackageReference Include="K4os.Hash.xxHash" Version="1.0.8" />
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="9.0.0" />
  </ItemGroup>

  <!-- Keep WPF references if needed -->
  <ItemGroup>
    <FrameworkReference Include="Microsoft.WindowsDesktop.App.WPF" />
  </ItemGroup>
</Project>
```

**Conversion Actions**:
1. Delete `packages.config` file
2. Replace entire `.csproj` content with SDK-style format above
3. Adjust versions if newer compatible versions available
4. Keep `AssemblyInfo.cs` if it exists (set `GenerateAssemblyInfo` to `false`)

##### Step 3: Package Updates

**See Target State section above for complete package list.**

**Critical Package Changes**:

| Package | Action | Old Version | New Version | Reason |
|---------|--------|-------------|-------------|--------|
| EntityFramework | Update | 6.2.0 | 6.5.1 | .NET 10 compatibility |
| EntityFramework.fr | Update | 6.2.0 | 6.5.1 | French localization for EF |
| MySql.Data | Update | 8.0.28 | 9.1.0 | .NET 10 compatibility, critical |
| System.Data.SqlClient | Replace | 4.8.3 | Remove | Deprecated |
| Microsoft.Data.SqlClient | Add | N/A | 5.2.2 | Modern SQL provider |
| Newtonsoft.Json | Update | 13.0.1 | 13.0.3 | Minor update |
| BouncyCastle | Update | 1.8.5 | 2.4.0 | .NET compatibility |
| Google.Protobuf | Update | 3.14.0 | 3.28.2 | Performance/compatibility |
| K4os.Compression.LZ4 | Update | 1.2.6 | 1.3.8 | .NET 10 support |
| K4os.Compression.LZ4.Streams | Update | 1.2.6 | 1.3.8 | .NET 10 support |
| K4os.Hash.xxHash | Update | 1.0.6 | 1.0.8 | Minor update |
| System.Buffers | Remove | 4.5.1 | N/A | Built into .NET 10 |
| System.Memory | Remove | 4.5.4 | N/A | Built into .NET 10 |
| System.Numerics.Vectors | Remove | 4.5.0 | N/A | Built into .NET 10 |
| System.Runtime.CompilerServices.Unsafe | Remove | 5.0.0 | N/A | Built into .NET 10 |
| System.Configuration.ConfigurationManager | Add | N/A | 9.0.0 | App.config support |

**Note on BouncyCastle**: Package name changed from `BouncyCastle` to `BouncyCastle.Cryptography` in v2.x

##### Step 4: Expected Breaking Changes

**1. Database Provider (CRITICAL)**

**Breaking Change**: System.Data.SqlClient ? Microsoft.Data.SqlClient

**Files Requiring Updates**:
- Any file with `using System.Data.SqlClient;`
- Entity Framework configuration (App.config)
- Connection string provider names

**Code Changes**:
```csharp
// BEFORE
using System.Data.SqlClient;

// AFTER
using Microsoft.Data.SqlClient;
```

**App.config Changes**:
```xml
<!-- BEFORE -->
<entityFramework>
  <providers>
    <provider invariantName="System.Data.SqlClient" 
              type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
  </providers>
</entityFramework>

<!-- AFTER -->
<entityFramework>
  <providers>
    <provider invariantName="Microsoft.Data.SqlClient" 
              type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
  </providers>
</entityFramework>
```

```xml
<!-- BEFORE -->
<connectionStrings>
  <add name="DbGestionnaireStagiaireEntities" 
       providerName="System.Data.SqlClient" ... />
</connectionStrings>

<!-- AFTER -->
<connectionStrings>
  <add name="DbGestionnaireStagiaireEntities" 
       providerName="Microsoft.Data.SqlClient" ... />
</connectionStrings>
```

**2. Application Entry Point**

**Breaking Change**: Application initialization API updated

**File**: `Program.cs`

**Code Changes**:
```csharp
// BEFORE
static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Acceuil());
    }
}

// AFTER
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Acceuil());
    }
}
```

**3. BouncyCastle API Changes**

**Breaking Change**: BouncyCastle v2.x has namespace and API changes

**Potential Issues**:
- Namespace: `Org.BouncyCastle.*` remains but some types moved
- Some APIs deprecated or changed signatures
- Check any crypto code using BouncyCastle

**Action**: Review any code using BouncyCastle after upgrade, test thoroughly

**4. MySql.Data 9.x Changes**

**Potential Breaking Changes**:
- Connection string parsing may differ
- Some API behaviors changed
- Better .NET integration (async/await)

**Action**: Test all MySQL connections and operations if used

**5. Configuration Access**

**If code uses App.config directly**:

```csharp
// Still works but requires package
using System.Configuration;  // Add NuGet: System.Configuration.ConfigurationManager

var connectionString = ConfigurationManager.ConnectionStrings["name"].ConnectionString;
```

##### Step 5: Code Modifications

**Required Changes**:

1. **Update All Using Statements** (search and replace):
   ```
   Find: using System.Data.SqlClient;
   Replace: using Microsoft.Data.SqlClient;
   ```

2. **Update Program.cs** (see Step 4, Breaking Change #2)

3. **Review Designer Files**:
   - Open each form in designer: `Acceuil.Designer.cs`, `Dashbord.Designer.cs`
   - Verify no errors
   - Regenerate if necessary

4. **Check for Hardcoded Provider Names**:
   ```csharp
   // Search for any hardcoded references
   "System.Data.SqlClient"  // Replace with "Microsoft.Data.SqlClient"
   ```

5. **Verify Resource Files**:
   - Ensure all `.resx` files still accessible
   - No changes needed typically, but verify

**Areas Requiring Review**:
- Database access code (connection, commands, readers)
- Entity Framework DbContext initialization
- Configuration loading
- Any serialization code (check for BinaryFormatter usage - not allowed in .NET 10)
- WPF interop if used (PresentationCore/PresentationFramework)

##### Step 6: Configuration Changes (App.config)

**Current App.config**:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="entityFramework" ... />
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>
  <entityFramework>
    <providers>
      <provider invariantName="System.Data.SqlClient" ... />
    </providers>
    ...
  </entityFramework>
  <runtime>
    <assemblyBinding>...</assemblyBinding>
  </runtime>
  <connectionStrings>
    <add name="..." providerName="System.Data.SqlClient" ... />
  </connectionStrings>
</configuration>
```

**Updated App.config**:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  
  <!-- REMOVED: <startup> section not needed in .NET 10 -->
  
  <entityFramework>
    <providers>
      <provider invariantName="Microsoft.Data.SqlClient" 
                type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
    </providers>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
      <parameters>
        <parameter value="mssqllocaldb" />
      </parameters>
    </defaultConnectionFactory>
  </entityFramework>
  
  <!-- REMOVED: <runtime><assemblyBinding> not needed in .NET 10 -->
  
  <connectionStrings>
    <add name="DbGestionnaireStagiaireEntities" 
         connectionString="Data Source=PT62\SQL2022;Initial Catalog=Gestion_Etudiants;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;" 
         providerName="Microsoft.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

**Changes Summary**:
- ? Remove `<startup>` section
- ? Update provider: `System.Data.SqlClient` ? `Microsoft.Data.SqlClient`
- ? Remove `<runtime><assemblyBinding>` section
- ? Keep `<entityFramework>` section (EF6 needs it)
- ? Keep `<connectionStrings>` with updated provider

##### Step 7: Testing Strategy

**Unit Testing**:
- Test Entity Framework context initialization
- Test database connections
- Test data queries (read operations)
- Test data modifications (write operations)
- Test business logic

**Integration Testing**:
1. **Database Connectivity**:
   - [ ] Connect to PT62\SQL2022 successfully
   - [ ] Entity Framework initializes without errors
   - [ ] Connection pooling works
   - [ ] Transactions work correctly

2. **Form Testing**:
   - [ ] `Acceuil` form opens in designer
   - [ ] `Acceuil` form displays correctly at runtime
   - [ ] `Dashbord` form opens in designer
   - [ ] `Dashbord` form displays correctly at runtime
   - [ ] Form navigation works
   - [ ] Form events fire correctly

3. **User Control Testing**:
   - [ ] `UCDashbord` loads in parent form
   - [ ] `Filtre Emploi de Temps` loads and functions
   - [ ] User control events work
   - [ ] Data binding works

4. **Data Operations**:
   - [ ] Load `InfoEmploiDeTemps` data
   - [ ] Filter schedules using `Filtre Emploi de Temps`
   - [ ] Display data in dashboard
   - [ ] All CRUD operations function

5. **Localization Testing**:
   - [ ] French resources load (`Acceuil.fr.resx`)
   - [ ] All labels display in French
   - [ ] No missing translations
   - [ ] Culture-specific formatting (dates, numbers)

**UI Testing** (Manual):
- Verify all forms display correctly
- Check layout and control positioning
- Verify fonts and colors
- Test high DPI scaling
- Verify images and icons load
- Test all buttons and menus
- Test all navigation flows

**Performance Testing**:
- Measure cold start time
- Measure warm start time
- Compare database query times
- Monitor memory usage
- Check UI responsiveness

##### Step 8: Validation Checklist

**Build Validation**:
- [ ] Solution loads in Visual Studio 2022+
- [ ] `dotnet restore` completes without errors
- [ ] `dotnet build` succeeds
- [ ] 0 compilation errors
- [ ] < 5 warnings (preferably 0)
- [ ] No deprecated API warnings

**Designer Validation**:
- [ ] `Acceuil.cs` opens in designer
- [ ] `Dashbord.cs` opens in designer
- [ ] No designer errors or warnings
- [ ] All controls visible

**Runtime Validation**:
- [ ] Application starts successfully
- [ ] Main form (`Acceuil`) displays
- [ ] No unhandled exceptions at startup
- [ ] Database connection establishes
- [ ] Entity Framework works

**Functional Validation**:
- [ ] All forms function correctly
- [ ] All user controls function correctly
- [ ] Data loads and displays correctly
- [ ] Schedule filtering works
- [ ] Dashboard updates correctly
- [ ] Navigation works
- [ ] Localization works (French)

**Quality Validation**:
- [ ] Performance ? baseline (not slower)
- [ ] Memory usage acceptable
- [ ] No regressions from .NET Framework version
- [ ] All known bugs remain only known bugs (no new ones)

**Documentation Validation**:
- [ ] README updated with .NET 10 requirements
- [ ] Setup instructions updated
- [ ] Deployment guide updated
- [ ] Known issues documented

#### Risk Level: Medium

**Justification**:
- Windows Forms designer compatibility (medium risk)
- Database provider breaking change (medium risk)
- Multiple package updates (low-medium risk)
- Single project (lower complexity)
- Good test coverage possible

#### Complexity Rating: Medium

**Justification**:
- Project file conversion (medium complexity)
- Multiple package updates (medium complexity)
- Breaking changes in database provider (medium complexity)
- Code changes required but straightforward
- No inter-project dependencies (simpler)

---

## Risk Management

### High-Risk Changes

| Project | Risk Level | Description | Mitigation |
|---------|-----------|-------------|------------|
| Gestion des Vacataires | HIGH | Windows Forms designer compatibility - forms may not load in designer after migration | Test each form individually; have screenshots for reference; use version control for rollback; regenerate designer code if needed |
| Gestion des Vacataires | HIGH | Database provider breaking change (System.Data.SqlClient ? Microsoft.Data.SqlClient) | Update all using statements systematically; update App.config consistently; test database connectivity thoroughly before proceeding |
| Gestion des Vacataires | MEDIUM-HIGH | Entity Framework 6 configuration on .NET 10 | Keep EF6 (supported); update to 6.5.1; test DbContext initialization; verify migrations work; keep App.config for EF configuration |
| Gestion des Vacataires | MEDIUM | Multiple package updates (11 of 15 packages) | Update to known-compatible versions; test after each major package update; check release notes for breaking changes |

### Security Vulnerabilities

**Assessment Finding**: No explicit security vulnerabilities flagged in current packages during assessment phase.

**However, proactive security improvements**:

| Package | Current Version | Security Concern | Remediation |
|---------|----------------|------------------|-------------|
| BouncyCastle | 1.8.5 (old) | Older cryptography library | Update to 2.4.0 (latest stable) for security fixes |
| MySql.Data | 8.0.28 | Older version may have undisclosed issues | Update to 9.1.0 (latest) for security patches |
| EntityFramework | 6.2.0 | Older version | Update to 6.5.1 for any security fixes |

**Action**: Update all packages to latest compatible versions as part of atomic upgrade to minimize security exposure.

### Contingency Plans

#### Contingency 1: Project File Conversion Fails

**Symptoms**:
- Solution won't load in Visual Studio
- Invalid project file errors
- MSBuild errors

**Recovery**:
```powershell
# Rollback to previous version
git checkout HEAD -- "Gestion des Vacataires.csproj"
git checkout HEAD -- packages.config

# Review SDK-style conversion
# Try again with corrected format
```

**Alternative**: Use try-convert tool
```powershell
dotnet tool install -g try-convert
try-convert "Gestion des Vacataires.csproj" --force-web-conversion
```

#### Contingency 2: Package Update Causes Conflicts

**Symptoms**:
- Package restore fails
- Version conflicts
- Incompatible package versions

**Recovery**:
1. Identify conflicting packages from error messages
2. Update one package at a time to isolate issue
3. Check NuGet package pages for compatibility
4. Try intermediate versions if latest doesn't work

**Alternative**: Remove all packages and add back systematically
```xml
<!-- Start with minimal set -->
<PackageReference Include="EntityFramework" Version="6.5.1" />
<PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.2" />
<!-- Add others one by one -->
```

#### Contingency 3: Forms Won't Open in Designer

**Symptoms**:
- Designer throws exceptions
- Controls missing or misplaced
- Designer file corrupted

**Recovery**:
1. **Option A**: Regenerate designer file
   - Open form in code view
   - Make minor change (add comment)
   - Save and reopen designer

2. **Option B**: Manually fix designer code
   - Compare with backup/screenshots
   - Fix control initialization code
   - Verify InitializeComponent() method

3. **Option C**: Recreate form
   - Create new form
   - Copy business logic from old form
   - Recreate UI using designer
   - Copy event handlers

**Preventive**: Take screenshots of all forms before starting migration

#### Contingency 4: Database Connectivity Fails

**Symptoms**:
- Cannot connect to SQL Server
- Provider not found errors
- Entity Framework errors

**Recovery**:

**Issue**: Connection string provider not updated
```xml
<!-- Fix in App.config -->
<add name="DbGestionnaireStagiaireEntities" 
     providerName="Microsoft.Data.SqlClient"  <!-- Must be Microsoft, not System -->
     ... />
```

**Issue**: Using statements not updated
```csharp
// Fix in all code files
using Microsoft.Data.SqlClient;  // Not System.Data.SqlClient
```

**Issue**: EF provider configuration not updated
```xml
<!-- Fix in App.config -->
<provider invariantName="Microsoft.Data.SqlClient"  <!-- Must be Microsoft -->
          type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
```

**Test**:
```csharp
// Create simple test
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Connected!");
}
```

#### Contingency 5: Performance Degrades

**Symptoms**:
- Slower startup
- Slower database queries
- Higher memory usage

**Investigation**:
1. Profile application before and after
2. Check for inefficient code patterns
3. Verify database query plans unchanged
4. Check for debugging symbols in Release build

**Recovery**:
```xml
<!-- Ensure Release configuration optimized -->
<PropertyGroup Condition="'$(Configuration)'=='Release'">
  <Optimize>true</Optimize>
  <DebugType>none</DebugType>
</PropertyGroup>
```

**Consider**: Microsoft.Data.SqlClient is generally faster, so performance should improve. If it degrades significantly, investigate configuration.

#### Contingency 6: Localization Resources Don't Load

**Symptoms**:
- French text missing
- Resource loading errors
- Missing strings

**Recovery**:
1. Verify `.resx` files included in build:
   ```xml
   <ItemGroup>
     <EmbeddedResource Include="**\*.resx" />
   </ItemGroup>
   ```

2. Check resource file properties:
   - Build Action: Embedded Resource
   - Custom Tool: ResXFileCodeGenerator

3. Regenerate designer files for resources:
   - Open `.resx` file
   - Make minor change
   - Save to regenerate `.Designer.cs`

4. Verify resource access in code:
   ```csharp
   // Should still work
   var text = Properties.Resources.SomeString;
   ```

#### Contingency 7: Compilation Errors Exceed 50

**Symptoms**:
- > 50 compilation errors after initial build
- Overwhelming number of errors
- Many obscure error messages

**Recovery**:
1. **Stop and rollback**:
   ```powershell
   git reset --hard HEAD~1
   ```

2. **Incremental approach**:
   - Fix project file issues first
   - Then fix package issues
   - Then fix code issues
   - Commit at each checkpoint

3. **Seek help**:
   - Review .NET upgrade documentation
   - Check community forums
   - Consult Windows Forms migration guides

4. **Consider professional assistance** if stuck

### Risk Mitigation Summary

**Proactive Measures**:
- ? Use version control (Git) - already on `upgrade-to-NET10` branch
- ? Create backup tag before starting
- ? Take screenshots of all forms
- ? Document current functionality
- ? Test in isolated environment first
- ? Follow All-At-Once strategy systematically
- ? Test after each major step
- ? Have rollback plan ready

**Reactive Measures**:
- Contingency plans for each high-risk area
- Clear rollback procedures
- Alternative approaches documented
- Decision points for when to rollback vs continue
- Resources for troubleshooting

**Risk Acceptance**:
- Some risk unavoidable in any migration
- Mitigation reduces but doesn't eliminate risk
- Team must be prepared for troubleshooting
- Buffer time in schedule for issue resolution
- Stakeholder communication important

---

## Testing & Validation Strategy

### Phase-by-Phase Testing Requirements

#### Phase 0: Preparation Testing

**Verification Tests**:
- [ ] .NET 10 SDK installed and accessible
  ```powershell
  dotnet --list-sdks  # Should show 10.x.x
  ```

- [ ] Visual Studio 2022 17.12+ or VS 2025 installed
  ```powershell
  # Check VS version in Help > About
  ```

- [ ] Git repository clean and on correct branch
  ```powershell
  git status  # Should be on upgrade-to-NET10
  git log -1  # Verify latest commit
  ```

- [ ] Backup tag created
  ```powershell
  git tag  # Should show v1.0-netframework48
  ```

**Success Criteria**: All prerequisites verified before proceeding

#### Phase 1: Atomic Upgrade Testing

**After Project File Conversion**:

```powershell
# Test 1: Solution loads
# Open solution in Visual Studio 2022+
# Verify no load errors

# Test 2: Project file valid
dotnet build-server shutdown  # Clear cache
dotnet restore "Gestion des Vacataires.csproj"
# Should complete without errors

# Test 3: Target framework correct
# Open .csproj and verify:
# <TargetFramework>net10.0-windows</TargetFramework>
```

**After Package Updates**:

```powershell
# Test 1: Package restore
dotnet restore
# Should complete without errors
# No package conflicts

# Test 2: Verify packages
dotnet list package
# Verify all packages at expected versions

# Test 3: Check for deprecated packages
# Should NOT see:
# - System.Buffers
# - System.Memory
# - System.Numerics.Vectors
# - System.Runtime.CompilerServices.Unsafe
# - System.Data.SqlClient

# Should see:
# - EntityFramework 6.5.1
# - Microsoft.Data.SqlClient 5.2.2
# - MySql.Data 9.1.0
```

**After Code Changes**:

```powershell
# Test 1: Code compiles
dotnet build
# Should complete without errors

# Test 2: Check warnings
# Review build output for warnings
# Address any deprecation warnings

# Test 3: Verify using statements updated
# Search for "using System.Data.SqlClient" - should find 0 instances
# Search for "using Microsoft.Data.SqlClient" - should find instances
```

**After Build Success**:

```powershell
# Test 1: Application starts
dotnet run
# Or run from Visual Studio
# Should start without crashing

# Test 2: Main form loads
# Acceuil form should display
# No exceptions at startup

# Test 3: No immediate errors
# Check Output window for errors
# Check Event Viewer for .NET errors
```

#### Phase 2: Comprehensive Testing

**Smoke Tests** (Quick validation after each atomic upgrade step):

1. **Build Smoke Test** (2 minutes):
   ```powershell
   dotnet clean
   dotnet build
   # Must succeed with 0 errors
   ```

2. **Startup Smoke Test** (2 minutes):
   - Launch application
   - Verify main form displays
   - Close application
   - No crashes

3. **Database Smoke Test** (3 minutes):
   - Launch application
   - Trigger any database operation
   - Verify no connection errors
   - Close application

**Comprehensive Validation** (Before phase completion):

### Database Connectivity Testing

**Test Suite**:

1. **Connection Test**:
```csharp
// Create test method or console app
using Microsoft.Data.SqlClient;
using System.Configuration;

public void TestConnection()
{
    var connectionString = ConfigurationManager.ConnectionStrings["DbGestionnaireStagiaireEntities"].ConnectionString;
    
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
        
        var command = new SqlCommand("SELECT 1", connection);
        var result = command.ExecuteScalar();
        Assert.AreEqual(1, result);
    }
}
```

2. **Entity Framework Test**:
```csharp
public void TestEntityFramework()
{
    using (var context = new DbGestionnaireStagiaireEntities())
    {
        // Test context initializes
        Assert.IsNotNull(context);
        
        // Test database connection
        Assert.IsTrue(context.Database.Exists());
        
        // Test simple query
        var count = context.YourEntitySet.Count();
        Assert.IsTrue(count >= 0);
    }
}
```

3. **CRUD Operations Test**:
```csharp
public void TestCrudOperations()
{
    using (var context = new DbGestionnaireStagiaireEntities())
    {
        // Test READ
        var items = context.InfoEmploiDeTemps.ToList();
        Assert.IsNotNull(items);
        
        // Test CREATE (if applicable)
        // Test UPDATE (if applicable)
        // Test DELETE (if applicable)
    }
}
```

**Success Criteria**:
- [ ] Connection to PT62\SQL2022 successful
- [ ] Entity Framework context initializes
- [ ] Simple queries execute successfully
- [ ] CRUD operations work (if applicable)
- [ ] No provider not found errors
- [ ] Connection pooling works

### Form Designer Testing

**Test Each Form**:

1. **Acceuil Form**:
   - [ ] Opens in Visual Studio designer
   - [ ] All controls visible
   - [ ] Layout correct (compare to screenshot)
   - [ ] Properties load correctly
   - [ ] No designer errors in Error List
   - [ ] Can edit in designer and save changes
   - [ ] Form displays at runtime
   - [ ] All events work

2. **Dashbord Form**:
   - [ ] Opens in Visual Studio designer
   - [ ] All controls visible
   - [ ] Layout correct
   - [ ] No designer errors
   - [ ] Can edit and save
   - [ ] Displays at runtime
   - [ ] All events work

**Designer Validation Process**:
```
For each form:
1. Open in designer (double-click .cs file or use View Designer)
2. Verify no errors in Error List window
3. Verify all controls visible in designer
4. Compare layout to screenshot taken before migration
5. Click around to verify controls selectable
6. Make minor change (e.g., change a label text)
7. Save
8. Rebuild
9. Run application and verify form displays correctly
10. Undo test change
```

### Functional Testing

**User Control Testing**:

1. **UCDashbord Control**:
   - [ ] Loads in parent form (Dashbord)
   - [ ] Displays correctly
   - [ ] Data binding works
   - [ ] Events fire correctly
   - [ ] No layout issues

2. **Filtre Emploi de Temps Control**:
   - [ ] Loads correctly
   - [ ] Filter functionality works
   - [ ] Events fire correctly
   - [ ] Data filters correctly
   - [ ] UI responsive

**Form Functionality Testing**:

1. **Acceuil Form (Welcome/Login)**:
   - [ ] Displays correctly
   - [ ] Layout correct
   - [ ] All controls functional
   - [ ] Navigation to Dashbord works
   - [ ] Login functionality works (if applicable)
   - [ ] Validation works

2. **Dashbord Form**:
   - [ ] Displays correctly
   - [ ] UCDashbord control loads
   - [ ] Data displays in dashboard
   - [ ] Filter control works
   - [ ] Schedule data displays
   - [ ] All navigation works
   - [ ] All buttons work

**Data Operations Testing**:

1. **Load InfoEmploiDeTemps Data**:
   - [ ] Data loads from database
   - [ ] Displays correctly in UI
   - [ ] No data corruption
   - [ ] Performance acceptable

2. **Schedule Filtering**:
   - [ ] Filter control works
   - [ ] Filter applies correctly
   - [ ] Results accurate
   - [ ] Performance acceptable

3. **Dashboard Display**:
   - [ ] Dashboard loads data
   - [ ] Data displays correctly
   - [ ] Updates work
   - [ ] No errors

### Localization Testing

**French Resources**:

1. **Acceuil Form**:
   - [ ] `Acceuil.fr.resx` loads correctly
   - [ ] All labels in French
   - [ ] No missing translations
   - [ ] No resource loading errors

2. **Other Resources**:
   - [ ] All `.resx` files load
   - [ ] Embedded resources accessible
   - [ ] No "missing resource" errors
   - [ ] Images and icons display

**Culture Testing**:
```csharp
// Test culture-specific formatting
var culture = new CultureInfo("fr-FR");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

// Verify:
// - Date format: dd/MM/yyyy
// - Number format: comma as decimal separator
// - Currency format: Euro
```

### Performance Testing

**Baseline Metrics** (Measure on .NET Framework 4.8 first):

1. **Startup Time**:
   - Cold start (first launch)
   - Warm start (subsequent launches)

2. **Database Operations**:
   - Connection time
   - Simple query time
   - Complex query time
   - Large dataset load time

3. **Memory Usage**:
   - Startup memory
   - After loading data
   - After user operations

4. **UI Responsiveness**:
   - Form load time
   - Control rendering time
   - Navigation time

**Performance Comparison**:

| Metric | .NET Framework 4.8 | .NET 10 | Delta | Acceptable? |
|--------|-------------------|---------|-------|-------------|
| Cold Start | ___ ms | ___ ms | ___ ms | Yes/No |
| Warm Start | ___ ms | ___ ms | ___ ms | Yes/No |
| DB Connection | ___ ms | ___ ms | ___ ms | Yes/No |
| Simple Query | ___ ms | ___ ms | ___ ms | Yes/No |
| Form Load | ___ ms | ___ ms | ___ ms | Yes/No |
| Memory Usage | ___ MB | ___ MB | ___ MB | Yes/No |

**Acceptance Criteria**:
- Startup time: ? baseline + 10%
- Database queries: ? baseline (should be equal or faster)
- Memory: ? baseline + 20%
- UI: ? baseline + 10%

**Performance Test Tools**:
```csharp
// Measure startup time
var stopwatch = Stopwatch.StartNew();
Application.Run(new Acceuil());
stopwatch.Stop();
Console.WriteLine($"Startup: {stopwatch.ElapsedMilliseconds}ms");

// Measure database operation
stopwatch.Restart();
using (var context = new DbGestionnaireStagiaireEntities())
{
    var data = context.InfoEmploiDeTemps.ToList();
}
stopwatch.Stop();
Console.WriteLine($"DB Query: {stopwatch.ElapsedMilliseconds}ms");
```

### Regression Testing

**Test Matrix**:

| Feature | Test Case | Expected Result | Actual Result | Pass/Fail |
|---------|-----------|----------------|---------------|-----------|
| Acceuil Form | Opens at startup | Form displays correctly | | |
| Dashbord | Navigation from Acceuil | Dashbord opens | | |
| Database | Load schedule data | Data displays | | |
| Filter | Apply schedule filter | Results filtered | | |
| Dashboard | Display data | Data shows correctly | | |
| Localization | French labels | All text in French | | |
| Resources | Images load | All images display | | |

**Regression Test Execution**:
1. Execute all test cases
2. Compare actual vs expected results
3. Document any deviations
4. Investigate root cause of failures
5. Fix issues
6. Retest

**Success Criteria**:
- [ ] 100% of regression tests pass
- [ ] No new bugs introduced
- [ ] All existing features work
- [ ] Performance acceptable

### Test Execution Schedule

**Day 1-2: Atomic Upgrade**
- Checkpoint tests after each major step
- Smoke tests at each stage
- Document issues immediately

**Day 3: Designer & Database Testing**
- Test all forms in designer
- Database connectivity tests
- Entity Framework tests

**Day 4: Functional Testing**
- Form functionality tests
- User control tests
- Data operations tests

**Day 5: Localization & Performance**
- Localization tests
- Performance baseline comparison
- Regression testing

**Day 6-7: Issue Resolution**
- Fix identified issues
- Retest affected areas
- Full regression test

**Day 8: Final Validation**
- Complete regression test
- Performance validation
- Documentation review
- Stakeholder demo

### Test Environment

**Development Environment**:
- OS: Windows 10 1809+ or Windows 11
- IDE: Visual Studio 2022 17.12+ or VS 2025
- Runtime: .NET 10 Desktop Runtime
- Database: SQL Server 2022 (PT62\SQL2022)

**Test Data**:
- Use existing test database if available
- Or use production database (read-only operations)
- Or create test data set

**Test Tools**:
- Visual Studio Test Explorer
- Manual testing checklist
- Performance profiling tools
- Memory profilers if needed

### Defect Management

**Severity Classification**:

- **Critical**: Application won't start or database doesn't connect
- **High**: Major feature broken or significant performance degradation
- **Medium**: Minor feature issue or cosmetic problem
- **Low**: Typo, minor UI issue, non-critical warning

**Defect Resolution Priority**:
1. Critical defects: Must fix before proceeding
2. High defects: Must fix before completion
3. Medium defects: Fix if time allows, or document as known issue
4. Low defects: Document as known issue, fix later

**Defect Tracking**:
```
Defect Log:
- ID
- Description
- Severity
- Steps to reproduce
- Expected vs actual behavior
- Root cause (when identified)
- Fix implemented
- Verification status
```

---

## Complexity & Effort Assessment

### Overall Complexity Rating

**Solution Complexity**: **Simple**

**Justification**:
- Single project (no inter-project dependencies)
- Clear upgrade path (well-documented Windows Forms + EF6 migration)
- All packages have .NET 10-compatible versions
- No circular dependencies
- Straightforward architecture
- Small to medium codebase (~1,500-2,000 LOC)

### Per-Project Complexity

| Project | Complexity | Dependencies | Risk | Effort |
|---------|-----------|--------------|------|--------|
| Gestion des Vacataires.csproj | Medium | 0 project deps, 15 packages | Medium | 1-2 weeks |

**Complexity Factors**:

- **Project File Conversion**: Medium complexity
  - Legacy to SDK-style conversion well-documented
  - Tools available (try-convert)
  - Manual conversion feasible
  
- **Package Updates**: Medium complexity
  - 11 packages need updates/changes
  - All have .NET 10 versions available
  - Breaking change: System.Data.SqlClient ? Microsoft.Data.SqlClient
  
- **Code Changes**: Low-Medium complexity
  - Using statement updates (search/replace)
  - Program.cs entry point update (simple)
  - Configuration updates (straightforward)
  - No major refactoring needed
  
- **Windows Forms Designer**: Medium-High risk/complexity
  - Designer may need form regeneration
  - Resource files (French localization) need verification
  - Visual testing required
  
- **Database Provider**: Medium complexity
  - Breaking change but well-documented
  - Consistent changes across all code
  - EF6 supports new provider

### Phase Complexity Assessment

#### Phase 0: Preparation
- **Complexity**: Low
- **Effort**: 2-4 hours (if needed)
- **Dependencies**: None
- **Risk**: Low

**Rationale**: Standard environment setup and verification

#### Phase 1: Atomic Upgrade
- **Complexity**: Medium
- **Effort**: 8-16 hours (1-2 days)
- **Dependencies**: Phase 0 (environment ready)
- **Risk**: Medium-High

**Rationale**: 
- Project file conversion is well-documented but can have issues
- Multiple package updates increase surface area for issues
- Breaking change in database provider requires systematic updates
- Build errors require troubleshooting skills

**Breakdown**:
- Project file conversion: 2-3 hours
- Package updates: 2-3 hours
- Configuration updates: 1-2 hours
- Code changes: 2-4 hours
- Build and fix errors: 2-6 hours (variable based on issues found)

#### Phase 2: Test Validation
- **Complexity**: Medium
- **Effort**: 16-24 hours (2-3 days)
- **Dependencies**: Phase 1 (must build successfully)
- **Risk**: Medium

**Rationale**:
- Functional testing straightforward
- Designer testing can reveal issues
- Performance testing needs baseline comparison
- Issue resolution time variable

**Breakdown**:
- Designer validation: 2-3 hours
- Database testing: 3-4 hours
- Functional testing: 4-6 hours
- Localization testing: 2-3 hours
- Performance testing: 2-3 hours
- Issue resolution: 3-8 hours (variable)

### Resource Requirements

**Personnel**:

**Option A: Single Developer (Recommended)**
- 1 developer, full-time
- Duration: 1-2 weeks (8-10 business days)
- Total effort: 26-44 hours

**Skills Required**:
- .NET Framework to modern .NET migration experience
- Windows Forms knowledge
- Entity Framework 6 experience
- SQL Server/database knowledge
- Git version control
- Troubleshooting and debugging skills
- Familiarity with MSBuild and project files

**Option B: Two Developers**
- Developer 1: Lead migration (project file, packages, code)
- Developer 2: Testing and validation
- Duration: 1 week (5 business days)
- Total effort: 26-44 hours (split between two)

**Skills Required** (combined):
- Same as Option A, distributed

**Recommendation**: **Option A (Single Developer)**
- Lower coordination overhead
- Better knowledge transfer
- More realistic for small team
- Built-in buffer for issues

**Skill Level**: Intermediate to Senior .NET Developer
- Must understand project file structures
- Must be comfortable with package management
- Must have debugging skills
- Should have migration experience (helpful but not required)

### Dependency Ordering

**No Project Dependencies** - Single project solution

**Execution Order**:
1. Environment setup (if needed)
2. Project file conversion
3. Package updates
4. Configuration updates
5. Code updates
6. Build and fix errors
7. Testing and validation

**Critical Path**: Linear execution, no parallelization opportunities

### Effort Summary by Category

| Category | Estimated Hours | Percentage | Notes |
|----------|----------------|------------|-------|
| **Project Structure** | 2-4 | 8-10% | Project file conversion, target framework |
| **Dependency Updates** | 4-6 | 15-15% | Package updates, migrations, additions |
| **Configuration** | 1-2 | 4-5% | App.config updates |
| **Code Changes** | 4-8 | 15-20% | Using statements, Program.cs, fixes |
| **Build & Debugging** | 2-6 | 8-15% | Compilation errors, warnings |
| **Testing** | 10-16 | 38-40% | All testing phases |
| **Documentation** | 2-4 | 8-10% | README, setup instructions |
| **Buffer** | 3-6 | 12% | Unexpected issues, learning |
| **Total** | **28-52 hours** | **100%** | Full migration effort |

**Realistic Estimate**: **35-45 hours** (accounting for learning curve and typical issues)

**Timeline**:
- **Conservative**: 2 weeks (10 business days) @ 4-5 hours/day
- **Aggressive**: 1 week (5 business days) @ 7-9 hours/day
- **Recommended**: 1.5 weeks (8 business days) @ 5-6 hours/day

### Complexity Ratings by Area

| Area | Complexity | Justification |
|------|-----------|---------------|
| **Project File** | Medium | SDK-style conversion well-documented, but can have issues |
| **Packages** | Medium | Multiple updates, one breaking change (SQL provider) |
| **Configuration** | Low | Straightforward App.config updates |
| **Code** | Low-Medium | Mostly search/replace, minimal refactoring |
| **Build** | Medium | Variable compilation errors to resolve |
| **Testing** | Medium | Comprehensive testing needed, multiple areas |
| **Designer** | Medium-High | Forms may need regeneration, visual validation needed |
| **Database** | Medium | Provider change is breaking but well-documented |
| **Overall** | **Medium** | Manageable complexity, good documentation available |

### Risk-Adjusted Effort

**Base Effort**: 28-52 hours  
**Risk Factor**: 1.2 (20% buffer for unknown issues)  
**Risk-Adjusted**: **34-62 hours**

**Recommended Planning Estimate**: **40-50 hours** (5-7 days single developer)

### Effort Confidence Level

**Confidence**: ?? **Medium-High** (70-80%)

**Factors Increasing Confidence**:
- Single project (simpler)
- Well-documented migration path
- All packages have .NET 10 versions
- No complex architecture
- Clear testing strategy

**Factors Decreasing Confidence**:
- Windows Forms designer can be unpredictable
- Multiple package updates increase surface area
- Breaking change in database provider
- First migration to .NET 10 (if team new to modern .NET)
- Potential hidden issues in legacy code

**Recommendation**: Use conservative estimate (2 weeks) to account for unknowns and provide buffer for issues.

---

## Source Control Strategy

### Branching Strategy

**Current State**:
- **Main Branch**: `master` (stable, .NET Framework 4.8)
- **Upgrade Branch**: `upgrade-to-NET10` (migration work) ? Already created
- **Remote**: `origin` ? https://github.com/force-putsh/Gestion-des-Vacataires

**Branch Structure**:
```
master (stable)
  ??? upgrade-to-NET10 (migration branch)
```

**Branch Protection**:
- `master` remains untouched during migration
- All migration work on `upgrade-to-NET10`
- Merge to `master` only after complete validation

### All-At-Once Source Control Guidance

**Recommended Approach**: **Single commit** when all validations pass

**Rationale**:
- All-At-Once strategy means atomic upgrade
- Single commit represents single logical change
- Easier to revert if issues found later
- Clean Git history

### Commit Strategy

#### Option 1: Single Commit (Preferred for All-At-Once)

**When to Use**: Migration completes successfully without major issues

**Commit Point**: After all tests pass

```bash
# Stage all changes
git add .

# Commit with comprehensive message
git commit -m "Migrate to .NET 10.0 LTS

Complete migration from .NET Framework 4.8 to .NET 10.0 LTS.

Changes:
- Convert project file to SDK-style format
- Update TargetFramework to net10.0-windows
- Migrate from packages.config to PackageReference
- Update EntityFramework 6.2.0 ? 6.5.1
- Update MySql.Data 8.0.28 ? 9.1.0
- Replace System.Data.SqlClient with Microsoft.Data.SqlClient 5.2.2
- Remove built-in packages (System.Buffers, System.Memory, etc.)
- Update all remaining packages to .NET 10-compatible versions
- Update Program.cs entry point (ApplicationConfiguration.Initialize)
- Update all using statements (System.Data.SqlClient ? Microsoft.Data.SqlClient)
- Update App.config (remove startup section, update providers)
- All forms open in designer successfully
- All tests pass (functional, database, localization, performance)
- No regressions identified

Testing:
- Build: Success (0 errors, 0 warnings)
- Database: Connection and EF6 working correctly
- Forms: All forms load in designer and at runtime
- Localization: French resources verified
- Performance: Meets baseline requirements

Closes #[issue-number]
"

# Push to remote
git push origin upgrade-to-NET10
```

**Benefits**:
- Clean history - single logical change
- Easy to review entire migration at once
- Simple to revert if needed: `git revert <commit-hash>`
- Clear before/after state

#### Option 2: Checkpoint Commits (Fallback if Issues Encountered)

**When to Use**: If issues arise and need to track progress incrementally

**Commit Points**: After each major milestone

```bash
# Checkpoint 1: After project file conversion
git add "Gestion des Vacataires.csproj"
git commit -m "Convert project to SDK-style format

- Replace legacy .csproj with SDK-style
- Set TargetFramework to net10.0-windows
- Add UseWindowsForms property
- Remove explicit file inclusions

Status: Project loads in VS2022, not yet building"

# Checkpoint 2: After package updates
git add "Gestion des Vacataires.csproj"
git commit -m "Update all NuGet packages for .NET 10

- Migrate from packages.config to PackageReference
- Update EntityFramework 6.2.0 ? 6.5.1
- Update MySql.Data 8.0.28 ? 9.1.0
- Add Microsoft.Data.SqlClient 5.2.2
- Remove System.Data.SqlClient (deprecated)
- Remove built-in packages
- Update all other packages

Status: Packages restored, compilation errors present"

# Checkpoint 3: After configuration updates
git add App.config
git commit -m "Update App.config for .NET 10 compatibility

- Remove <startup> section (not needed)
- Update EntityFramework provider to Microsoft.Data.SqlClient
- Update connection string provider name
- Remove assembly binding redirects

Status: Configuration migrated"

# Checkpoint 4: After code changes
git add *.cs
git commit -m "Update code for .NET 10 compatibility

- Update Program.cs entry point
- Replace using System.Data.SqlClient with Microsoft.Data.SqlClient
- Fix compilation errors
- Address breaking changes

Status: Builds successfully with 0 errors"

# Checkpoint 5: After testing complete
git add .
git commit -m "Complete testing and validation

- All forms tested in designer
- Database connectivity verified
- Functional tests pass
- Localization verified
- Performance acceptable
- Documentation updated

Status: Migration complete and validated"
```

**Benefits**:
- Can revert to specific checkpoint if needed
- Easier to identify what change caused an issue
- More granular history for debugging
- Progress visible incrementally

### Commit Message Format

**Structure**:
```
<Type>: <Short summary> (max 72 chars)

<Detailed description>

<Changes list>
- Change 1
- Change 2
...

<Testing performed>
- Test 1
- Test 2
...

<Status/Notes>

Closes #<issue-number>
```

**Types**:
- `feat`: New feature or capability
- `fix`: Bug fix
- `refactor`: Code restructuring
- `chore`: Maintenance (dependencies, config)
- `docs`: Documentation
- `test`: Testing additions/changes
- `migrate`: Migration work (use this)

**Example**:
```
migrate: Upgrade to .NET 10.0 LTS

Complete migration from .NET Framework 4.8 to .NET 10.0 Long Term Support.

Changes:
- Project file converted to SDK-style
- All packages updated to .NET 10-compatible versions
- Database provider migrated from System.Data.SqlClient to Microsoft.Data.SqlClient
- Application entry point modernized
- Configuration updated for .NET 10

Testing:
- Build: ? Success
- Database: ? Connectivity verified
- Forms: ? All load correctly
- Localization: ? French resources work
- Performance: ? Meets baseline

No regressions identified.

Closes #123
```

### Review and Merge Process

#### Before Merge to Master

**Pre-Merge Checklist**:
- [ ] All commits pushed to `upgrade-to-NET10` branch
- [ ] Build succeeds with 0 errors, minimal warnings
- [ ] All tests pass
- [ ] Forms open in designer
- [ ] Database connectivity verified
- [ ] Functional testing complete
- [ ] Localization verified
- [ ] Performance acceptable
- [ ] Documentation updated
- [ ] README updated with .NET 10 requirements
- [ ] No pending changes (git status clean)

#### Pull Request (If Using PR Workflow)

**PR Title**: "Migrate to .NET 10.0 LTS"

**PR Description Template**:
```markdown
## Migration Overview

Migrate "Gestion des Vacataires" from .NET Framework 4.8 to .NET 10.0 LTS.

## Changes

### Project Structure
- Converted to SDK-style project format
- Updated target framework to net10.0-windows

### Dependencies
- Updated 9 packages to .NET 10-compatible versions
- Removed 4 built-in packages
- Replaced System.Data.SqlClient with Microsoft.Data.SqlClient

### Code Changes
- Updated Program.cs entry point
- Updated all using statements for new database provider
- Fixed compilation errors

### Configuration
- Updated App.config for .NET 10
- Removed deprecated sections
- Updated Entity Framework provider

## Testing Performed

- ? Build: Success (0 errors, 0 warnings)
- ? Database: Connection and EF6 verified
- ? Forms: All load in designer and runtime
- ? Localization: French resources work
- ? Performance: Meets baseline
- ? Regression: No issues identified

## Breaking Changes

- Database provider: System.Data.SqlClient ? Microsoft.Data.SqlClient
  - **Impact**: Code using SqlClient updated
  - **Mitigation**: All references updated, tested

## Deployment Notes

**New Requirements**:
- .NET 10 SDK for development
- .NET 10 Desktop Runtime for production
- Visual Studio 2022 17.12+ for development

**No database changes required**

## Rollback Plan

If issues found after merge:
```
git revert <merge-commit>
```

## Checklist

- [x] Code compiles without errors
- [x] All tests pass
- [x] Forms work correctly
- [x] Database connectivity verified
- [x] Documentation updated
- [x] No regressions identified

## Related Issues

Closes #123
```

**PR Review Checklist**:
- [ ] Code review by another developer (if team > 1)
- [ ] All tests pass in CI/CD (if configured)
- [ ] No merge conflicts with master
- [ ] Documentation reviewed
- [ ] Deployment plan reviewed

#### Merge to Master

**When Ready to Merge**:

```bash
# Switch to master
git checkout master

# Pull latest (ensure up to date)
git pull origin master

# Merge upgrade branch
git merge upgrade-to-NET10

# Resolve any conflicts (should be none if master untouched)

# Test once more after merge
dotnet build
dotnet run  # Quick smoke test

# Push to remote
git push origin master

# Tag the release
git tag v2.0-net10.0
git push origin v2.0-net10.0
```

**Alternative: Squash Merge** (if used checkpoint commits):

```bash
git checkout master
git merge --squash upgrade-to-NET10
git commit -m "Migrate to .NET 10.0 LTS

Complete migration from .NET Framework 4.8 to .NET 10.0 LTS.
See upgrade-to-NET10 branch history for detailed changes.

Closes #123"
git push origin master
git tag v2.0-net10.0
git push origin v2.0-net10.0
```

**Squash Merge Benefits**:
- Single commit in master (clean history)
- All checkpoint commits preserved in feature branch
- Easy to reference detailed history if needed

### Post-Merge Actions

**After Successful Merge**:

1. **Tag the Release**:
   ```bash
   git tag -a v2.0-net10.0 -m "Release: .NET 10.0 migration"
   git push origin v2.0-net10.0
   ```

2. **Update GitHub Release** (if using):
   - Create release from tag
   - Add release notes
   - Attach binaries if applicable

3. **Notify Stakeholders**:
   - Send email to team
   - Update project board
   - Close related issues

4. **Clean Up** (optional):
   ```bash
   # Delete local branch (optional)
   git branch -d upgrade-to-NET10
   
   # Delete remote branch (optional, after confirming merge successful)
   git push origin --delete upgrade-to-NET10
   ```

5. **Update Documentation**:
   - Update wiki/confluence
   - Update deployment guides
   - Update developer onboarding docs

### Rollback Procedure

**If Issues Found After Merge to Master**:

**Option 1: Revert the Merge** (Safest):
```bash
# Find the merge commit
git log --oneline --graph -10

# Revert the merge
git revert -m 1 <merge-commit-hash>

# Push
git push origin master

# Fix issues on upgrade-to-NET10 branch
git checkout upgrade-to-NET10
# ... make fixes ...
git commit -am "Fix issues found in production"

# Try merge again later
```

**Option 2: Hard Reset** (If caught immediately, no other commits):
```bash
# DANGEROUS - Only if no one else has pulled
git reset --hard HEAD~1
git push -f origin master

# Fix issues on upgrade branch
git checkout upgrade-to-NET10
# ... make fixes ...
```

**Option 3: Hotfix Branch** (If partial rollback needed):
```bash
# Create hotfix branch from master
git checkout master
git checkout -b hotfix-database-provider

# Fix specific issue
# ... make targeted fixes ...
git commit -am "Fix database provider issue"

# Merge hotfix
git checkout master
git merge hotfix-database-provider
git push origin master
```

### Best Practices

**Do**:
- ? Keep master stable (never commit directly during migration)
- ? Use descriptive commit messages
- ? Commit logical units of work
- ? Test before committing
- ? Push regularly to backup work
- ? Tag important milestones

**Don't**:
- ? Commit half-finished work (unless explicitly checkpointing)
- ? Commit without testing
- ? Force push to shared branches (unless agreed)
- ? Delete branches before confirming merge successful
- ? Merge without review (if team > 1)

### Commit Frequency Guidance

**All-At-Once Strategy Recommendation**:
- **Preferred**: Single commit after all validations pass
- **Fallback**: Checkpoint commits if issues encountered
- **Never**: Multiple small commits for every tiny change

**Checkpoint Commit Triggers**:
- Major milestone completed (project file, packages, code)
- Want to preserve working state before risky change
- End of day (backup work in progress)
- Before attempting something that might break everything

**Final Commit Trigger**:
- All tests pass
- All validations complete
- Documentation updated
- Ready for production

---

## Success Criteria

### Technical Criteria (Must Have)

#### Build Success
- [x] Solution loads in Visual Studio 2022 17.12+ or VS 2025 without errors
- [x] `dotnet restore` completes successfully with 0 errors
- [x] `dotnet build` succeeds with 0 compilation errors
- [x] Build warnings: 0 preferred, < 5 acceptable
- [x] No deprecated API warnings
- [x] Project file is valid SDK-style format
- [x] TargetFramework is `net10.0-windows`

#### Package Compatibility
- [x] All packages restore successfully
- [x] No package version conflicts
- [x] All packages compatible with net10.0-windows
- [x] No deprecated packages remain:
  - ? System.Data.SqlClient removed
  - ? System.Buffers removed
  - ? System.Memory removed
  - ? System.Numerics.Vectors removed
  - ? System.Runtime.CompilerServices.Unsafe removed
- [x] Required packages present:
  - ? EntityFramework 6.5.1
  - ? Microsoft.Data.SqlClient 5.2.2
  - ? MySql.Data 9.1.0+
  - ? System.Configuration.ConfigurationManager 9.0.0

#### Runtime Success
- [x] Application starts without crashing
- [x] Main form (`Acceuil`) displays correctly
- [x] No unhandled exceptions at startup
- [x] Application runs on .NET 10 runtime
- [x] No .NET Framework dependencies remain

#### Database Connectivity
- [x] Connection to SQL Server (PT62\SQL2022) successful
- [x] Entity Framework context initializes without errors
- [x] Database queries execute successfully
- [x] Connection string with `Microsoft.Data.SqlClient` provider works
- [x] CRUD operations function correctly (if applicable)

#### Forms and Designer
- [x] All forms open in Visual Studio designer:
  - [ ] `Acceuil.cs` opens in designer
  - [ ] `Dashbord.cs` opens in designer
- [x] No designer errors in Error List
- [x] All controls visible and positioned correctly
- [x] Forms display correctly at runtime
- [x] Form navigation works
- [x] Form events fire correctly

#### User Controls
- [x] `UCDashbord` loads in parent form
- [x] `Filtre Emploi de Temps` loads and functions
- [x] User control events work
- [x] User control data binding works

#### Localization
- [x] French resources load correctly
- [x] All `.resx` files accessible
- [x] No "missing resource" errors
- [x] All localized text displays in French
- [x] Embedded resources (images, icons) load

#### Configuration
- [x] App.config loads correctly
- [x] Connection strings accessible
- [x] Entity Framework configuration works
- [x] No configuration-related errors

### Quality Criteria (Should Have)

#### Performance
- [x] Application startup time ? .NET Framework 4.8 baseline + 10%
- [x] Database query performance maintained or improved
- [x] UI responsiveness equal or better
- [x] Memory usage ? baseline + 20%
- [x] No performance regressions

**Performance Baselines**:
| Metric | .NET Framework 4.8 | .NET 10 | Delta | Pass/Fail |
|--------|-------------------|---------|-------|-----------|
| Cold Start | ___ ms | ___ ms | ___ ms | [ ] |
| Warm Start | ___ ms | ___ ms | ___ ms | [ ] |
| DB Connection | ___ ms | ___ ms | ___ ms | [ ] |
| Form Load | ___ ms | ___ ms | ___ ms | [ ] |
| Memory Usage | ___ MB | ___ MB | ___ MB | [ ] |

#### Code Quality
- [x] Code follows existing patterns and conventions
- [x] No code duplication introduced
- [x] No commented-out code left in
- [x] No temporary debug code left in
- [x] All using statements correct and necessary

#### Test Coverage
- [x] All functional tests pass
- [x] All integration tests pass (if exist)
- [x] Regression testing complete
- [x] No known critical bugs
- [x] No known high-priority bugs

#### Documentation
- [x] README updated with .NET 10 requirements
- [x] Setup instructions updated
- [x] Deployment guide updated
- [x] Known issues documented (if any)
- [x] Migration notes added
- [x] Version updated in documentation

### Process Criteria (Must Follow)

#### All-At-Once Strategy Principles
- [x] All project file changes completed atomically
- [x] All package updates completed atomically
- [x] All code changes completed in single operation
- [x] Build validated before moving to testing
- [x] Solution builds with 0 errors after atomic upgrade
- [x] Single testing phase after all changes complete
- [x] Preferred: Single commit for entire upgrade (or justified checkpoint commits)

#### Migration Strategy Followed
- [x] Project file converted to SDK-style first
- [x] Packages updated before code changes
- [x] Configuration updated consistently
- [x] Code changes systematic (all using statements updated)
- [x] Build fixed before testing
- [x] Testing comprehensive across all areas

#### Source Control Followed
- [x] All work on `upgrade-to-NET10` branch
- [x] Master branch remains stable
- [x] Commit messages descriptive
- [x] Logical commit points (if checkpointing)
- [x] No force pushes to shared branches
- [x] Tag created for .NET Framework 4.8 baseline

#### Testing Strategy Followed
- [x] Smoke tests after each major step
- [x] Designer validation for all forms
- [x] Database connectivity tested
- [x] Functional testing complete
- [x] Localization verified
- [x] Performance compared to baseline
- [x] Regression testing performed
- [x] Issues documented and fixed

### Functional Requirements (Must Work)

#### Core Features
- [x] **Welcome/Login Form** (`Acceuil`):
  - [ ] Form displays correctly
  - [ ] Layout correct
  - [ ] All controls functional
  - [ ] Navigation to Dashboard works
  - [ ] French localization works

- [x] **Dashboard Form** (`Dashbord`):
  - [ ] Form displays correctly
  - [ ] Dashboard control (`UCDashbord`) loads
  - [ ] Data displays correctly
  - [ ] All navigation works
  - [ ] All buttons functional

- [x] **Schedule Filter** (`Filtre Emploi de Temps`):
  - [ ] Control loads correctly
  - [ ] Filter functionality works
  - [ ] Results display correctly
  - [ ] Performance acceptable

- [x] **Data Operations**:
  - [ ] Load `InfoEmploiDeTemps` data
  - [ ] Display data in UI
  - [ ] Filter data correctly
  - [ ] No data corruption
  - [ ] Performance acceptable

### Acceptance Criteria Summary

**Migration considered successful when**:

1. ? **All Technical Criteria met** (100% required)
   - Builds successfully on .NET 10
   - All packages compatible
   - Application runs correctly
   - Database connectivity works
   - Forms open in designer and runtime

2. ? **All Quality Criteria met** (95%+ required)
   - Performance acceptable
   - Code quality maintained
   - Tests pass
   - Documentation updated

3. ? **Process Criteria followed** (100% required)
   - All-At-Once strategy applied correctly
   - Migration steps followed systematically
   - Source control strategy followed
   - Testing strategy completed

4. ? **All Functional Requirements work** (100% required)
   - All core features functional
   - No regressions from .NET Framework version
   - User experience unchanged

### Validation Checklist

**Before declaring migration complete**:

#### Pre-Merge Validation
- [ ] All technical criteria checked and passed
- [ ] All quality criteria checked and passed
- [ ] All process criteria followed
- [ ] All functional requirements verified
- [ ] Performance baselines documented and acceptable
- [ ] All known issues documented
- [ ] Stakeholders notified and approved

#### Merge Validation
- [ ] Code review complete (if applicable)
- [ ] Pull request approved (if using PR workflow)
- [ ] All tests pass in CI/CD (if configured)
- [ ] No merge conflicts with master
- [ ] Merge to master completed
- [ ] Tag created (v2.0-net10.0)

#### Post-Merge Validation
- [ ] Application works from master branch
- [ ] Quick smoke test passed
- [ ] Deployment documentation verified
- [ ] Team notified of completion
- [ ] Issues closed in tracking system

### Sign-Off

**Required Sign-Offs**:
- [ ] **Developer**: Migration complete, all criteria met
- [ ] **Tech Lead**: Code reviewed, quality acceptable
- [ ] **QA/Tester**: All tests passed, no regressions
- [ ] **Product Owner**: Functionality verified, ready for production

**Sign-Off Template**:
```
Migration Sign-Off: .NET 10.0 Upgrade

Developer Sign-Off:
- All technical criteria met: Yes/No
- All tests passed: Yes/No
- Documentation updated: Yes/No
- Ready for production: Yes/No
Signed: ___________ Date: ___________

Tech Lead Sign-Off:
- Code review complete: Yes/No
- Quality acceptable: Yes/No
- No concerns: Yes/No
Signed: ___________ Date: ___________

QA Sign-Off:
- Functional testing complete: Yes/No
- Regression testing complete: Yes/No
- Performance acceptable: Yes/No
- No critical bugs: Yes/No
Signed: ___________ Date: ___________

Product Owner Sign-Off:
- Functionality verified: Yes/No
- User experience acceptable: Yes/No
- Ready for deployment: Yes/No
Signed: ___________ Date: ___________
```

### Definition of Done

**Migration is DONE when**:

1. ? Application builds and runs on .NET 10
2. ? All forms work correctly (designer and runtime)
3. ? Database connectivity verified
4. ? All core features functional
5. ? French localization works
6. ? Performance meets baseline
7. ? No regressions identified
8. ? Documentation updated
9. ? Code merged to master
10. ? All sign-offs obtained

**At this point**: Migration can be considered complete and application ready for deployment.

---

## Appendix: Quick Reference

### Key Package Changes
- EntityFramework: 6.2.0 ? 6.5.1
- MySql.Data: 8.0.28 ? 9.1.0
- System.Data.SqlClient: 4.8.3 ? **Remove**
- **Add**: Microsoft.Data.SqlClient 5.2.2
- **Remove**: System.Buffers, System.Memory, System.Numerics.Vectors, System.Runtime.CompilerServices.Unsafe

### Key Code Changes
```csharp
// BEFORE
using System.Data.SqlClient;
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

// AFTER
using Microsoft.Data.SqlClient;
ApplicationConfiguration.Initialize();
```

### Key Config Changes
```xml
<!-- App.config -->
<!-- REMOVE: <startup> section -->
<!-- UPDATE: Provider names System.Data.SqlClient ? Microsoft.Data.SqlClient -->
<!-- REMOVE: <runtime><assemblyBinding> section -->
```

### Quick Commands
```powershell
# Build
dotnet restore
dotnet build

# Run
dotnet run

# Check SDKs
dotnet --list-sdks

# List packages
dotnet list package
```

---

**Plan Version**: 1.0  
**Plan Date**: January 2025  
**Plan Author**: Migration Planning Agent  
**Target Completion**: 1-2 weeks from start
