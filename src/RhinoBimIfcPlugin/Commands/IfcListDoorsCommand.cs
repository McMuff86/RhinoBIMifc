#if RHINO
using Rhino;
using Rhino.Commands;

namespace RhinoBimIfcPlugin.Commands
{
    public class IfcListDoorsCommand : Command
    {
        public override string EnglishName => "IfcListDoors";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("IfcListDoors stub – query IIfcProvider and list to console/ui.");
            return Result.Success;
        }
    }
}
#endif


