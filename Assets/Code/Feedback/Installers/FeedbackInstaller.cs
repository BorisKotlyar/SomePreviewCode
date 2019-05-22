using Feedback;
using Zenject;

public class FeedbackInstaller<T, A> : Installer<FeedbackInstaller<T, A>>
    where T : FeedbackController where A : IFeedbackRequestChecker
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(FeedbackController))
            .To<T>()
            .AsSingle();

        Container.BindInterfacesAndSelfTo<A>()
            .AsSingle();
    }
}