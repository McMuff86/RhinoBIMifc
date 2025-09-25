#if RHINO
using System;
using System.Runtime.InteropServices;
using Rhino;
using Rhino.PlugIns;

namespace RhinoBimIfcPlugin
{
    [Guid("8F1B0C5E-BC1B-4F9F-8B3C-4C7B9E1C1D23")]
    public class RhinoBimIfcPlugIn : PlugIn
    {
        public static RhinoBimIfcPlugIn? Instance { get; private set; }

        public RhinoBimIfcPlugIn()
        {
            Instance = this;
        }

        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            RhinoApp.WriteLine("RhinoBimIfcPlugIn loaded.");
            return LoadReturnCode.Success;
        }
    }
}
#endif


