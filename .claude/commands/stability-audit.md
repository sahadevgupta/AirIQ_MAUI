# .NET MAUI Project Stability Audit

You are a .NET MAUI stability expert. Perform a comprehensive audit of the current project based on the Crashless playbook. Your job is to find every crash risk, ANR source, memory leak, async misuse, and observability gap — then produce a prioritized report and optionally fix everything.

## Step 1: Ask operating mode

Before starting, ask the user which mode they want:

1. **Audit only** — full report, no code changes
2. **Audit + fix all** — full report, then apply all safe fixes automatically
3. **Audit + fix P0/P1 only** — full report, then apply only critical/high fixes

Default to **Audit only** if the user doesn't choose.

## Step 2: Scan the entire codebase

Thoroughly search ALL `.cs`, `.xaml`, `.csproj`, `.xml`, `.plist`, and `.json` files in the project. Use glob and grep extensively. Do not sample — scan everything.

## Step 3: Check each audit category

For every category below, search the codebase for violations and record each finding with file path, line number, code snippet, severity, and recommended fix.

### Category 1: Unhandled Exceptions (P0)

Search for:
- `[RelayCommand]` or `[ICommand]` methods missing try/catch
- `async Task` methods in ViewModels without exception handling
- HTTP calls (`HttpClient`, `GetAsync`, `PostAsync`, `SendAsync`, `GetStringAsync`, `ReadFromJsonAsync`) without try/catch
- Missing `EnsureSuccessStatusCode()` after HTTP responses
- `data!` or force-unwrap (`!`) on nullable API responses
- `ObservableCollection` or bindable properties declared without `= new()` initializer
- Properties bound in XAML that could be null at binding time

### Category 2: Main Thread Blocking / ANR Risks (P0)

Search for:
- `.Result` on any Task (especially in UI code, event handlers, constructors, OnAppearing)
- `.Wait()` on any Task
- `.GetAwaiter().GetResult()` in UI paths
- `Thread.Sleep()` outside of `#if DEBUG` blocks
- Synchronous file I/O, database calls, or network calls in page lifecycle methods
- Heavy computation in `OnAppearing`, `OnNavigatedTo`, constructors, or event handlers without `Task.Run`

### Category 3: Async/Await Correctness (P0-P1)

Search for:
- `async void` methods (flag all except framework-required event handlers like `OnAppearing`, `OnClicked` etc.)
- Fire-and-forget: async method calls without `await` and without `.SafeFireAndForget()` or equivalent error handler
- `_ = SomeAsyncMethod()` without exception routing
- Missing `CancellationToken` parameters in async methods bound to page lifecycle
- `TaskScheduler.UnobservedTaskException` not wired in startup

### Category 4: Platform-Specific Crash Sources (P1)

Search for:
- UI property assignments (`Text =`, `ItemsSource =`, `IsVisible =`, property sets on controls) inside `Task.Run`, background threads, or async continuations without `MainThread.BeginInvokeOnMainThread`
- Permission usage (`Camera`, `Location`, `Photos`, `Microphone`, `Contacts`, etc.) without `Permissions.CheckStatusAsync` / `Permissions.RequestAsync`
- Android: Check `Platforms/Android/AndroidManifest.xml` for missing `<uses-permission>` matching code usage
- iOS: Check `Platforms/iOS/Info.plist` for missing `NS*UsageDescription` keys matching code usage
- Platform-specific code in `#if ANDROID` / `#if IOS` blocks for lifecycle issues

### Category 5: Memory Leaks / Lifecycle Issues (P1)

Search for:
- Event subscriptions (`+=`) without matching unsubscriptions (`-=`)
- `OnAppearing` that subscribes to events without `OnDisappearing` unsubscribe
- `MessagingCenter.Subscribe` or `WeakReferenceMessenger.Default.Register` without corresponding `Unregister`
- `IDisposable` implementations missing `Dispose()` calls or not using `using`/`await using`
- Pages/ViewModels holding references to services that hold references back (circular references)
- Image sources or streams not disposed
- Handlers not disconnected for custom views with native resources (`DisconnectHandler`)

### Category 6: Navigation & Binding Race Conditions (P1)

Search for:
- Async data loading in `OnAppearing` without `CancellationTokenSource` that's cancelled in `OnDisappearing`
- ViewModel property assignments after async operations without checking if page/token is still valid
- Commands that can execute concurrently (missing `IsBusy` guard or `AsyncRelayCommand` concurrency check)
- `Shell.Current.GoToAsync` or `Navigation.PushAsync` in async methods without try/catch

### Category 7: Observability Gaps (P2)

Check for presence/absence of:
- `AppDomain.CurrentDomain.UnhandledException` handler in startup
- `TaskScheduler.UnobservedTaskException` handler in startup
- `AndroidEnvironment.UnhandledExceptionRaiser` in Android startup (if Android target exists)
- `ObjCRuntime.Runtime.MarshalManagedException` in iOS startup (if iOS target exists)
- Any crash reporting SDK integration (Sentry, Firebase Crashlytics, App Center, Bugsnag)
- `ICrashReporter` or equivalent abstraction
- Breadcrumb logging on navigation, API calls, lifecycle events
- Runtime tags (device model, OS version, app version, connectivity) attached to crash reports
- Source Link enabled in `.csproj` (`<EnableSourceLink>true</EnableSourceLink>`)
- Symbol/PDB strategy for release builds

### Category 8: Trimming & AOT Risks (P2)

Search for:
- `JsonSerializer.Serialize` / `JsonSerializer.Deserialize` without source-generated `JsonSerializerContext`
- Reflection usage (`Type.GetType`, `Activator.CreateInstance`, `GetMethod`, `GetProperty`, `Assembly.GetTypes`) without `[DynamicallyAccessedMembers]` or `[DynamicDependency]`
- `.csproj` files: check `PublishTrimmed`, `TrimMode`, `EnableLLVM`, `PublishAot` settings
- Missing trimmer warning CI gates (no `/p:SuppressTrimAnalysisWarnings=false` or `/p:TrimmerSingleWarn=false` in build scripts)

### Category 9: HTTP Resilience (P2)

Search for:
- Direct `new HttpClient()` instead of `IHttpClientFactory`
- Missing `HttpClient.Timeout` configuration
- No retry/resilience policy (Polly or `Microsoft.Extensions.Http.Resilience`)
- POST/PUT/DELETE requests with retry policies (non-idempotent retry risk)

### Category 10: Testing Gaps (P3)

Check for:
- Test projects existence and coverage of ViewModels
- Tests for command failure/cancellation scenarios
- Release-mode build validation in CI/CD (`.github/workflows`, `azure-pipelines.yml`, etc.)
- Any soak/stress test scripts or configuration

## Step 4: Produce the report

Format the output exactly as follows:

---

### Executive Summary

- **Stability posture**: [Critical / Poor / Fair / Good / Excellent]
- **Total findings**: [count] ([P0 count] critical, [P1 count] high, [P2 count] medium, [P3 count] low)
- **Top 3 risks**:
  1. [risk]
  2. [risk]
  3. [risk]
- **Recommended first actions** (this week):
  1. [action]
  2. [action]
  3. [action]

### Findings

For each finding, use this format:

> **[P0/P1/P2/P3] [Short title]**
> - **File**: `path/to/file.cs:LINE`
> - **Code**: `problematic code snippet`
> - **Risk**: [What can happen — crash, ANR, leak, data loss, etc.]
> - **Fix**: [Concrete code change or pattern to apply]
> - **Auto-fixable**: Yes / No / Needs review

Group findings by category, sorted by priority within each group.

### Proposed Change Set

List all changes grouped by priority:
- P0 changes (apply immediately)
- P1 changes (apply this sprint)
- P2 changes (schedule soon)
- P3 changes (backlog)

For each change: file, what changes, why it's safe.

### Missing Infrastructure

List any recommended infrastructure that doesn't exist yet:
- Global exception handlers
- Crash reporting setup
- BaseViewModel pattern
- CI quality gates
- Source generation for JSON
- etc.

---

## Step 5: Apply fixes (if user chose mode 2 or 3)

When applying fixes:
- Fix one file at a time, showing what you're changing and why
- Keep edits minimal — fix the root cause only, don't refactor surrounding code
- Preserve existing code style (indentation, naming, patterns)
- Don't add new NuGet packages without asking first
- Don't restructure project architecture
- For each fix applied, mark it in the report
- After all fixes, summarize what was changed and what still needs manual review
- If a fix is ambiguous or risky, ask before applying

## Important rules

- Be thorough — scan every file, not just a sample
- Be precise — include exact file paths and line numbers
- Be practical — prioritize by real user impact, not theoretical purity
- Distinguish between confirmed issues and potential risks
- Don't flag framework-required patterns as issues (e.g., `async void` for event handlers is acceptable if delegated properly)
- Consider the project's target frameworks and MAUI version when evaluating findings
