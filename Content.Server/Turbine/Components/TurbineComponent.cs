// it generates energy
using Robust.Shared.Audio;
using Content.Server.Turbine.EntitySystems;
using Content.Shared.Turbine;
using Content.Server.Turbine.Components;

namespace Content.Server.Turbine.Components;

public sealed partial class TurbineComponent : SharedTurbineControllerComponent
{
    /// <summary>
    /// How stable the turbine currently is.
    /// When this falls to <= 0 the turbine explodes.
    /// </summary>
    [DataField("stability")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int Stability = 100;

    [DataField("RPM")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int RPM = 0;

    [DataField("Activated")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool IsOn = false;

    [DataField("nextUpdate")]
    public TimeSpan NextUpdate = default!;
    public TimeSpan NextUIUpdate = default!;
}