# Progress so far

## Milestone 1 (Convex Hull):

---

- [X] DivideAndConquer
- [X] ExtremePoints
- [X] ExtremeSegment
- [X] GrahamScan
- [X] Incremental
- [X] JarvisMarch
- [X] QuickHull

---

# Quick summary

- This repo has 2 branches: main and sdk-8, one obviously uses sdk 8 while the other uses og template sdk, write your code in whichever branch you like, push and pull actions are auto synced

- To test a finished algorithm write in a terminal: `dotnet test CGAlgorithmsUnitTest/CGAlgorithmsUnitTest.csproj --filter "FullyQualifiedName~AlgorithmNameTest", or simply run from VS.

---

# CGUI Repository & Collaboration Guide

This document outlines the changes made to modernize this repository from **.NET Framework 4.5** to **.NET 8 SDK-style**, as well as instructions for building, running unit tests, and debugging C# code for developers on Linux and Windows.

---

> [!IMPORTANT]
> The original template was in SDK 4.5 and this modified version is SDK 8.0, working on both versions will cause errors and won't build. Meaning you will have to work with either one or the other. However SDK 8.0 is perfectly compatible with most systems, ensure it is installed on the device.

## Quickstart & CLI Workflow

### 1. Restore & Build
To restore packages and build the solution CLI-wide:
```bash
dotnet restore
dotnet build
```

### 2. Running All Unit Tests
Run all unit tests across the entire workspace:
```bash
dotnet test
```

### 3. Testing Specific Algorithms or Test Files
Use the `--filter` flag to run unit tests selectively:

* **Graham Scan Tests:**
  ```bash
  dotnet test CGAlgorithmsUnitTest/CGAlgorithmsUnitTest.csproj --filter "FullyQualifiedName~GrahamScanTest"
  ```
* **Divide & Conquer Tests:**
  ```bash
  dotnet test CGAlgorithmsUnitTest/CGAlgorithmsUnitTest.csproj --filter "FullyQualifiedName~DivideAndConquerTest"
  ```
* **Extreme Points Tests:**
  ```bash
  dotnet test CGAlgorithmsUnitTest/CGAlgorithmsUnitTest.csproj --filter "FullyQualifiedName~ExtremePointsTest"
  ```
* **Extreme Segments Tests:**
  ```bash
  dotnet test CGAlgorithmsUnitTest/CGAlgorithmsUnitTest.csproj --filter "FullyQualifiedName~ExtremeSegmentsTest"
  ```
* **Single Test Case:**
  ```bash
  dotnet test CGAlgorithmsUnitTest/CGAlgorithmsUnitTest.csproj --filter "FullyQualifiedName~GrahamScanTestCase1"
  ```

---

## Summary of Changes from the Original Repository

1. **Project File Modernization (.NET Framework 8)**
   - All legacy `.csproj` files (`CGUtilities`, `CGAlgorithms`, `CGUtilitiesUnitTest`, `CGAlgorithmsUnitTest`) were updated to modern .NET SDK format targeting `net8.0`.
   - Removed verbose manual `<Compile Include="...">` references. Modern SDK projects include all `.cs` files in their folder automatically.

2. **Cross-Platform Compatibility Note**
   - The UI project (`CGUI`) relies on **WPF (Windows Presentation Foundation)**, which is proprietary to Windows. While project files are updated to .NET 8, UI execution is native to Windows only. Algorithmic core logic (`CGAlgorithms`) and unit tests (`CGAlgorithmsUnitTest`) are fully cross-platform.

---

## Understanding C# Error Messages & Test Failures

When developing or executing tests, errors typically fall into two categories:

### 1. Build / Compilation Errors (CS-series codes)
- **What it means:** Syntax errors, missing dependencies, or illegal type references prevent the project from compiling.
- **Example:** `CS0246: The type or namespace name 'Point' could not be found`.
- **Solution:** Verify `using` statements at the top of the file, ensure modern .NET references match, and run `dotnet restore`.

### 2. Logical Assertion Failures (`Assert.IsTrue failed`)
- **What it means:** The project built and executed successfully, but your algorithm returned an incorrect mathematical result for the test case input.
- **Example Stack Trace:**
  ```text
  Assert.IsTrue failed. Fails in Convex Hull - Graham Scan: Case 10 (1000 Point)
  at CGAlgorithmsUnitTest.ConvexHullTest.Case1000Points() in .../ConvexHullTest.cs:line 3514
  at CGAlgorithmsUnitTest.GrahamScanTest.GrahamScanNormalTestCase1000Points() in .../GrahamScanTest.cs:line 63
  ```
- **How to Debug:**
  1. Open the indicated line in `ConvexHullTest.cs` to view the input set of points and expected convex hull output.
  2. Use conditional breakpoints or `Console.WriteLine` statements within your algorithm class in `CGAlgorithms/` to track intermediate geometric state during execution.
