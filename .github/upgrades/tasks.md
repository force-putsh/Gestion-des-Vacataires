# Migration Tasks: .NET Framework 4.8 ? .NET 10.0 LTS
**Project**: Gestion des Vacataires  
**Strategy**: All-At-Once (Atomic Upgrade)  
**Estimated Duration**: 1-2 weeks

---

## Progress Dashboard

**Overall Progress**: 5/8 tasks complete (62%) ![62%](https://progress-bar.xyz/62)

| Phase | Tasks | Complete | Status |
|-------|-------|----------|--------|
| Phase 0: Prerequisites | 1 | 0 | ? Not Started |
| Phase 1: Atomic Upgrade | 6 | 0 | ? Not Started |
| Phase 2: Testing & Validation | 1 | 0 | ? Not Started |
| **TOTAL** | **8** | **0** | **? Not Started** |

**Legend**: `[ ]` Not Started | `[?]` In Progress | `[?]` Complete | `[?]` Failed | `[?]` Skipped

---

## Phase 0: Prerequisites Verification

### [?] TASK-001: Verify development environment *(Completed: 2026-01-15 12:33)*
**Goal**: Ensure all tools and SDKs are installed and ready  
**Estimated Time**: 15-30 minutes  
**Risk**: Low

**Actions**:
- [?] (1) Verify .NET 10 SDK installed
  ```powershell
  dotnet --list-sdks
  # Expected: 10.x.x listed
  ```
  
- [?] (2) Verify Visual Studio 2022 17.12+ or VS 2025 installed
  - Open Visual Studio
  - Help ? About
  - Verify version ? 17.12
  
- [?] (3) Verify Git repository state
  ```powershell
  cd "C:\Users\vngounou\source\repos\Gestion-des-Vacataires"
  git status
  # Expected: On branch upgrade-to-NET10, working tree clean
  ```
  
- [?] (4) Create backup tag for current state
  ```powershell
  git checkout master
  git tag v1.0-netframework48
  git push origin v1.0-netframework48
  git checkout upgrade-to-NET10
  ```
  
- [?] (5) Take screenshots of all forms (backup for comparison)
  - Open solution in Visual Studio
  - Open Acceuil.cs in designer ? Screenshot
  - Open Dashbord.cs in designer ? Screenshot
  - Save screenshots to documentation folder

**Validation**:
- ? `dotnet --list-sdks` shows .NET 10.x.x
- ? Visual Studio version ? 17.12
- ? Git tag `v1.0-netframework48` created
- ? On branch `upgrade-to-NET10`
- ? Screenshots saved

**Rollback**: N/A (no changes made yet)

---

## Phase 1: Atomic Upgrade

### [?] TASK-002: Convert project to SDK-style format
**Goal**: Transform legacy .csproj to modern SDK-style  
**Estimated Time**: 1-2 hours  
**Risk**: Medium-High  
**Dependencies**: TASK-001

**Actions**:
- [?] (1) Backup current project file
  ```powershell
  cp "Gestion des Vacataires.csproj" "Gestion des Vacataires.csproj.backup"
  cp "packages.config" "packages.config.backup"
  ```

- [ ] (2) Close solution in Visual Studio

- [ ] (3) Replace `Gestion des Vacataires.csproj` with SDK-style format:
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

    <ItemGroup>
      <FrameworkReference Include="Microsoft.WindowsDesktop.App.WPF" />
    </ItemGroup>
  </Project>
  ```

- [ ] (4) Delete `packages.config` file
  ```powershell
  rm "packages.config"
  ```

- [ ] (5) Open solution in Visual Studio 2022
  - Should load without errors
  - Verify solution explorer shows project

- [ ] (6) Restore packages
  ```powershell
  dotnet restore "Gestion des Vacataires.csproj"
  ```

**Validation**:
- ? Solution loads in Visual Studio 2022
- ? Project file is SDK-style format
- ? `dotnet restore` completes without errors
- ? `packages.config` deleted
- ? TargetFramework is `net10.0-windows`

**Rollback**: 
```powershell
cp "Gestion des Vacataires.csproj.backup" "Gestion des Vacataires.csproj"
cp "packages.config.backup" "packages.config"
```

---

### [?] TASK-003: Update App.config for .NET 10 *(Completed: 2026-01-15 14:05)*
**Goal**: Remove deprecated sections and update providers  
**Estimated Time**: 30 minutes  
**Risk**: Low  
**Dependencies**: TASK-002

**Actions**:
- [?] (1) Open `App.config` in editor

- [?] (2) Remove `<startup>` section entirely
  - Delete lines containing:
    ```xml
    <startup>
      <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    ```

- [?] (3) Update EntityFramework provider invariant name
  - Find: `invariantName="System.Data.SqlClient"`
  - Replace with: `invariantName="Microsoft.Data.SqlClient"`

- [?] (4) Update connection string provider name
  - Find: `providerName="System.Data.SqlClient"`
  - Replace with: `providerName="Microsoft.Data.SqlClient"`

- [?] (5) Remove `<runtime><assemblyBinding>` section entirely
  - Delete entire `<runtime>` element and its contents

- [?] (6) Save `App.config`

**Expected App.config structure after changes**:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  
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
  
  <connectionStrings>
    <add name="DbGestionnaireStagiaireEntities" 
         connectionString="Data Source=PT62\SQL2022;Initial Catalog=Gestion_Etudiants;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;" 
         providerName="Microsoft.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

**Validation**:
- ? `<startup>` section removed
- ? All provider names changed to `Microsoft.Data.SqlClient`
- ? `<runtime>` section removed
- ? XML is well-formed (no syntax errors)
- ? Connection strings still present with updated provider

**Rollback**: Use Git to revert App.config changes

---

### [?] TASK-004: Update Program.cs entry point *(Completed: 2026-01-15 14:07)*
**Goal**: Modernize application initialization for .NET 10  
**Estimated Time**: 15 minutes  
**Risk**: Low  
**Dependencies**: TASK-002

**Actions**:
- [?] (1) Open `Program.cs` in editor

- [?] (2) Locate the `Main` method

- [?] (3) Replace old initialization code
  - **REMOVE**:
    ```csharp
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    ```
  
  - **ADD**:
    ```csharp
    ApplicationConfiguration.Initialize();
    ```

- [?] (4) Save `Program.cs`

**Expected Program.cs after changes**:
```csharp
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

**Validation**:
- ? `ApplicationConfiguration.Initialize()` present
- ? Old methods (`EnableVisualStyles`, `SetCompatibleTextRenderingDefault`) removed
- ? `Application.Run(new Acceuil())` unchanged
- ? Code compiles without errors

**Rollback**: Use Git to revert Program.cs changes

---

### [?] TASK-005: Update all using statements for database provider *(Completed: 2026-01-15 14:08)*
**Goal**: Replace System.Data.SqlClient with Microsoft.Data.SqlClient  
**Estimated Time**: 30 minutes  
**Risk**: Low  
**Dependencies**: TASK-002

**Actions**:
- [?] (1) Search entire solution for `using System.Data.SqlClient;`
  - In Visual Studio: Ctrl+Shift+F
  - Search for: `using System.Data.SqlClient`

- [?] (2) For each file found, replace:
  - Find: `using System.Data.SqlClient;`
  - Replace: `using Microsoft.Data.SqlClient;`

- [?] (3) Files likely requiring updates (check each):
  - Program.cs (if contains database code)
  - Any data access files
  - Any form files with database code
  - InfoEmploiDeTemps.cs (check if it has SqlClient usage)

- [?] (4) Search for hardcoded string references
  - Search for: `"System.Data.SqlClient"`
  - Replace with: `"Microsoft.Data.SqlClient"`

- [?] (5) Save all modified files

**Validation**:
- ? No instances of `using System.Data.SqlClient` remain
- ? All replaced with `using Microsoft.Data.SqlClient`
- ? No compilation errors from missing namespace
- ? Search for "System.Data.SqlClient" returns 0 results (except in App.config comments)

**Rollback**: Use Git to revert all .cs file changes

---

### [?] TASK-006: Build solution and fix compilation errors *(Completed: 2026-01-15 14:11)*
**Goal**: Get solution building with 0 errors  
**Estimated Time**: 2-4 hours (variable based on errors)  
**Risk**: Medium-High  
**Dependencies**: TASK-002, TASK-003, TASK-004, TASK-005

**Actions**:
- [?] (1) Clean solution
  ```powershell
  dotnet clean
  ```

- [?] (2) Restore packages
  ```powershell
  dotnet restore
  ```

- [?] (3) Build solution
  ```powershell
  dotnet build
  ```

- [?] (4) Review build output for errors

- [?] (5) Fix compilation errors systematically:
  
  **Common Error Types**:
  
  a) **Missing namespace/type errors**:
  - Error: "The type or namespace name 'X' could not be found"
  - Solution: Add missing using statement or PackageReference
  
  b) **BouncyCastle namespace changes** (if used):
  - Package name changed to `BouncyCastle.Cryptography` in v2.x
  - Some types may have moved namespaces
  - Review BouncyCastle usage and update accordingly
  
  c) **MySql.Data API changes** (if used):
  - Some APIs may have changed in v9.x
  - Check MySql.Data migration documentation
  - Update usage accordingly
  
  d) **Designer-generated code issues**:
  - If designer files have errors, try regenerating:
    - Open form in designer
    - Make minor change
    - Save
    - Undo change
  
  e) **Entity Framework configuration issues**:
  - Ensure EF6 provider registration correct in App.config
  - Check DbContext initialization code

- [?] (6) After fixing each error, rebuild incrementally

- [?] (7) Continue until build succeeds with 0 errors

**Validation**:
- ? `dotnet build` completes successfully
- ? 0 compilation errors
- ? Warnings < 5 (0 preferred)
- ? No deprecated API warnings
- ? Solution explorer shows no errors

**Rollback**: If > 50 errors and stuck:
```powershell
git reset --hard HEAD
# Review plan, seek assistance, or try incremental approach
```

---

### [?] TASK-007: Verify designer functionality *(Completed: 2026-01-15 14:15)*
**Goal**: Ensure all forms open in Visual Studio designer  
**Estimated Time**: 30 minutes  
**Risk**: Medium  
**Dependencies**: TASK-006 (must build first)

**Actions**:
- [?] (1) In Visual Studio, open `Acceuil.cs` in designer
  - Right-click `Acceuil.cs` ? View Designer
  - Or double-click `Acceuil.cs`

- [?] (2) Verify Acceuil form in designer:
  - All controls visible
  - Layout matches screenshot
  - No errors in Error List
  - Can select controls
  - Properties window shows control properties

- [?] (3) In Visual Studio, open `Dashbord.cs` in designer
  - Right-click `Dashbord.cs` ? View Designer

- [?] (4) Verify Dashbord form in designer:
  - All controls visible
  - UCDashbord control visible
  - Layout matches screenshot
  - No errors in Error List

- [?] (5) Check Error List for any designer warnings
  - Review and address if critical

- [?] (6) Test minor edit in designer:
  - Change a label text
  - Save
  - Verify change persists
  - Undo change

**Validation**:
- ? `Acceuil.cs` opens in designer without errors
- ? `Dashbord.cs` opens in designer without errors
- ? All controls visible in both forms
- ? No critical errors in Error List
- ? Can edit and save forms in designer

**If Designer Fails**:
- Check detailed error in Error List
- Try regenerating designer file (see TASK-006)
- Compare with screenshots taken in TASK-001
- If necessary, manually fix InitializeComponent() method

---

## Phase 2: Testing & Validation

### [?] TASK-008: Comprehensive testing and validation
**Goal**: Verify all functionality works correctly on .NET 10  
**Estimated Time**: 1-2 days  
**Risk**: Medium  
**Dependencies**: TASK-007 (designer must work)

**Actions**:

#### Part A: Runtime Startup Test (15 min)
- [ ] (1) Run application from Visual Studio (F5 or Ctrl+F5)
- [ ] (2) Verify application starts without crashing
- [ ] (3) Verify Acceuil form displays correctly
- [ ] (4) Check Output window for errors or warnings
- [ ] (5) Close application

#### Part B: Database Connectivity Test (30 min)
- [ ] (1) Run application
- [ ] (2) Trigger any database operation (form that loads data)
- [ ] (3) Verify no "provider not found" errors
- [ ] (4) Verify no connection errors
- [ ] (5) Check if data loads successfully
- [ ] (6) Test navigation that involves database access

#### Part C: Form Functionality Test (1-2 hours)
- [ ] (1) Test Acceuil form:
  - All controls responsive
  - Events fire correctly
  - Validation works (if applicable)
  - Navigation to Dashbord works
  
- [ ] (2) Test Dashbord form:
  - Form displays correctly
  - UCDashbord control loads and displays
  - Data displays in dashboard
  - All navigation works
  - All buttons functional

- [ ] (3) Test Filtre Emploi de Temps control:
  - Control loads correctly
  - Filter functionality works
  - Results display correctly
  - Filter applies to data correctly

- [ ] (4) Test InfoEmploiDeTemps data operations:
  - Data loads from database
  - Displays correctly in UI
  - No data corruption
  - Performance acceptable

#### Part D: Localization Test (30 min)
- [ ] (1) Verify French resources load:
  - All labels display in French
  - No "missing resource" errors in Output window
  - Acceuil.fr.resx resources accessible
  
- [ ] (2) Check embedded resources:
  - Images display correctly
  - Icons load correctly
  - No resource loading errors

#### Part E: Performance Baseline Test (1 hour)
- [ ] (1) Measure application startup time:
  - Cold start (first launch after reboot)
  - Warm start (subsequent launches)
  - Record times
  
- [ ] (2) Test database operation performance:
  - Time to connect to database
  - Time to load InfoEmploiDeTemps data
  - Compare to .NET Framework 4.8 baseline (if available)
  
- [ ] (3) Monitor memory usage:
  - Memory at startup
  - Memory after data loaded
  - Memory after several operations
  
- [ ] (4) Test UI responsiveness:
  - Form load times
  - Control rendering speed
  - Navigation speed

**Performance Acceptance Criteria**:
- Startup time: ? .NET Framework baseline + 10% (or < 3 seconds if no baseline)
- Database operations: ? baseline (or reasonable for SQL Server)
- Memory: ? baseline + 20% (or < 200MB for small app)
- UI: No noticeable lag

#### Part F: Regression Testing (2-3 hours)
- [ ] (1) Execute all major user workflows:
  - Login/Welcome flow
  - Dashboard viewing
  - Schedule filtering
  - Data operations
  - Navigation paths
  
- [ ] (2) Compare behavior to .NET Framework version:
  - All features work identically
  - No missing functionality
  - No new bugs introduced
  
- [ ] (3) Document any deviations found

#### Part G: Issue Resolution (variable time)
- [ ] (1) Document all issues found during testing
- [ ] (2) Prioritize issues (Critical ? High ? Medium ? Low)
- [ ] (3) Fix critical and high priority issues
- [ ] (4) Retest after fixes
- [ ] (5) Document known issues (medium/low) for later resolution

**Validation**:
- ? Application starts successfully
- ? Database connectivity works
- ? All forms functional
- ? French localization works
- ? Performance acceptable
- ? 0 critical bugs
- ? 0 high priority bugs
- ? All regression tests pass

**If Critical Issues Found**:
- STOP execution
- Document issue thoroughly
- Review with team/stakeholders
- Determine if rollback needed or if fixable
- Do NOT proceed until critical issues resolved

---

## Final Steps

### [ ] TASK-009: Documentation and commit
**Goal**: Update documentation and commit all changes  
**Estimated Time**: 1-2 hours  
**Risk**: Low  
**Dependencies**: TASK-008 (all tests must pass)

**Actions**:
- [ ] (1) Update README.md:
  - Change .NET Framework 4.8 to .NET 10.0
  - Update requirements section
  - Add .NET 10 SDK requirement
  - Update Visual Studio version requirement

- [ ] (2) Update any setup/deployment documentation:
  - New runtime requirements
  - Installation instructions
  - Build instructions

- [ ] (3) Document known issues (if any)

- [ ] (4) Verify all changes staged for commit:
  ```powershell
  git status
  ```

- [ ] (5) Review all changed files before committing

- [ ] (6) Commit with comprehensive message:
  ```powershell
  git add .
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
  - Update Program.cs entry point (ApplicationConfiguration.Initialize)
  - Update all using statements
  - Update App.config for .NET 10

  Testing:
  - Build: Success (0 errors, 0 warnings)
  - Database: Connection and EF6 verified
  - Forms: All load correctly in designer and runtime
  - Localization: French resources work
  - Performance: Meets baseline
  - Regression: All tests pass

  No regressions identified.
  "
  ```

- [ ] (7) Push to remote:
  ```powershell
  git push origin upgrade-to-NET10
  ```

**Validation**:
- ? README.md updated
- ? Documentation updated
- ? Commit created with detailed message
- ? Changes pushed to remote
- ? Branch `upgrade-to-NET10` up to date

---

## Execution Log

This section will be populated during execution with timestamped progress updates.

### Session: [Date - To be filled during execution]

**Started**: [Timestamp]  
**Status**: Not Started

#### Progress Updates
- [ ] Task execution will be logged here with timestamps
- [ ] Issues encountered will be documented
- [ ] Resolutions will be recorded

---

## Notes for Execution

**Important Reminders**:

1. **Execute sequentially** - Complete each task before moving to next
2. **Validate after each task** - Use validation criteria to confirm success
3. **Stop on critical failures** - Don't proceed if validation fails
4. **Document issues** - Record any problems encountered
5. **Commit strategically** - Single commit after all tests pass (preferred)
6. **Test thoroughly** - Task 008 is critical, don't rush
7. **Keep backups** - Git allows rollback if needed

**Emergency Rollback**:
```powershell
git reset --hard HEAD~1  # Undo last commit
git checkout HEAD -- <file>  # Undo changes to specific file
```

**Get Help**:
- Review plan.md for detailed guidance
- Consult .NET 10 migration documentation
- Check Windows Forms migration guides
- Community forums for specific issues

---

**Tasks File Version**: 1.0  
**Generated**: January 2025  
**Based on**: plan.md v1.0  
**Strategy**: All-At-Once (Atomic Upgrade)
