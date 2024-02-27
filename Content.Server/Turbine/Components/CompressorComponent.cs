namespace Content.Server.Turbine.Components
{
    public sealed partial class CompressorComponent : Component
    {
        [DataField("Activated")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool ToggleActivated = false;
    }
}