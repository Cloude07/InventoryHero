using CloudeDev.Boot.SaveSystema;
using UnityEngine;
using VContainer.Unity;

namespace CloudeDev.Boot
{
    public class BootEntryPoint : IInitializable
    {
        private IAsyncSceneLoading _sceneLoading;
        private SaveLoaderManager _saveProgress;

        public BootEntryPoint(IAsyncSceneLoading asyncScene, SaveLoaderManager saveProgress)
        {
            _saveProgress = saveProgress;
            _sceneLoading = asyncScene;
        }

        public async void Initialize()
        {
            Application.targetFrameRate = 60;
           // _saveProgress.LoadData();

            await _sceneLoading.LoadAsync(loadedScene.MENU);
        }
    }
}
