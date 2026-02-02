namespace GameTemplate.UI
{
    public class UIQueueNavigatorInstaller : UINavigatorInstaller<UIQueueScreen>
    {
        protected override void BindDependencies()
        {
            Container.Bind<UIStackNavigator>()
                .WithId(navigatorId)
                .AsCached()
                .WithArguments(screens, contianer)
                .NonLazy();
        }
    }
}