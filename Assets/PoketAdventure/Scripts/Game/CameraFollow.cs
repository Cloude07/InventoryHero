using CloudeDev.Game.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CloudeDev.Game
{
    public class CameraFollow : ITickable
    {
        private Camera _camera;
        private IPlayerView _view;
        private Vector3 _velocity = Vector3.zero;

        private float smoothTime = 0.125f;   
        private Vector3 offset = new Vector3(5, -1.5f, -10f); 

        [Inject]
        private void Construct(IPlayerView playerView)
        {
            _camera = Camera.main;
            _view = playerView;
        }

        public void Tick()
        {
            Vector3 targetPosition = _view.PlayerTransform.position + offset;
            _camera.transform.position = Vector3.SmoothDamp(
                _camera.transform.position,
                targetPosition,
                ref _velocity,
                smoothTime
            );
        }
    }
}