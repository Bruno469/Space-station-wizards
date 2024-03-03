using Robust.Shared.Audio;
using Content.Server.Turbine.EntitySystems;
using Content.Shared.Turbine;

namespace Content.Server.Turbine.Components;

[Access(typeof(TurbineControllerSystem), typeof(TurbineNodeGroup))]
[RegisterComponent]
public partial class TurbineControllerComponent : SharedTurbineControllerComponent
{
    [ViewVariables]
    public TimeSpan UpdateUIPeriod = TimeSpan.FromSeconds(3.0);

    [DataField("nextUpdate")]
    public TimeSpan NextUpdate = default!;
    public TimeSpan NextUIUpdate = default!;

    [DataField("clickSound")]
    [ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier ClickSound = new SoundPathSpecifier("/Audio/Machines/machine_switch.ogg");

    [DataField("lastUpdate")]
    public TimeSpan LastUpdate = default!;

    [DataField("updatePeriod")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan UpdatePeriod = TimeSpan.FromSeconds(10.0);
}