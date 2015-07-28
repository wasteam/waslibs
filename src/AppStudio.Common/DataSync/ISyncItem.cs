namespace AppStudio.Common.DataSync
{
    public interface ISyncItem<T>
    {
        void Sync(T other);
        bool NeedSync(T other);
    }
}
