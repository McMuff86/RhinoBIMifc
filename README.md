Rhino BIM IFC Plugin – README

Ziel: Ein Rhino‑8‑Plugin (C#) + Grasshopper‑Toolchain, das IFC‑Modelle performant in 3D darstellt, relevante Informationen (v. a. Türen) extrahiert und daraus automatisiert parametrische Türrahmen/Zargen mit Türblatt erzeugt. Zusätzlich wird eine vollständige Werkstoffliste (BOM) generiert und an einen AI‑gestützten Workflow für Planung, Termine und Bestellvorschläge übergeben.

1) Elevator Pitch

Warum: Türen aus IFC schnell, korrekt und reproduzierbar in Rhino/Grasshopper umsetzen – inklusive Stücklisten für Fertigung und Einkauf.

Was: Plugin zum Laden/Filtern/Visualisieren von IFC, Mappings IfcDoor → GH‑Param und automatisches Erzeugen der Türgeometrie + BOM.

Outcome:

3D‑Vorschau in Rhino

Parametrische Geometrie via Grasshopper

Validierte BOM (CSV/JSON) für ERP/Bestellung

Übergabe an AI‑Agenten für Zeitplan & Beschaffung

2) Architekturüberblick

[IFC File] → [IFC Provider Adapter] → [IFC Cache/Index]
      → (a) [Viewer Layer in Rhino]  
      → (b) [Door Extractor] → [DoorParam DTO] → [GH Runner] → [Rhino Geometry]
                                       ↘︎ [BOM Builder] → [CSV/JSON]
                                                      ↘︎ [AI Orchestrator I/O]

Kernideen

Adapter‑Pattern für IFC‑Parser (Basis: IFC development repository als Submodule; Fallback: XBimToolkit).

Strikte Trennung: IFC Lesen/Indexieren, Visualisierung, Parametik/Grasshopper, BOM.

Headless GH‑Runner im Plugin (Grasshopper API) für automatisierte Geometrie‑Erzeugung.

Robuste Einheitenverwaltung (z. B. UnitsNet) und Caching größerer IFCs.

3) Technologien & Standards

Rhino 8, C#/.NET 8, RhinoCommon API, Grasshopper SDK

IFC: Adapter für IFC development repo (Primär), XBim.Essentials/Geometry (Fallback)

JSON Schemas für DoorParam & BOM (OpenAPI/JSON Schema)

Tests: xUnit + FluentAssertions; Playwright für UI‑Smoke via Rhino.Inside Test Harness

CI/CD: GitHub Actions (Build, Unit‑ & Integration‑Tests, Sign, Release)

Code‑Style: EditorConfig, .NET Analyzers (CA, IDE, StyleCop optional)

4) Repository‑Struktur

root
├── src
│   ├── RhinoBimIfcPlugin/           # Rhino Plug-In (C#)
│   ├── RhinoBimIfc.Core/            # Domain, DTOs, Services, IFC Adapter Interfaces
│   ├── RhinoBimIfc.IFC.Adapter/     # Adapter zum „IFC development repository“ (Submodule)
│   ├── RhinoBimIfc.IFC.XBim/        # Fallback Adapter (XBim)
│   ├── RhinoBimIfc.GH/              # GH Runner + Helpers (Param Binding, Bake)
│   └── RhinoBimIfc.View/            # DisplayConduits, Layering, Styles
├── gh
│   └── DoorGenerator.ghx            # Referenz-GH-Definition (parametrischer Türrahmen/Zarge)
├── schemas
│   ├── doorparam.schema.json
│   └── bomline.schema.json
├── samples
│   ├── small_house.ifc
│   └── test_project.ifc
├── tests
│   ├── RhinoBimIfc.Core.Tests/
│   ├── RhinoBimIfc.IFC.Adapter.Tests/
│   └── RhinoBimIfc.Integration.Tests/
└── README.md (dieses Dokument)

5) AI‑Agent Team & Rollen (klar definiert)

Orchestrator‑Agent (Lead): plant Sprints/Epics, priorisiert Backlog, triggert Spezialisten, konsolidiert Ergebnisse.

Rhino‑Plugin‑Agent (C#): RhinoCommon, Commands, UI, Display, Thread‑Marshalling.

IFC‑Agent (Parsing): Adapter zum IFC development repo, Datenmodell, Index/Bucket‑Struktur, Einheiten.

Grasshopper‑Agent: GH‑Definition (Türparametrik), API‑Binding, Headless Runs, Bake‑Strategie.

BOM/Data‑Agent: Mapping Tür → BOM, Materialregeln, CSV/JSON, Validierung & Dedup.

QA‑Agent: Tests (Unit/Integration/UI), Testdaten, Akzeptanzkriterien, Regression.

UX/UI‑Agent: Panels, Wizard „Tür‑Batch“, Status/Progress, Fehlermeldungen.

DevOps‑Agent: CI/CD, Signing, Versionierung, Release Artefakte, Crash‑Reports (Sentry o. ä.).

Procurement‑Agent (AI): Bestellvorschläge, Lieferzeiten, Alternativen, Übergabe an ERP.

Kommunikation

Alle Agenten berichten Ergebnisse als structured JSON an den Orchestrator.

Konvention: role, inputs, outputs, evidence, risks, next_actions.

6) Epics & Aufgaben (mit Zuweisung)

EPIC A – IFC Basis

A1 (IFC‑Agent): Adapter‑Interface IIfcProvider definieren (Laden, Einheiten, Element‑Query, PropertySets).

A2 (IFC‑Agent): Implementierung IfcDevAdapter (Submodule einbinden, Reader, Caches, Door‑Query).

A3 (IFC‑Agent): Fallback XbimAdapter (nur Read‑Only, IfcDoor + Geometrie‑BReps).

A4 (QA‑Agent): Test‑IFCs + Gold‑Standards (Türanzahl, Maße, Psets) erstellen.

EPIC B – Rhino Plugin Core

B1 (Rhino‑Plugin‑Agent): PlugIn‑Skelett, Settings, Logging, Error‑Boundary.

B2 (Rhino‑Plugin‑Agent): Commands: IfcOpen, IfcDoorPreview, IfcDoorsToGH.

B3 (UX/UI‑Agent): Panel „IFC Explorer“ (Baum/Filter), Door‑Liste, Property‑View.

B4 (Rhino‑Plugin‑Agent): DisplayConduit für farbige Kategorien (Tür, Wand, etc.).

EPIC C – Grasshopper Integration

C1 (Grasshopper‑Agent): Referenz DoorGenerator.ghx anlegen (Türblatt/Rahmen/Zarge param.).

C2 (Grasshopper‑Agent): API‑Binding: SetParam by name, trigger ScheduleSolution, return geometry.

C3 (Grasshopper‑Agent): Bake‑Service mit Layer‑Konzept & Attribute (UserText: Tür‑ID, Typ, Maße).

EPIC D – Mapping & Regeln

D1 (IFC‑ & GH‑Agent): Mapping Spezifikation IfcDoor → DoorParam (siehe §7) fixieren.

D2 (BOM/Data‑Agent): Material‑/Hardware‑Regeln (Feuerschutz/Schallschutz/Anschlag).

D3 (QA‑Agent): Mapping‑Tests (Kantenfälle: fehlende Maße, unklare Psets, Einheitenmix).

EPIC E – BOM & Export

E1 (BOM/Data‑Agent): BOM Builder (Aggregation, Stückzahlen, Abmessungen, Zuschnittmaß).

E2 (BOM/Data‑Agent): Exporter (CSV, JSON) mit Schemas & Validierung.

E3 (QA‑Agent): Snapshots/Golden Files für BOM‑Vergleich.

EPIC F – AI‑Workflow I/O

F1 (Orchestrator‑ & Procurement‑Agent): JSON‑Schnittstellen definieren (Plan/Bestellung).

F2 (DevOps‑Agent): CLI Hook oder HTTP Localhost Endpoint für AI‑Agenten.

F3 (QA‑Agent): Contract‑Tests gegen AI‑Samples.

7) Mapping‑Spezifikation: IfcDoor → DoorParam (V1)

Quelle (IFC):

IfcDoor.OverallHeight, IfcDoor.OverallWidth

IfcDoorType.OperationType (falls vorhanden) → Anschlag/Öffnungsart

Property Sets: Pset_DoorCommon.FireRating, Pset_DoorCommon.AcousticRating, Pset_DoorCommon.IsExternal

Lage/Richtung: IfcLocalPlacement + Transform – berechnet Weltlage und Öffnungsrichtung

Ziel (DoorParam DTO):

{
  "id": "GUID",
  "name": "string",
  "width_mm": 985,
  "height_mm": 2110,
  "thickness_mm": 40,
  "handing": "Left|Right",
  "operation": "SingleSwing|DoubleSwing|Sliding|Folding|Other",
  "fire_rating": "EI30|EI60|null",
  "acoustic_rating_db": 32,
  "is_external": false,
  "frame_profile": "ZARGE_STD_78",
  "leaf_material": "MDF_LACK",
  "hardware_set": "HS_STD_1",
  "world_transform": [16 floats row-major],
  "notes": ""
}

Regeln

Maße & Einheiten konsequent in mm normalisieren.

handing aus OperationType + Türnormalenrichtung ableiten.

Fallbacks: Wenn Maße fehlen → aus Öffnung ableiten (IfcRelFillsElement / OpeningElement).

Material/Hardware per Regelbaum (z. B. FireRating → anderes Hardware‑Set).

8) BOM‑Schema (V1)

{
  "id": "uuid",
  "door_id": "GUID",
  "item_code": "string",
  "description": "string",
  "category": "Frame|Leaf|Hardware|Seal|Finish|Misc",
  "material": "string",
  "length_mm": 2110,
  "width_mm": 985,
  "thickness_mm": 40,
  "qty": 2,
  "unit": "pcs|m|set",
  "source": "GH|IFC",
  "notes": ""
}

Export: out/bom_{project}_{timestamp}.csv und .json (+ Schema‑Validation).

9) Rhino Plugin – Technische Leitplanken

Commands: IfcOpen, IfcListDoors, IfcDoorsToGH, IfcExportBOM.

UI: Eto.Forms Panel (TreeView: IFC Struktur, Grid: Türen, Property Panel).

Display: DisplayConduit für Highlights; Layerfarben nach Typ.

Threading: IFC‑Parsing & GH‑Runs async; UI‑Updates via RhinoApp.InvokeOnUiThread.

Settings: JSON in %AppData%/RhinoBimIfc/settings.json (Pfad GH‑Def, Exportordner, Regeln).

Logging: Serilog (Rolling File); Fehlerdialog mit „Report“.

10) Grasshopper – Integration

Loading: GH_DocumentIO lädt DoorGenerator.ghx read‑only.

Param Binding: Parameter/Sliders per eindeutigen Namen setzen (IGH_Param), Typprüfung.

Run: doc.ScheduleSolution mit Callback; Ergebnis über IGH_BakeAwareObject oder Explizit‑Bake‑Service.

Attributierung: UserText an gebakten Objekten (door_id, Material, Typ) + Layerstruktur.

Batch: Mehrere Türen sequentiell, Fortschrittsanzeige, Abbruch möglich.

11) Code‑Skelette (Kurz)

IIfcProvider.cs

public interface IIfcProvider
{
    Task<IfcModelInfo> LoadAsync(string path, CancellationToken ct);
    IEnumerable<IfcDoorInfo> GetDoors();
    Brep TryGetBrep(Guid ifcGuid); // für Vorschau
    IfcUnits Units { get; }
    PropertyBag GetProperties(Guid ifcGuid);
}

DoorParam.cs

public record DoorParam(
    Guid Id, string Name, double WidthMm, double HeightMm, double ThicknessMm,
    string Handing, string Operation, string? FireRating, int? AcousticRatingDb,
    bool IsExternal, string FrameProfile, string LeafMaterial, string HardwareSet,
    double[] WorldTransform, string? Notes
);

GhRunner.cs (Ausschnitt)

public Task<GhResult> BuildDoorAsync(DoorParam param, CancellationToken ct)
{
    var gh = GhDocumentLoader.Load(_settings.GhDoorPath);
    GhParamSetter.Set(gh, "width_mm", param.WidthMm);
    // ... weitere Params
    var geom = GhExecutor.RunAndCollect(gh, outputs: ["frame", "leaf"], ct);
    return Task.FromResult(new GhResult(geom));
}

12) Tests & Qualität

Unit: Mappingtests (IfcDoor→DoorParam), Einheitenkonvertierung, BOM‑Aggregation.

Integration: IFC‑Sample laden, N Türen generieren, BOM vergleichen (Golden Files).

UI Smoke: Öffnen IFC, Liste erscheinen, Preview‑Highlight.

Performance: Zeitbudget pro 100 Türen, Memory‑Profiling.

Akzeptanzkriterien (Beispiele)

A: Für ein Sample mit 50 Türen werden ≥48 korrekt erkannt; Maße ±2 mm; BOM validiert.

B: DoorGenerator.ghx erzeugt Geometrie ohne User‑Interaktion (Headless).

13) Dev Setup

Prereqs: Rhino 8, .NET 8 SDK, Git LFS (für Samples), Visual Studio 2022.

git submodule add <IFC development repository URL> src/RhinoBimIfc.IFC.Adapter/IfcDev

Build‐Order: Core → IFC.Adapter → GH → Plugin.

In Rhino: Packages → Plugin laden (Debug Attach) → Test mit samples/*.ifc.

Pfade in settings.json prüfen (GH‑Def, Exportordner).

14) CI/CD

GitHub Actions:

build.yml: Restore, Build, Test, Pack (Rhino RHI/ZIP), Artefakte.

release.yml: Tag Trigger, Sign, Release Notes, Uploads.

Optional: Crash‑Reports (Sentry) + anonyme Nutzungsmetriken (Opt‑In).

15) Sicherheit & Datenschutz

IFC‑, BOM‑ und Projektdateien bleiben lokal (Standard).

Export nur auf Nutzerwunsch.

Keine personenbezogenen Daten in Logs; Maskierung von Projektnamen optional.

16) AI‑Agent Schnittstellen

Input für Orchestrator

{
  "project": "HausA_Los3",
  "ifc_path": "./samples/small_house.ifc",
  "actions": ["parse_ifc","generate_doors","export_bom"],
  "deadline": "2025-09-15"
}

Output an Procurement‑Agent (Beispiel)

{
  "project": "HausA_Los3",
  "bom_file": "out/bom_HausA_2025-08-13.json",
  "recommendations": [
    {"item_code":"HS_STD_1","qty":50,"supplier":"Maco","eta_days":5},
    {"item_code":"SEAL_X45","qty":100,"supplier":"Athmer","eta_days":3}
  ]
}

17) Offene Punkte & Roadmap

V2: Mehr Türtypen (Doppelflügel, Schiebetüren), Zargenvarianten, Brandschutz‑Sets.

V2: Kollisionschecks (Türflügel vs. Umgebung), Raumzuordnung (IfcSpace).

V2: Direkte ERP‑Integration (z. B. Borm) via Connector.

V3: Rule‑Editor im UI (No‑Code‑Regeln für Material/Hardware).

V3: Batch‑Runs über Rhino.Compute Worker Farm.

18) Referenzen & Best Practices

Rhino Developer Docs (bei Unklarheiten IMMER zuerst prüfen): RhinoCommon, Grasshopper API, DisplayConduits, Threading.

IFC: buildingSMART Specs (IfcDoor, Psets).

.NET: Async/await, Dependency Injection, Logging, Unit Tests.

19) Lizenz & Beitrag

Lizenz: TBD (z. B. MIT).

Beiträge via Pull Requests mit Tests und kurzer Architekturbegründung.

20) Kurzanleitung (Happy Path)

IfcOpen → IFC laden.

IfcListDoors → Türen prüfen, Filter setzen.

IfcDoorsToGH → Geometrie + BOM generieren (Batch).

IfcExportBOM → CSV/JSON prüfen → an AI‑Orchestrator übergeben.

Viel Erfolg beim Build! 💪


Appendix über IFC 5:

# IFC 5 development repository

Welcome to the **IFC 5 alpha Examples**!  These examples are the result of years of work by many volunteers. The IFC implementer forum, the IFC 5 taskforce, and the wider community of interested implementers, have been working on these examples inntensively. This repository contains initial examples for the IFC 5 developments.

## Disclaimer: Early Stage Examples

Please note that these examples are **preliminary** and represent a direction of working for IFC 5. There are several **important caveats** to keep in mind:

1. **Incomplete Features**: Many features of IFC 5 have not been fully explored or implemented in these examples. 
2. **Schema Changes**: IFC 5 is still evolving, and future updates to the development will require revisions to these examples.
3. **Limited Validation**: These examples have undergone significant validation and testing. However, they are still incomplete.
4. **Known Issues**: There are known and unknown issues and incomplete sections within the examples.
5. **Development in Progress**: Further work is needed to improve the quality, accuracy, and completeness of these examples.

## Viewer

A viewer to visualize and explore the files is available under /docs/viewer. It is also live on https://ifc5.technical.buildingsmart.org/viewer/
The viewer source code is MIT licensed. The intent is to help users understand the examples; and to help software developers to understand how to implement the composition of the objects.

## Schema

The schema sourece is defined using typespec and can be found under /schema.
The JSON schema of IFC, and extensions will be published on ifcx.dev

## Future Development

Further **documentation will follow soon**. 
We are actively working on enhancing these examples, addressing known issues. Contributions, feedback, and collaboration are welcome! If you would like to contribute or discuss the development of these examples, feel free to open an issue.
 

## Usage

You are welcome to clone or download this repository, but please bear in mind the current limitations and treat these examples as a **work in progress**. 
Do not create derivatives of these examples, but please actively contribute with PRs and opening issues.

## Feedback and Contributions

We highly encourage feedback from the community and contributions from those familiar with IFC 5 or similar standards. Please adhere to the buildingSMART behavior policy when discussing on GitHub and the forums.  

---

**Please Note**: The examples provided here are for **educational and testing purposes** only. They are not suitable for production use without further refinement.