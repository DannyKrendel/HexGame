using Cysharp.Threading.Tasks;

namespace HexGame.UI
{
    public interface IScreen
    {
        UniTask Show();
        UniTask Hide();
    }
}