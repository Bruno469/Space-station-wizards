using Content.Shared.Turbine;
using JetBrains.Annotations;
using Robust.Client.GameObjects;

namespace Content.Client.Turbine.UI
{
    [UsedImplicitly]
    public sealed class TurbineControllerBoundUserInterface : BoundUserInterface
    {
        private TurbineWindow? _window;

        public TurbineControllerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _window = new TurbineWindow(this);
            _window.OnClose += Close;
            _window.OpenCentered();
        }

        /// <summary>
        /// Update the ui each time new state data is sent from the server.
        /// </summary>
        /// <param name="state">
        /// Data of the <see cref="SharedReagentDispenserComponent"/> that this ui represents.
        /// Sent from the server.
        /// </param>
        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            var castState = (TurbineControllerBoundUserInterfaceState) state;
            _window?.UpdateState(castState); //Update window state
        }

        public void ButtonPressed(UiButton button)
        {
            SendMessage(new UiButtonPressedMessage(button));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _window?.Dispose();
            }
        }
    }
}