using TheGame.Code.RaceSystem;
using TheGame.Code.UI;
using VContainer;
using VContainer.Unity;

namespace TheGame.Code.DI
{
    public class GameSceneLifeScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TrajectoryRecorder>(Lifetime.Scoped)
                .As<ITrajectoryRecorder>();

            builder.Register<RacePresenter>(Lifetime.Scoped)
                .AsSelf();

            builder.RegisterComponentInHierarchy<RaceManager>();
        }
    }
}