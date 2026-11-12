using System.Collections;
using System.Collections.Generic;
using CloudeDev.Game.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.UI
{
    public class ItemPickupPopupUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _root;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _statsText;
        [SerializeField] private TMP_Text _amountText;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInTime = 0.15f;
        [SerializeField] private float _holdTime = 2f;
        [SerializeField] private float _fadeOutTime = 0.3f;
        [SerializeField] private float _verticalOffset = 80f;

        private RectTransform _rect;
        [SerializeField] private Vector2 _basePosition;
        [SerializeField] private Vector2 _itemPosition;
        private readonly Queue<(ItemConfig item, int amount)> _queue = new();
        private bool _isShowing;

        private void Awake()
        {
            _rect = _root.transform as RectTransform;
            if (_rect != null) _basePosition = _rect.anchoredPosition;
            if (_root != null) _root.SetActive(false);
        }

        public void ShowInventoryItem(ItemConfig item, int amount)
        {
            if (item == null || _root == null) return;
            _queue.Enqueue((item, amount));
            if (!_isShowing) StartCoroutine(ProcessQueue());
        }



        private IEnumerator ProcessQueue()
        {
            _isShowing = true;
            while (_queue.Count > 0)
            {
                var (item, amount) = _queue.Dequeue();
                yield return ShowSingle(item, amount);
            }
            _isShowing = false;
        }

        private IEnumerator ShowSingle(ItemConfig item, int amount)
        {
            _icon.sprite = item.icon;
            _titleText.SetText(item.nameItem);
            _statsText.SetText(ItemDescriptionHelper.GetStatsLine(item));
            _amountText.SetText(amount > 1 ? $"x{amount}" : string.Empty);

            _root.SetActive(true);
            _canvasGroup.alpha = 0f;
            _rect.anchoredPosition = _basePosition + Vector2.down * _verticalOffset;

            // Fade in + move in
            float elapsed = 0f;
            while (elapsed < _fadeInTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _fadeInTime);
                _canvasGroup.alpha = t;
                _rect.anchoredPosition = Vector2.Lerp(
                    _basePosition + Vector2.down * _verticalOffset,
                    _basePosition, t);
                yield return null;
            }
            _canvasGroup.alpha = 1f;
            _rect.anchoredPosition = _basePosition;

            yield return new WaitForSeconds(_holdTime);

            // Fade out + move up
            elapsed = 0f;
            while (elapsed < _fadeOutTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _fadeOutTime);
                _canvasGroup.alpha = 1f - t;
                _rect.anchoredPosition = _basePosition + Vector2.up * (_verticalOffset * t);
                yield return null;
            }

            _root.SetActive(false);
            yield return new WaitForSeconds(0.1f);   // небольшая пауза между предметами
        }
    }
}