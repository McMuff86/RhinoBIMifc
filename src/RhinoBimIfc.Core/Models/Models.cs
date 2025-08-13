using System;
using System.Collections.Generic;

namespace RhinoBimIfc.Core.Models
{
    public sealed class IfcModelInfo
    {
        public string FilePath { get; init; } = string.Empty;
        public string SchemaVersion { get; init; } = string.Empty;
        public DateTime LoadedAtUtc { get; init; } = DateTime.UtcNow;
    }

    public sealed class IfcDoorInfo
    {
        public Guid Guid { get; init; }
        public string Name { get; init; } = string.Empty;
        public double? OverallWidth { get; init; }
        public double? OverallHeight { get; init; }
        public IReadOnlyDictionary<string, object?> Properties { get; init; } = new Dictionary<string, object?>();
    }

    public sealed class PropertyBag
    {
        public IReadOnlyDictionary<string, object?> Values { get; }
        public PropertyBag(IReadOnlyDictionary<string, object?> values) => Values = values;
    }

    public enum IfcUnits
    {
        Unknown = 0,
        Millimeters,
        Meters,
    }
}


