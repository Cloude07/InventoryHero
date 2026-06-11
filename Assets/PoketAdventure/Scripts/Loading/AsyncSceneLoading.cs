using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


namespace CloudeDev.Boot.SceneLoading
{

    public class AsyncSceneLoading : IAsyncSceneLoading
    {
        private CancellationTokenSource _cts;
        private readonly Dictionary<string, SceneInstance> _loadedScence =
     new Dictionary<string, SceneInstance>();

        public async UniTask LoadAsync(string sceneName)
        {
            _cts = new CancellationTokenSource();
            LoadingIsDone(false);
            var loadedScene = await Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive)
                .WithCancellation(_cts.Token);
            SceneManager.SetActiveScene(loadedScene.Scene);
            if (_loadedScence.ContainsKey(sceneName) == false)
                _loadedScence.Add(sceneName, loadedScene);
            _cts.Cancel();
        }

        public void LoadingIsDone(bool value)
        {
          // при выполнение загрузки показать загрузку
        }

        public async UniTask UnloadAsync(string sceneName)
        {
            _cts = new CancellationTokenSource();
            var sceneInstance = _loadedScence[sceneName];
            await Addressables.UnloadSceneAsync(sceneInstance)
                .WithCancellation(_cts.Token).AsUniTask();
            _loadedScence.Remove(sceneName);
            _cts.Cancel();
        }


    }
   
}
