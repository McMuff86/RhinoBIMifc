#if RHINO
using System;
using System.IO;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;
using RhinoBimIfcPlugin.Services;
using RhinoBimIfc.IFC.XBim;

namespace RhinoBimIfcPlugin.Commands
{
    public class IfcOpenCommand : Command
    {
        public override string EnglishName => "IfcOpen";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            try
            {
                string? path = null;
                if (mode == RunMode.Interactive)
                {
                    var ofd = new OpenFileDialog
                    {
                        Title = "Open IFC/IFCX",
                        Filter = "IFC/IFCX files (*.ifc;*.ifcx;*.json)|*.ifc;*.ifcx;*.json|All files (*.*)|*.*"
                    };
                    if (!ofd.ShowOpenDialog())
                        return Result.Cancel;
                    path = ofd.FileName;
                }
                else
                {
                    var gi = new GetString();
                    gi.SetCommandPrompt("Enter path to IFC/IFCX file");
                    if (gi.Get() != Rhino.Input.GetResult.String)
                        return Result.Cancel;
                    path = gi.StringResult().Trim('"');
                }

                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    RhinoApp.WriteLine("File not found.");
                    return Result.Failure;
                }

                var ext = Path.GetExtension(path).ToLowerInvariant();
                bool isIfcx = ext == ".ifcx" || ext == ".json";
                bool isIfc = ext == ".ifc" || ext == ".stp" || ext == ".step";

                if (!isIfcx && !isIfc)
                {
                    RhinoApp.WriteLine("Unsupported file type: {0}", ext);
                    return Result.Failure;
                }

                // For now we do not parse. We remember the selection and advise conversion if needed.
                IfcSession.Instance.Set(path, isIfcx);

                if (isIfc)
                {
                    // Minimal sanity check via XBim and draw a bounding box placeholder (no meshing yet)
                    if (XbimLoader.TryGetBasicInfo(path, out var count))
                    {
                        RhinoApp.WriteLine("IFC file opened (products: {0}). Preview placeholder only.", count);
                        // No geometry extraction here; optional: just mark file loaded
                    }
                    else
                    {
                        RhinoApp.WriteLine("IFC open failed. Ensure file is valid.");
                    }
                }
                else
                {
                    RhinoApp.WriteLine("Loaded IFCX path: {0}", path);
                    // Try a very simple preview: compute a bounding box from point arrays if present
                    if (!IfcPreview.TryAddBoundingBoxFromIfcx(path, doc, out var bbox))
                    {
                        RhinoApp.WriteLine("No preview geometry found in IFCX. Later stages will generate geometry via GH.");
                    }
                }

                return Result.Success;
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("IfcOpen error: {0}", ex.Message);
                return Result.Failure;
            }
        }
    }
}
#endif


