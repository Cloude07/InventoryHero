using CloudeDev.Game.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CloudeDev.UI
{
    public class BackgroundScroll : MonoBehaviour
    {
        [SerializeField] private float _scrollSpeed = 0.007f;
        [SerializeField] private Vector2Int _direction;
        private RawImage _backgroundScrolImage;
        private Vector2 _uvRectSize;
        private IPlayerView _playerView;

        [Inject]
        private void Construct(IPlayerView playerView)
        {
            _playerView = playerView;
            _backgroundScrolImage = GetComponent<RawImage>();
            _uvRectSize = _backgroundScrolImage.uvRect.size;
        }

        private void Start()
        {
            Scroll().Forget();
        }


        private async UniTask Scroll()
        {
            while (destroyCancellationToken.IsCancellationRequested == false)
            {
                if (_playerView.isRunn)
                {
                    _backgroundScrolImage.uvRect = new Rect(
                        _backgroundScrolImage.uvRect.position +
                        new Vector2(_direction.x * _scrollSpeed, _direction.y * _scrollSpeed)
                        * Time.deltaTime, _uvRectSize);
                }
                await UniTask.Yield(PlayerLoopTiming.Update, destroyCancellationToken);
            }
        }
    }
}

