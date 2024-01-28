using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(InfoScreen))]
public class InfoScreenTweener : MonoBehaviour, IWindowAnimator
{
    [Serializable] private struct TweenSettings
    {
        public float SizeChangeStartTime;
        public float SizeChangeDuration;
        public float MainFadeStartTime;
        public float MainFadeDuration;
        public float TextFadeStartTime;
        public float TextFadeDuration;
        public float TotalDuration =>
            Mathf.Max(
                (SizeChangeStartTime + SizeChangeDuration),
                (MainFadeStartTime + MainFadeDuration),
                (TextFadeStartTime + TextFadeDuration));
        public float AcceptRaycastsAfter;
        public float IgnoreRaycastsAfter;
        public float AcceptInteractionsAfter;
        public float IgnoreInteractionsAfter;
        public Ease Ease;
    }
    
    [Header("Component References")]
    [SerializeField] private CanvasGroup _labelCanvasGroup;
    [SerializeField] private CanvasGroup _bodyCanvasGroup;
    private CanvasGroup _mainCanvasGroup;
    private RectTransform _rectTransform;
    
    [Header("Show/Hide Tween Settings")]
    private Sequence _showHideTween;
    private float _targetAlpha;
    [SerializeField] private TweenSettings _showTweenSettings;
    [SerializeField] private TweenSettings _hideTweenSettings;
    private Vector2 _shownSize;
    [SerializeField] private Vector2 _hiddenSize;
    private float _shownAlpha;
    [SerializeField] private float _hiddenAlpha;
    
    [Header("Text Tween Settings")]
    private Sequence _textTween;
    [SerializeField] private float _textChangeDuration;

    public void Init()
    {
        _mainCanvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _shownAlpha = _mainCanvasGroup.alpha;
        _shownSize = _rectTransform.sizeDelta;
        Tween(new TweenSettings()
        {
            MainFadeStartTime = 0,
            MainFadeDuration = 0,
            TextFadeStartTime = 0,
            TextFadeDuration = 0,
            SizeChangeStartTime = 0,
            SizeChangeDuration = 0,
        }, _hiddenAlpha, _hiddenSize);
    }

    public void Show()
    {
        Tween(_showTweenSettings, _shownAlpha, _shownSize);
    }

    public void Hide()
    {
        Tween(_hideTweenSettings, _hiddenAlpha, _hiddenSize);
    }

    public void Crossfade(Action transitionCallback)
    {
        FadeText(_textChangeDuration, transitionCallback);
    }

    private bool CheckDuration(TweenSettings settings, float alpha, Vector2 size)
    {
        if (settings.TotalDuration > 0) return true;
        _showHideTween?.Kill();
        _rectTransform.sizeDelta = size;
        _mainCanvasGroup.alpha = alpha;
        _bodyCanvasGroup.alpha = alpha;
        bool interactableAfter = settings.AcceptInteractionsAfter >= 0 && settings.AcceptInteractionsAfter > settings.IgnoreInteractionsAfter;
        _mainCanvasGroup.interactable = interactableAfter;
        _bodyCanvasGroup.interactable = interactableAfter;
        bool raycastableAfter = settings.AcceptRaycastsAfter >= 0 && settings.AcceptRaycastsAfter > settings.IgnoreRaycastsAfter;
        _mainCanvasGroup.blocksRaycasts = raycastableAfter;
        _bodyCanvasGroup.blocksRaycasts = raycastableAfter;
        return false;
    }

    private bool CheckTarget(TweenSettings settings, float alpha, Vector2 size)
    {
        // If there's no active tween, good to continue.
        if (_showHideTween == null) return true;
        if (!_showHideTween.active) return true;

        // If the active tween is going to a different place, good to continue.
        double tolerance = 0.01f;
        if (Math.Abs(_targetAlpha - alpha) > tolerance) return true;

        // If the active tween has more time remaining than the new one has in total, good to continue.
        float remainingDuration = _showHideTween.Duration() * (1 - _showHideTween.ElapsedPercentage());
        if (settings.TotalDuration < remainingDuration) return true;

        // Yeah you should probably stop here.
        return false;
    }

    private bool CheckCurrent(TweenSettings settings, float alpha, Vector2 size)
    {
        double tolerance = 0.01f;
        if (_rectTransform.sizeDelta != size ||
            !(Math.Abs(_mainCanvasGroup.alpha - _shownAlpha) < tolerance) ||
            !(Math.Abs(_bodyCanvasGroup.alpha - _shownAlpha) < tolerance))
            return true;

        bool interactableAfter = settings.AcceptInteractionsAfter >= 0 && settings.AcceptInteractionsAfter > settings.IgnoreInteractionsAfter;
        _mainCanvasGroup.interactable = interactableAfter;
        _bodyCanvasGroup.interactable = interactableAfter;
        bool raycastableAfter = settings.AcceptRaycastsAfter >= 0 && settings.AcceptRaycastsAfter > settings.IgnoreRaycastsAfter;
        _mainCanvasGroup.blocksRaycasts = raycastableAfter;
        _bodyCanvasGroup.blocksRaycasts = raycastableAfter;
        return false;
    }

    private void Tween(TweenSettings settings, float alpha, Vector2 size)
    {
        if (!CheckDuration(settings, alpha, size)) return;
        if (!CheckTarget(settings, alpha, size)) return;
        if (!CheckCurrent(settings, alpha, size)) return;

        void SetInteractable(bool value)
        {
            _mainCanvasGroup.interactable = value;
            _bodyCanvasGroup.interactable = value;
            _labelCanvasGroup.interactable = value;
        }
        
        void SetRaycastable(bool value)
        {
            _mainCanvasGroup.blocksRaycasts = value;
            _bodyCanvasGroup.blocksRaycasts = value;
            _labelCanvasGroup.blocksRaycasts = value;
        }
        
        _showHideTween?.Kill();
        _targetAlpha = alpha;
        _showHideTween = DOTween.Sequence(this).SetEase(settings.Ease)
            .Insert(settings.SizeChangeStartTime, DOTween.To(() => _rectTransform.sizeDelta, value => _rectTransform.sizeDelta = value, size, settings.SizeChangeDuration))
            .Insert(settings.MainFadeStartTime, DOTween.To(() => _mainCanvasGroup.alpha, value => _mainCanvasGroup.alpha = value, alpha, settings.MainFadeDuration))
            .Insert(settings.TextFadeStartTime, DOTween.To(() => _bodyCanvasGroup.alpha, value => _bodyCanvasGroup.alpha = value, alpha, settings.TextFadeDuration));
        if (settings.IgnoreInteractionsAfter >= 0)
            _showHideTween.InsertCallback(settings.IgnoreInteractionsAfter, () => SetInteractable(false));
        if (settings.AcceptInteractionsAfter >= 0)
            _showHideTween.InsertCallback(settings.AcceptInteractionsAfter, () => SetInteractable(true));
        if (settings.IgnoreRaycastsAfter >= 0)
            _showHideTween.InsertCallback(settings.IgnoreRaycastsAfter, () => SetRaycastable(false));
        if (settings.AcceptRaycastsAfter >= 0)
            _showHideTween.InsertCallback(settings.AcceptRaycastsAfter, () => SetRaycastable(true));
        
        DOTween.Play(_showHideTween);
    }

    private void FadeText(float duration, Action transitionCallback)
    {
        float half = duration * 0.5f;
        _textTween?.Kill();
        _textTween = DOTween.Sequence(this)
            .Append(DOTween.To(() => _bodyCanvasGroup.alpha, value => _bodyCanvasGroup.alpha = value, 0, half))
            .Join(DOTween.To(() => _labelCanvasGroup.alpha, value => _labelCanvasGroup.alpha = value, 0, half))
            .AppendCallback(() => transitionCallback?.Invoke())
            .Append(DOTween.To(() => _bodyCanvasGroup.alpha, value => _bodyCanvasGroup.alpha = value, 1, half))
            .Join(DOTween.To(() => _labelCanvasGroup.alpha, value => _labelCanvasGroup.alpha = value, 1, half));
        DOTween.Play(_textTween);
    }
}