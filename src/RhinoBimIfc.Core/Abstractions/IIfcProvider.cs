using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RhinoBimIfc.Core.Models;

namespace RhinoBimIfc.Core.Abstractions
{
    public interface IIfcProvider
    {
        Task<IfcModelInfo> LoadAsync(string path, CancellationToken cancellationToken);
        IEnumerable<IfcDoorInfo> GetDoors();
        object? TryGetBrep(Guid ifcGuid);
        IfcUnits Units { get; }
        PropertyBag GetProperties(Guid ifcGuid);
    }
}


