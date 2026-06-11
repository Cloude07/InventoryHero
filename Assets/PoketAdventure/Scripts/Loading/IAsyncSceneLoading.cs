using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace CloudeDev.Boot
{
    public interface IAsyncSceneLoading
    {
        UniTask LoadAsync(string sceneName);
        UniTask UnloadAsync(string sceneName);
        void LoadingIsDone(bool value);
    }
}