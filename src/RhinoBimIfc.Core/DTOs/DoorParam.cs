using System;

namespace RhinoBimIfc.Core.DTOs
{
    public record DoorParam(
        Guid Id,
        string Name,
        double WidthMm,
        double HeightMm,
        double ThicknessMm,
        string Handing,
        string Operation,
        string? FireRating,
        int? AcousticRatingDb,
        bool IsExternal,
        string FrameProfile,
        string LeafMaterial,
        string HardwareSet,
        double[] WorldTransform,
        string? Notes
    );
}


