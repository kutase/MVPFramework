namespace MVPFramework.Events
{
    public interface IEventSubscriber
    {
        void Unsubscribe();
        void OnDespawned();
    }
}