using System.Linq;
using Content.Server.Turbine.Components;
using Content.Server.Turbine.EntitySystems;
using Content.Server.Explosion.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Robust.Server.GameObjects;
using Robust.Shared.Map.Components;
using Content.Server.Damage.Components;

namespace Content.Server.Turbine;

[NodeGroup(NodeGroupID.TurbineEngine)]
public sealed class TurbineNodeGroup : BaseNodeGroup
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [ViewVariables]
    // public EntityUid? MasterController = GetEntityQuery<TurbineComponent>();

    public override void LoadNodes(List<Node> groupNodes)
    {
        base.LoadNodes(groupNodes);

        var compressorQuery = _entMan.GetEntityQuery<CompressorComponent>();
    }

    /// <summary>
    /// Calculates the amount of power the turbine can produce with the given settings
    /// </summary>
    public float CalculatePower(int rpm)
    {
        return (float) (2 * 3.14159 * rpm / 60);
    }

    public int GetTotalStability(int rpm)
    {
        if (rpm <= 30000)
            return 100;
        else
        {
            return 0;
        }
    }

    public void ExplodeTurbine(int rpm, EntityUid uid)
    {
        var radius = Math.Min(2 * rpm, 8f);
        _entMan.System<ExplosionSystem>().TriggerExplosive(uid, radius: radius, delete: false);
    }
}