using Cysharp.Threading.Tasks;

namespace HexGame.Storage
{
    public interface IStorage<T>
    {
        UniTask Save(string key, T data);

        UniTask<T> Load(string key);
    }
}