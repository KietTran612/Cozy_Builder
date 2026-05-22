using CozyBuilder.Camera;
using CozyBuilder.Town.Debugging;
using CozyBuilder.Town.Data;
using CozyBuilder.Town.Placement;
using CozyBuilder.Town.Rendering;
using CozyBuilder.Town.Rules;
using VContainer;
using VContainer.Unity;

namespace CozyBuilder.Bootstrap
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TownDataStore>(Lifetime.Singleton);
            builder.Register<RuleEvaluator>(Lifetime.Singleton);
            builder.Register<TownVisualRebuilder>(Lifetime.Singleton);
            builder.Register<PlacementService>(Lifetime.Singleton);
            builder.Register<PrototypePlacementState>(Lifetime.Singleton);
            builder.Register<PrototypeTownDebugState>(Lifetime.Singleton);
            builder.Register<CameraService>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<TownGridView>();
            builder.RegisterComponentInHierarchy<PrototypePlacementDebugDriver>();
            builder.RegisterComponentInHierarchy<PrototypePlacementInputDriver>();
            builder.RegisterComponentInHierarchy<PrototypePlacementControlsView>()
                .AsSelf()
                .As<ICameraInputBlocker>();
            builder.RegisterComponentInHierarchy<PrototypeTownDebugView>()
                .AsSelf()
                .As<ICameraInputBlocker>();
            builder.RegisterComponentInHierarchy<PrototypeCameraInputDriver>();
        }
    }
}
