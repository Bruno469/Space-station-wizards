using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.Turbine.Components;
using Content.Server.Chat.Managers;
using Content.Server.NodeContainer;
using Content.Server.Power.Components;
using Content.Shared.Turbine;
using Content.Shared.Database;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Mind.Components;
using Content.Shared.Popups;
using Robust.Server.Containers;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.Turbine.EntitySystems;

public sealed class TurbineControllerSystem : EntitySystem
{
    /*
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;

    public const bool CompressorOn = false;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TurbineComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<TurbineComponent, InteractUsingEvent>(OnInteractUsing);
        SubscribeLocalEvent<TurbineComponent, PowerChangedEvent>(OnPowerChanged);
        SubscribeLocalEvent<TurbineComponent, UiButtonPressedMessage>(OnUiButtonPressed);
    }
    public override void Update(float frameTime)
    {
        var curTime = _gameTiming.CurTime;
        var query = EntityQueryEnumerator<TurbineComponent, NodeContainerComponent>();
        while (query.MoveNext(out var uid, out var controller, out var nodes))
        {
            if (controller.NextUpdate <= curTime)
                UpdateController(uid, curTime, controller, nodes);
            else if (controller.NextUIUpdate <= curTime)
                UpdateUi(uid, controller);
        }
    }
    */

    /*
    private void UpdateController(EntityUid uid, TimeSpan curTime, TurbineComponent? controller = null, NodeContainerComponent? nodes = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        controller.LastUpdate = curTime;
        controller.NextUpdate = curTime + controller.UpdatePeriod;

        if (!TryGetTurbineNodeGroup(uid, out var group, nodes))
            return;
        controller.Stability = group.GetTotalStability();

        group.UpdateCombuterVisuals();
        UpdateDisplay(uid, controller.Stability, controller);

        if (controller.Stability <= 0)
            group.Explode();
    */

    // public void UpdateUi(EntityUid uid, TurbineComponent? controller = null)
    // {
    //     if (!Resolve(uid, ref controller))
    //         return;

    //     if (!_userInterfaceSystem.TryGetUi(uid, TurbineControllerUiKey.Key, out var bui))
    //         return;

    //     var state = GetUiState(uid, controller);
    //     _userInterfaceSystem.SetUiState(bui, state);

    //     controller.NextUIUpdate = _gameTiming.CurTime + controller.UpdateUIPeriod;
    // }

    // private TurbineControllerBoundUserInterfaceState GetUiState(EntityUid uid, TurbineComponent controller)
    // {
    //     // set current power statistics in kW
    //     float currentPowerSupply = group.CalculatePower(controller.RPM);
    //     if (TryComp<PowerSupplierComponent>(uid, out var powerOutlet) && coreCount > 0)
    //     {
    //         currentPowerSupply = powerOutlet.CurrentSupply;
    //     }
    //     return new TurbineControllerBoundUserInterfaceState(powered, IsMasterController(uid), controller.ToggleActivated, jar.RotationStatus, CurrentPowerSupply);
    // }

    /*
    private bool IsMasterController(EntityUid uid)
    {
        return TryGetTurbineNodeGroup(uid, out var group) && group.MasterController == uid;
    }
    */

    private bool TryGetTurbineNodeGroup(EntityUid uid, [MaybeNullWhen(false)] out TurbineNodeGroup group, NodeContainerComponent? nodes = null)
    {
        if (!Resolve(uid, ref nodes))
        {
            group = null;
            return false;
        }

        group = nodes.Nodes.Values
            .Select(node => node.NodeGroup)
            .OfType<TurbineNodeGroup>()
            .FirstOrDefault();

        return group != null;
    }

}