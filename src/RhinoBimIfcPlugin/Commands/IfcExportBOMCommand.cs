#if RHINO
using Rhino;
using Rhino.Commands;

namespace RhinoBimIfcPlugin.Commands
{
    public class IfcExportBOMCommand : Command
    {
        public override string EnglishName => "IfcExportBOM";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("IfcExportBOM stub – aggregate BOM and export CSV/JSON.");
            return Result.Success;
        }
    }
}
#endif


