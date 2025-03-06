using DITools;
using MVPFramework.Presenter;

namespace MVPFramework.Model
{
    public abstract class ModelBase<TData> : IModel<TData>, IContainerConstructable
    {
        private bool isModelUpdated;
        protected IScreenParams OpenParams;

        public virtual TData Data { get; protected set; }

        protected abstract void InitData();

        protected abstract TData CreateData();

        public void Activate(IScreenParams openParams)
        {
            OpenParams = openParams;
            Data ??= CreateData();
            InitData();
        }

        public void OnShow()
        {
            
        }

        public virtual void Deactivate()
        {
        }
    }
}