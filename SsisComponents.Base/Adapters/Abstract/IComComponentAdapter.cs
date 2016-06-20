namespace SsisComponents.Base.Adapters.Abstract
{
    public interface IComComponentAdapter
    {
        void PreExecute();
        void ReinitializeMetaData();
        void ProvideComponentProperties();
        void OnInputPathAttached(int v);
    }
}