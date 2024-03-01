using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.Turbine.Components;
using Content.Server.Chat.Managers;
using Content.Server.NodeContainer;
using Content.Server.Power.Components;
using Content.Shared.Turbine;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Popups;
using Robust.Server.Containers;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Server.Turbine.EntitySystems;

public sealed class TurbineControllerSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IAdminManager _adminManager = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly AppearanceSystem _appearanceSystem = default!;
    [Dependency] private readonly ContainerSystem _containerSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;

    public const bool CompressorOn = false;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TurbineControllerComponent, UiButtonPressedMessage>(OnUiButtonPressed);
    }
    private TurbineControllerBoundUserInterfaceState GetUiState(EntityUid uid, TurbineComponent turbine, CompressorComponent compressor)
    {
        float currentPowerSupply = 0;
        // set current power statistics in kW
        if (TryGetTurbineNodeGroup(uid, out var group))
        {
            currentPowerSupply = group.CalculatePower(turbine.RPM);
        }
        if (TryComp<PowerSupplierComponent>(uid, out var powerOutlet) && turbine.RPM > 0)
        {
            currentPowerSupply = powerOutlet.CurrentSupply;
        }
        return new TurbineControllerBoundUserInterfaceState(true, true, turbine.RPM, currentPowerSupply);
    }

    public void UpdateUi(EntityUid uid, TurbineControllerComponent? controller = null, CompressorComponent? compressor = null, TurbineComponent? turbine = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        if (!_userInterfaceSystem.TryGetUi(uid, TurbineControllerUiKey.Key, out var bui))
            return;

        // Verifica se turbine e compressor não são nulos antes de chamar GetUiState
        if (turbine != null && compressor != null)
        {
            var state = GetUiState(uid, turbine, compressor);
            _userInterfaceSystem.SetUiState(bui, state);
        }

        controller.NextUIUpdate = _gameTiming.CurTime + controller.UpdateUIPeriod;
    }

    public override void Update(float frameTime)
    {
        var curTime = _gameTiming.CurTime;
        var query = EntityQueryEnumerator<TurbineControllerComponent, NodeContainerComponent, TurbineComponent, CompressorComponent>();
        while (query.MoveNext(out var uid, out var controller, out var nodes, out var turbine, out var compressor))
        {
            if (controller.NextUpdate <= curTime)
                UpdateController(uid, curTime, controller, nodes, turbine);
            else if (controller.NextUIUpdate <= curTime)
                UpdateUi(uid, controller, compressor, turbine);
        }
    }

    private void UpdateController(EntityUid uid, TimeSpan curTime, TurbineControllerComponent? controller = null, NodeContainerComponent? nodes = null, TurbineComponent? turbine = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        controller.LastUpdate = curTime;
        controller.NextUpdate = curTime + controller.UpdatePeriod;

        if (!TryGetTurbineNodeGroup(uid, out var group, nodes))
            return;

        // Verifica se turbine não é nulo antes de acessar suas propriedades ou métodos
        if (turbine != null)
        {
            // Acessa as propriedades ou métodos de turbine apenas se não for nulo
            turbine.Stability = group.GetTotalStability(turbine.RPM);

            if (turbine.Stability <= 0)
                group.ExplodeTurbine(turbine.RPM, uid);
        }
    }
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
    private void OnUiButtonPressed(EntityUid uid, TurbineControllerComponent comp, UiButtonPressedMessage msg, TurbineComponent turbine, CompressorComponent compressor)
    {
        var user = msg.Session.AttachedEntity;
        if (!Exists(user))
            return;

        _audioSystem.PlayPvs(comp.ClickSound, uid, AudioParams.Default.WithVolume(-2f));
        switch (msg.Button)
        {
            case UiButton.ToggleActivated:
                CompressorActivated();
                break;
        }

        UpdateUi(uid, comp, compressor, turbine);
    }
    private void CompressorActivated()
    {
        Console.WriteLine("Compresor Ativado");
    }

}