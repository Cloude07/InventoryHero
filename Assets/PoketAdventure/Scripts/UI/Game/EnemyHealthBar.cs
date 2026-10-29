using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.Game.Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _root;          // что скрывать (Canvas)
        [SerializeField] private Gradient _colorGradient;   // цвет от HP: зелёный→жёлтый→красный
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _viewDuration = 2f;  // сколько держать видимым после удара

        private Transform _camera;
        private float _hideTimer;
        private bool _visible;

        private void Awake()
        {
            _camera = Camera.main != null ? Camera.main.transform : null;
        }
        public void ResetSilent(float current, float max)
        {
            if (_slider == null) return;

            max = Mathf.Max(1f, max);
            _slider.maxValue = max;
            _slider.value = current;

            if (_fillImage != null && _colorGradient != null)
                _fillImage.color = _colorGradient.Evaluate(current / max);

            if (_root != null) _root.SetActive(false);
            _visible = false;
            _hideTimer = 0f;
        }

        public void SetHealth(float current, float max)
        {
            if (_slider == null || _root == null) return;

            max = Mathf.Max(1f, max);
            _slider.maxValue = max;
            _slider.value = current;

            if (_fillImage != null && _colorGradient != null)
                _fillImage.color = _colorGradient.Evaluate(current / max);

            if (current <= 0f)
            {
                Hide();
                return;
            }


            _root.SetActive(true);
            _visible = true;


            _hideTimer = _viewDuration;   // обновляем таймер — бар виден ещё N секунд
        }

        public void Hide()
        {
            if (_root != null) _root.SetActive(false);
            _visible = false;
        }

        private void LateUpdate()
        {
            // 1. Бильборд — всегда лицом к камере
            if (_camera != null)
                transform.rotation = _camera.rotation;

            // 2. Автоскрытие, если HP не менялся N секунд
            if (!_visible) return;

            _hideTimer -= Time.deltaTime;
            if (_hideTimer <= 0f)
                Hide();
        }


    }
}