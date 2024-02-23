using Robust.Shared.Serialization;

namespace Content.Shared.Turbine;

[Virtual]
public partial class SharedTurbineControllerComponent : Component
{
}

[Serializable, NetSerializable]
public sealed class TurbineControllerBoundUserInterfaceState : BoundUserInterfaceState
{
    public readonly bool HasPower;
    public readonly bool IsMaster;
    public readonly int RPM;
    public readonly float CurrentPowerSupply;

    public TurbineControllerBoundUserInterfaceState(bool hasPower, bool isMaster, int rpm, float currentPowerSupply)
    {
        HasPower = hasPower;
        IsMaster = isMaster;
        RPM = rpm;
        CurrentPowerSupply = currentPowerSupply;
    }
}

[Serializable, NetSerializable]
public sealed class UiButtonPressedMessage : BoundUserInterfaceMessage
{
    public readonly UiButton Button;

    public UiButtonPressedMessage(UiButton button)
    {
        Button = button;
    }
}

[Serializable, NetSerializable]
public enum TurbineControllerUiKey
{
    Key
}

public enum UiButton
{
    ToggleActivated
}