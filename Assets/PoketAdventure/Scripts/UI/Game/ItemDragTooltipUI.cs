using CloudeDev.Game.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.UI
{
    public class ItemDragTooltipUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _root;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _statsText;
        [SerializeField] private TMP_Text _comparisonText;

        [Header("Layout")]
        [SerializeField] private Vector2 _offset = new Vector2(24f, -24f);  // смещение от курсора
        [SerializeField] private float _padding = 8f;                         // чтобы не прилипало к краю

        private RectTransform _rect;
        private Canvas _canvas;

        private void Awake()
        {
            if (_root != null) _rect = _root.transform as RectTransform;
            _canvas = GetComponentInParent<Canvas>();
            if (_root != null) _root.SetActive(false);
        }

        public void Show(ItemConfig item, ItemConfig currentlyEquipped)
        {
            if (item == null || _root == null) return;

            _icon.sprite = item.icon;
            _titleText.SetText(item.nameItem);

            string stats = ItemDescriptionHelper.GetStatsLine(item);
            _statsText.SetText(stats);
            _statsText.gameObject.SetActive(!string.IsNullOrEmpty(stats));

            string cmp = ItemDescriptionHelper.GetComparisonLine(item, currentlyEquipped);
            _comparisonText.SetText(cmp);
            _comparisonText.gameObject.SetActive(!string.IsNullOrEmpty(cmp));

            _root.SetActive(true);
            UpdatePosition();
        }

        public void Hide()
        {
            if (_root != null) _root.SetActive(false);
        }

        private void Update()
        {
            if (_root != null && _root.activeSelf)
                UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (_rect == null || _canvas == null) return;

            RectTransform canvasRect = _canvas.transform as RectTransform;
            Camera cam = null;
            if (_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                cam = _canvas.worldCamera != null ? _canvas.worldCamera : Camera.main;

            // Позиция курсора в координатах Canvas
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, Input.mousePosition, cam, out var canvasPoint))
                return;

            // Размер tooltip (LayoutGroup ещё может не применить размер — берём текущий)
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
            Vector2 size = _rect.sizeDelta;

            Vector2 pos = canvasPoint + _offset;

            // Не выходить за правый край
            if (pos.x + size.x > canvasRect.rect.xMax - _padding)
                pos.x = canvasPoint.x - size.x - _offset.x;

            // Не выходить за нижний край
            if (pos.y - size.y < canvasRect.rect.yMin + _padding)
                pos.y = canvasPoint.y + size.y + Mathf.Abs(_offset.y);

            // Не выходить за верхний край
            if (pos.y > canvasRect.rect.yMax - _padding)
                pos.y = canvasRect.rect.yMax - _padding;

            _rect.localPosition = pos;
        }
    }
}