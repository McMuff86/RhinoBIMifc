## RhinoBimIfc – Usage Guide

This project combines a Rhino 8 plugin (C#/.NET), Grasshopper automation, a lightweight web viewer, and IFC/TypeSpec schemas.

### Prerequisites
- Windows 10/11
- Rhino 8 (for plugin/Grasshopper workflows)
- .NET 8 SDK (Visual Studio 2022 recommended)
- Node.js 20+ and npm
- Git LFS (for sample IFCs)

### Repository Structure (high level)
- `src/` – C# solution and Node viewer sources
- `schema/` – TypeSpec sources and OpenAPI outputs
- `schemas/` – JSON Schemas (DoorParam, BOM line)
- `gh/` – Grasshopper reference definition
- `examples/` – Sample IFC(X) models
- `web/` – Static site and viewer bundle output

## Getting Started

### 1) Clone and prepare
```bash
git clone <this-repo>
cd RhinoBimIfc
git lfs pull
```

### 2) Node: viewer and tests
```bash
cd src
npm install
npm run test
npm run build-viewer
npm run serve
# open the printed localhost URL to view `web/viewer/index.html` with `render.mjs`
```

#### Convert IFC (.ifc) to IFCX (.ifcx) for the web viewer
- Install uv (see Agents.md) and Python 3.11+
- Create env and install deps:
```bash
uv venv
uv pip install ifcopenshell
```
- Build CLI once:
```bash
cd src
npm run build-cli
```
- Convert:
```bash
npm run convert:ifc -- "../examples/Hello Wall/IFC_Test.ifc" "../examples/Hello Wall/IFC_Test.ifcx"
```
- Load the produced `.ifcx` in the web viewer (file form or URL).

### 3) Schema: compile and generate types
```bash
cd schema
npm install
npm run compile
npm run gen
# outputs OpenAPI under schema/out and TS types under schema/out/ts/ifcx.d.ts
```

### 4) .NET: build and tests
```powershell
cd ..
dotnet build RhinoBimIfc.sln -c Release
dotnet test .\tests\RhinoBimIfc.Core.Tests\RhinoBimIfc.Core.Tests.csproj -c Release
dotnet test .\tests\RhinoBimIfc.IFC.Adapter.Tests\RhinoBimIfc.IFC.Adapter.Tests.csproj -c Release
dotnet test .\tests\RhinoBimIfc.Integration.Tests\RhinoBimIfc.Integration.Tests.csproj -c Release
```

### 5) Rhino plugin (debug flow)
- Open `RhinoBimIfc.sln` in Visual Studio 2022.
- Set `src/RhinoBimIfcPlugin/RhinoBimIfcPlugin.csproj` as Startup.
- Build and start with debugging; load the plugin in Rhino if prompted.
- Commands (planned): `IfcOpen`, `IfcListDoors`, `IfcDoorsToGH`, `IfcExportBOM`.
- Note: Plugin/commands compile only when `RHINO` is defined. Use: `dotnet build RhinoBimIfc.sln -c Debug -p:RhinoBuild=true` and ensure RhinoCommon is referenced in VS.

## Data Contracts
- Door parameters DTO: `src/RhinoBimIfc.Core/DTOs/DoorParam.cs`
- DoorParam JSON schema: `schemas/doorparam.schema.json`
- BOM line JSON schema: `schemas/bomline.schema.json`

## Samples and Testing
- Sample IFCs under `examples/Hello Wall/` and others.
- Integration tests will load a sample IFC and validate door counts and dimensions.

## Troubleshooting
- esbuild missing: run `npm install` inside `src/`.
- TypeSpec tools missing: run `npm install` inside `schema/`.
- Rhino plugin not loading: ensure Rhino 8 and .NET 8 are installed, and run VS as admin if needed for debugging.

## Next Steps
- Implement primary IFC adapter (IFC development repository) and fallback XBim adapter.
- Add Rhino commands and Eto.Forms panel per README guidance.


