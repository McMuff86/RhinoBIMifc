using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace RhinoBimIfcPlugin.Services
{
    public static class IfcPreview
    {
        public static bool TryAddBoundingBoxFromIfcx(string path, RhinoDoc doc, out BoundingBox bbox)
        {
            bbox = BoundingBox.Empty;
            try
            {
                using var stream = File.OpenRead(path);
                using var docJson = JsonDocument.Parse(stream);
                var root = docJson.RootElement;
                var mins = new double[] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity };
                var maxs = new double[] { double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity };

                void Extend(double x, double y, double z)
                {
                    if (x < mins[0]) mins[0] = x; if (y < mins[1]) mins[1] = y; if (z < mins[2]) mins[2] = z;
                    if (x > maxs[0]) maxs[0] = x; if (y > maxs[1]) maxs[1] = y; if (z > maxs[2]) maxs[2] = z;
                }

                void ScanAttributes(JsonElement attributes)
                {
                    foreach (var prop in attributes.EnumerateObject())
                    {
                        var name = prop.Name;
                        var val = prop.Value;
                        if (name == "usd::usdgeom::mesh::points" || name == "points::array::positions")
                        {
                            ExtractPoints(val, Extend);
                        }
                    }
                }

                void Traverse(JsonElement node)
                {
                    if (node.ValueKind != JsonValueKind.Object) return;
                    if (node.TryGetProperty("attributes", out var attrs) && attrs.ValueKind == JsonValueKind.Object)
                    {
                        ScanAttributes(attrs);
                    }
                    if (node.TryGetProperty("children", out var children) && children.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var ch in children.EnumerateArray()) Traverse(ch);
                    }
                }

                if (root.TryGetProperty("root", out var explicitRoot))
                {
                    Traverse(explicitRoot);
                }
                else
                {
                    Traverse(root);
                }

                if (double.IsInfinity(mins[0]) || double.IsInfinity(maxs[0]))
                {
                    return false;
                }

                var minPt = new Point3d(mins[0], mins[1], mins[2]);
                var maxPt = new Point3d(maxs[0], maxs[1], maxs[2]);
                bbox = new BoundingBox(minPt, maxPt);

                var box = new Box(bbox);
                var brep = Brep.CreateFromBox(box);
                if (brep != null)
                {
                    var attr = new ObjectAttributes { ColorSource = ObjectColorSource.ColorFromObject, ObjectColor = System.Drawing.Color.Red }; 
                    doc.Objects.AddBrep(brep, attr);
                    doc.Views.ActiveView?.ActiveViewport.ZoomBoundingBox(bbox);
                    doc.Views.Redraw();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine($"IFCX preview failed: {e.Message}");
                return false;
            }
        }

        private static void ExtractPoints(JsonElement value, Action<double,double,double> onPoint)
        {
            // Handles nested arrays like [[x,y,z], ...] possibly nested deeper
            void Walk(JsonElement el, List<double> buffer)
            {
                if (el.ValueKind == JsonValueKind.Number)
                {
                    if (el.TryGetDouble(out var d)) buffer.Add(d);
                    if (buffer.Count == 3)
                    {
                        onPoint(buffer[0], buffer[1], buffer[2]);
                        buffer.Clear();
                    }
                    return;
                }
                if (el.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in el.EnumerateArray())
                    {
                        Walk(item, buffer);
                    }
                }
            }
            Walk(value, new List<double>(3));
        }
    }
}


