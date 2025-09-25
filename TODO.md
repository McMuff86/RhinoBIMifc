## RhinoBimIfc – Backlog / TODO

Short actionable items the agents should implement next. Keep this file updated.

### IFCX ↔ IFC Conversion
- Define conversion strategy for `.ifcx` (IFCX JSON) → `.ifc` (STEP):
  - Evaluate using the IFC development repository composition → STEP export.
  - Alternative: Generate IFC 4x3 entities via XBim from IFCX graph (mapping layer).
  - Decide primary path, keep other as fallback.
- Implement CLI: extend `src/ifcx-cli/ifcx-cli.ts` with `convert_ifcx_to_ifc <in.ifcx> <out.ifc>`.
- Implement reverse path (optional, for viewer): `.ifc` → `.ifcx` via adapter or service.
- Add schema validation before/after conversion; log normalization (units to mm).
- Tests: golden files for a small model (Hello Wall). Compare counts/dimensions.

### Viewer
- Already guards against `.ifc` uploads. Add info link to converter once available.
- Optional: URL param to auto-load sample `.ifcx`.

### Rhino Plug‑in (turn class library into actual PlugIn)
- Add `RhinoCommon` NuGet reference; target Rhino 8.
- Create `PlugIn` class deriving `Rhino.PlugIns.PlugIn` with GUID and name.
- Add basic commands: `IfcOpen`, `IfcListDoors`, `IfcDoorsToGH`, `IfcExportBOM` (stubs).
- Debug config: start external program `Rhino.exe` with plugin registration during first run.

### IFC Adapter(s)
- Implement `IIfcProvider` primary adapter (IFC development repository) in `src/RhinoBimIfc.IFC.Adapter/`.
- Implement fallback XBim adapter (read-only) in `src/RhinoBimIfc.IFC.XBim/`.
- Unit tests around door extraction and properties mapping.

### Grasshopper Runner
- Add GH runner in `src/RhinoBimIfc.GH/` to bind `DoorParam` to `gh/DoorGenerator.ghx`.
- Bake service with attributes (UserText) and layer strategy.

### BOM & Data
- Implement BOM builder from geometry/parameters. Validate against `schemas/bomline.schema.json`.
- Export `.csv` and `.json` to `out/`.

### Orchestrator & Agents
- Define local CLI or HTTP entry for the orchestrator to trigger: parse → generate → export.
- Contracts live in `Agents.md` (keep in sync with `schemas/` and TypeSpec schema under `schema/`).

### Python Utilities (optional)
- If using Python utilities under `src/utils/python/`, standardize via Astral’s uv:
  - `uv venv && uv pip install -r requirements.txt && uv run python tool.py`.


