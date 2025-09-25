using System;
using System.IO;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace RhinoBimIfc.IFC.XBim
{
    public static class XbimLoader
    {
        public static bool TryGetBasicInfo(string path, out int numProducts)
        {
            numProducts = 0;
            try
            {
                using var model = IfcStore.Open(path);
                numProducts = model.Instances.CountOf<IIfcProduct>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
