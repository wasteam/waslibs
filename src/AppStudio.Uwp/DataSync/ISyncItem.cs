namespace AppStudio.Uwp.DataSync
{
    public interface ISyncItem<T>
    {
        void Sync(T other);
        bool NeedSync(T other);
    }
}
