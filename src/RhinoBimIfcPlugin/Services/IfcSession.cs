using System;

namespace RhinoBimIfcPlugin.Services
{
    public sealed class IfcSession
    {
        private static readonly Lazy<IfcSession> _instance = new Lazy<IfcSession>(() => new IfcSession());
        public static IfcSession Instance => _instance.Value;

        private IfcSession() { }

        public string? FilePath { get; private set; }
        public bool IsIfcx { get; private set; }
        public DateTimeOffset? LoadedAt { get; private set; }

        public void Set(string path, bool isIfcx)
        {
            FilePath = path;
            IsIfcx = isIfcx;
            LoadedAt = DateTimeOffset.Now;
        }
    }
}


