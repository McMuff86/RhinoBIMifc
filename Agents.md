## RhinoBimIfc – AI Agents

This document defines the AI agents collaborating around this repository. It captures responsibilities, inputs/outputs (contracts), and deep links to the most relevant code and schemas. Keep it current when capabilities change.

### Principles
- Clear single ownership for each concern (IFC parsing, GH runs, BOM, UI, CI/CD).
- Contracts first: agents exchange structured JSON only.
- Deterministic runs with versioned inputs (IFC files, GH definition, rules).

### Key Artifacts (linked)
- IFC provider interface: `src/RhinoBimIfc.Core/Abstractions/IIfcProvider.cs`
- Door parameters DTO: `src/RhinoBimIfc.Core/DTOs/DoorParam.cs`
- DoorParam JSON Schema: `schemas/doorparam.schema.json`
- BOM line JSON Schema: `schemas/bomline.schema.json`
- TypeSpec schema: `schema/ifcx.tsp` (compile via `npm run compile` in `schema/`)
- Viewer entry page: `web/index.html` (bundled by `src` → `web/viewer/render.mjs`)
- Node build/test scripts: `src/package.json`

## Agents

### Orchestrator Agent (Lead)
- Purpose: Plan runs, trigger specialized agents, aggregate outputs, report status.
- Inputs:
  - Project request JSON (see Contracts → Orchestrator Input).
- Outputs:
  - Run plan, progress updates, consolidated results (BOM, logs, artifacts).
- Triggers: IFC Agent → Grasshopper Agent → BOM/Data Agent → DevOps Agent.

### IFC Agent (Parsing & Indexing)
- Purpose: Load IFC, normalize units, extract elements and properties with focus on doors.
- Code Links: `src/RhinoBimIfc.Core/Abstractions/IIfcProvider.cs`
- Inputs:
  - IFC file path, optional filters.
- Outputs:
  - Door list mapped to `DoorParam` fields where possible; raw properties per GUID; geometry previews (optional Brep for Rhino viewer layer).
- Notes: Prefer primary adapter (IFC development repository) with fallback XBim.

### Grasshopper Agent (Parametric Geometry)
- Purpose: Bind `DoorParam` to GH definition, run headless, collect geometry.
- Artifacts: `gh/DoorGenerator.ghx`
- Inputs:
  - `DoorParam` per door; GH definition path; run settings.
- Outputs:
  - Geometry (frame/leaf/…); baked object identifiers and attributes (UserText) when requested.

### BOM/Data Agent (Aggregation & Export)
- Purpose: Build validated BOM from doors and rule sets; export CSV/JSON.
- Schemas: `schemas/bomline.schema.json`, `schemas/doorparam.schema.json`
- Inputs:
  - Generated geometry/parameters; rule configuration; project metadata.
- Outputs:
  - `out/bom_{project}_{timestamp}.json` and `.csv` (schema-validated).

### Rhino Plugin Agent (C# UI & Commands)
- Purpose: RhinoCommon commands, UI panels, display, threading.
- Projects: `src/RhinoBimIfcPlugin`, `src/RhinoBimIfc.View`, `src/RhinoBimIfc.GH`
- Inputs:
  - User commands (IfcOpen, IfcListDoors, IfcDoorsToGH, IfcExportBOM).
- Outputs:
  - In-app visualization, baked geometry, progress feedback, logs.

### QA Agent (Quality & Tests)
- Purpose: Unit, integration, and UI smoke tests; golden files for BOM.
- Projects: `tests/` (dotnet), `src/test` (Node viewer), `examples/` sample IFCs.
- Outputs:
  - Test reports; golden snapshots; acceptance criteria checks.

### UX/UI Agent (Panels & Experience)
- Purpose: Eto.Forms panel design (Explorer, Door list, Properties), display conduits.
- Inputs: IFC tree, selection, filters.
- Outputs: Panels, styles, display rules.

### DevOps Agent (CI/CD & Releases)
- Purpose: Build pipelines, signing, versioning, release artifacts.
- Inputs: Repo state; tags.
- Outputs: Build artifacts (RHI/ZIP), release notes, crash reporting hooks.

### Procurement Agent (AI Recommendations)
- Purpose: Analyze BOM, propose suppliers and lead times.
- Inputs: BOM JSON.
- Outputs: Recommendations JSON for planning/purchasing tools.

## Contracts (JSON)

### Orchestrator Input
```json
{
  "project": "HausA_Los3",
  "ifc_path": "./examples/Hello Wall/hello-wall.ifc",
  "actions": ["parse_ifc", "generate_doors", "export_bom"],
  "deadline": "2025-09-15"
}
```

### DoorParam (see DTO and schema)
- DTO: `src/RhinoBimIfc.Core/DTOs/DoorParam.cs`
- JSON Schema: `schemas/doorparam.schema.json`

### BOM Line
- JSON Schema: `schemas/bomline.schema.json`

## Operational Playbooks

### Parse → Generate → Export (Happy Path)
1. IFC Agent loads IFC and extracts doors → `DoorParam` list.
2. Grasshopper Agent builds geometry per door; attaches attributes.
3. BOM/Data Agent aggregates lines; validates against schemas; exports CSV/JSON.
4. DevOps Agent stores artifacts under `out/` and publishes as needed.

### Viewer (Web)
- Source bundling: `src` → `web/viewer/render.mjs` via `npm run build-viewer`.
- Entry page: `web/index.html`.

## Change Management
- Update this document when:
  - Interfaces change (e.g., `IIfcProvider`, DTOs, schemas).
  - New agents or actions are added.
  - File paths or build steps change.

### Notes on Python tooling (Astral uv)
- If Python tasks are required (e.g., utilities under `src/utils/python/`), prefer using Astral's `uv` for fast, reproducible environments.
- Example quickstart:
  - Install `uv` from `https://docs.astral.sh/uv/`.
  - Create and run: `uv venv && uv pip install -r requirements.txt && uv run python your_script.py`.
- This keeps Python tooling isolated and deterministic, aligning with our agents’ reproducibility principle.

### Backlog / TODO
- See `TODO.md` for the prioritized backlog (conversion, plugin wiring, BOM, tests).

