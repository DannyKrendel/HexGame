using Cysharp.Threading.Tasks;

namespace HexGame.Storage
{
    public interface IStorage<T>
    {
        UniTask Save(string key, T value);

        UniTask<T> Load(string key);
    }
}