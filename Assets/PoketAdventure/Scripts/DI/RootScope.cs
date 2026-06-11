using CloudeDev.Audio;
using CloudeDev.Boot;
using CloudeDev.UI.Boot;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CloudeDev.DI
{
    public class RootScope : LifetimeScope
    {
        [SerializeField] private LoadingView _loadingView;
        [SerializeField] private AudioManager _audioManager;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BootEntryPoint>();
            builder.RegisterInstance(_audioManager);
            builder.RegisterInstance(_loadingView);
        }
    }
}
