namespace DFsm.Infrastructure
{
    public interface IExtensionManager
    {
        void AddExtension(object extension);
        TExtension GetExtension<TExtension>() where TExtension : class;
    }
}
