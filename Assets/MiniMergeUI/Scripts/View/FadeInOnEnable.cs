using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMergeUI.View
{
    public class FadeInOnEnable : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _duration = 0.35f;
        [SerializeField] private float _delay = 0f;

        private Tween _tween;
        private Color _imageStart;
        private Color _textStart;

        private void Awake()
        {
            if (_image != null) _imageStart = _image.color;
            if (_text != null) _textStart = _text.color;
        }

        private void OnEnable()
        {
            _rectTransform.SetAsLastSibling();

            if (_image != null)
            {
                var c = _imageStart; c.a = 1f;
                _image.color = c;
            }

            if (_text != null)
            {
                var c = _textStart; c.a = 1f;
                _text.color = c;
            }

            _tween?.Kill();

            float startTime = _delay;

            if (_image != null)
                _tween = _image.DOFade(0f, _duration).SetDelay(startTime);

            if (_text != null)
            {
                var t = _text.DOFade(0f, _duration).SetDelay(startTime);
                _tween = _tween == null ? t : DOTween.Sequence().Join(_tween).Join(t);
            }

            if (_tween != null)
                _tween.OnComplete(() => gameObject.SetActive(false));
            else
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _tween?.Kill();
            _tween = null;
        }
    }
}
