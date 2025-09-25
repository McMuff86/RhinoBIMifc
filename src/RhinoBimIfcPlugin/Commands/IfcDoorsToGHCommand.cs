#if RHINO
using Rhino;
using Rhino.Commands;

namespace RhinoBimIfcPlugin.Commands
{
    public class IfcDoorsToGHCommand : Command
    {
        public override string EnglishName => "IfcDoorsToGH";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("IfcDoorsToGH stub – bind DoorParam to GH and bake geometry.");
            return Result.Success;
        }
    }
}
#endif


