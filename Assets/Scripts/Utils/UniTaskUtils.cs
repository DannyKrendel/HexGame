using System.Threading;

namespace HexGame.Utils
{
    public static class UniTaskUtils
    {
        public static CancellationToken RefreshToken(ref CancellationTokenSource tokenSource) {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }
    }
}