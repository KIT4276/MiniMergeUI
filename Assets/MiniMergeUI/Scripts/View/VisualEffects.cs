using DG.Tweening;
using UnityEngine;

namespace MiniMergeUI.View
{
    public class VisualEffects : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [Header("General Pop Parameters")]
        [SerializeField] private float _upTime = 0.08f;
        [SerializeField] private float _downTime = 0.1f;
        [Header("Merge Pop Parameters")]
        [SerializeField] private float _scale = 1.25f;
        [Header("End Drag Pop Parameters")]
        [SerializeField] private float _scale1 = 0.8f;

        private Vector3 _baseScale;

        private void Awake() => 
            _baseScale = _rectTransform.localScale;

        public void PlayMergePop() => 
            PlayPop(_scale, _upTime, _downTime);

        public void PlayEndDragPop() => 
            PlayPop(_scale1, _upTime, _downTime);

        private void PlayPop(float scale, float upTime, float downTime)
        {
            _rectTransform.DOKill();

            _rectTransform.localScale = _baseScale;
            _rectTransform.DOScale(_baseScale * scale, upTime)
              .SetEase(Ease.OutBack)
              .OnComplete(() =>
              {
                  _rectTransform.DOScale(_baseScale, downTime).SetEase(Ease.InOutSine);
              });
        }
    }
}
