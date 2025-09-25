#if RHINO
using System.Reflection;
using System.Runtime.InteropServices;
using Rhino.PlugIns;

// Optional descriptive metadata shown in Rhino's Plug-in Manager
[assembly: PlugInDescription(DescriptionType.Organization, "RhinoBimIfc")]
[assembly: PlugInDescription(DescriptionType.WebSite, "https://example.com")]
[assembly: PlugInDescription(DescriptionType.Email, "dev@example.com")]

// Unique plug-in GUID
[assembly: Guid("8F1B0C5E-BC1B-4F9F-8B3C-4C7B9E1C1D23")]
#endif


