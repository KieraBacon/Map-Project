using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    [Serializable] private struct TweenSettings
    {
        public float SizeChangeStartTime;
        public float SizeChangeDuration;
        public float MainFadeStartTime;
        public float MainFadeDuration;
        public float TextFadeStartTime;
        public float TextFadeDuration;
        public float TotalDuration => Mathf.Max(
                (SizeChangeStartTime + SizeChangeDuration),
                (MainFadeStartTime + MainFadeDuration),
                (TextFadeStartTime + TextFadeDuration));
        public bool InteractableAfter;
        public Ease Ease;
    }
    private static readonly ResourceObjectNamePair k_Resource = new("Info Screen");
    private static InfoScreen _instance;
    public static InfoScreen Main =>
        _instance != null || ResourceInstantiator.TryInstantiateResource(k_Resource, Canvas.Main, out _instance) ? _instance : null;

    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private TMP_Text _bodyText;
    [SerializeField] private CanvasGroup _labelCanvasGroup;
    [SerializeField] private CanvasGroup _bodyCanvasGroup;
    [SerializeField] private TweenSettings _showTweenSettings;
    [SerializeField] private TweenSettings _hideTweenSettings;
    [SerializeField] private Vector2 _hiddenSize;
    [SerializeField] private float _hiddenAlpha;
    [SerializeField] private float _textChangeDuration;
    [SerializeField] private ScrollRect _scrollRect;
    private float _targetAlpha;
    private Vector2 _shownSize;
    private float _shownAlpha;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Sequence _showTween;
    private Sequence _textTween;
    private IScreenData _currentData;

    private IScreenData CurrentData
    {
        get => _currentData;
        set
        {
            _currentData = value;
            BodyText = _currentData.BodyText;
            HeaderText = _currentData.HeaderText;
            _scrollRect.verticalNormalizedPosition = 1;
        }
    }

    private bool _isShowing;
    public bool IsShowing => _isShowing;
    public bool IsShowingScreenData(IScreenData data) => _currentData == data;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _shownAlpha = _canvasGroup.alpha;
        _shownSize = _rectTransform.sizeDelta;
        Tween(new TweenSettings()
        {
            MainFadeStartTime = 0, MainFadeDuration = 0,
            TextFadeStartTime = 0, TextFadeDuration = 0,
            SizeChangeStartTime = 0, SizeChangeDuration = 0,
        }, _hiddenAlpha, _hiddenSize);
    }

    public string HeaderText
    {
        get => _headerText.text;
        set => _headerText.text = value;
    }
    
    public string BodyText
    {
        get => _bodyText.text;
        set => _bodyText.text = value;
    }

    public void Toggle()
    {
        if (IsShowing)
            Hide();
        else
            Show();
    }

    public void Show(IScreenData data)
    {
        if (IsShowing && IsShowingScreenData(data)) return;
        if (!IsShowing)
        {
            CurrentData = data;
            Show();
        }
        else
        {
            FadeText(_currentData, data, _textChangeDuration);
        }
    }

    public void Show()
    {
        _isShowing = true;
        Tween(_showTweenSettings, _shownAlpha, _shownSize);
    }

    public void Hide()
    {
        _isShowing = false;
        Tween(_hideTweenSettings, _hiddenAlpha, _hiddenSize);
    }

    private bool CheckDuration(TweenSettings settings, float alpha, Vector2 size)
    {
        if (settings.TotalDuration > 0) return true;
        _showTween?.Kill();
        _rectTransform.sizeDelta = size;
        _canvasGroup.alpha = alpha;
        _bodyCanvasGroup.alpha = alpha;
        _canvasGroup.interactable = settings.InteractableAfter;
        _bodyCanvasGroup.interactable = settings.InteractableAfter;
        return false;
    }

    private bool CheckTarget(TweenSettings settings, float alpha, Vector2 size)
    {
        // If there's no active tween, good to continue.
        if (_showTween == null) return true;
        if (!_showTween.active) return true;

        // If the active tween is going to a different place, good to continue.
        double tolerance = 0.01f;
        if (Math.Abs(_targetAlpha - alpha) > tolerance) return true;

        // If the active tween has more time remaining than the new one has in total, good to continue.
        float remainingDuration = _showTween.Duration() * (1 - _showTween.ElapsedPercentage());
        if (settings.TotalDuration < remainingDuration) return true;

        // Yeah you should probably stop here.
        return false;
    }

    private bool CheckCurrent(TweenSettings settings, float alpha, Vector2 size)
    {
        double tolerance = 0.01f;
        if (_rectTransform.sizeDelta != size ||
            !(Math.Abs(_canvasGroup.alpha - _shownAlpha) < tolerance) ||
            !(Math.Abs(_bodyCanvasGroup.alpha - _shownAlpha) < tolerance))
            return true;
        
        _canvasGroup.interactable = settings.InteractableAfter;
        _bodyCanvasGroup.interactable = settings.InteractableAfter;
        return false;
    }
    
    private void Tween(TweenSettings settings, float alpha, Vector2 size)
    {
        if (!CheckDuration(settings,alpha,size)) return;
        if (!CheckTarget(settings,alpha,size)) return;
        if (!CheckCurrent(settings,alpha,size)) return;

        _showTween?.Kill();
        _targetAlpha = alpha;
        _canvasGroup.blocksRaycasts = settings.InteractableAfter;
        _canvasGroup.interactable = false;
        _bodyCanvasGroup.interactable = false;
        _showTween = DOTween.Sequence(this).SetEase(settings.Ease)
            .Insert(settings.SizeChangeStartTime, DOTween.To(() => _rectTransform.sizeDelta, value => _rectTransform.sizeDelta = value, size, settings.SizeChangeDuration))
            .Insert(settings.MainFadeStartTime, DOTween.To(() => _canvasGroup.alpha, value => _canvasGroup.alpha = value, alpha, settings.MainFadeDuration))
            .Insert(settings.TextFadeStartTime, DOTween.To(() => _bodyCanvasGroup.alpha, value => _bodyCanvasGroup.alpha = value, alpha, settings.TextFadeDuration))
            .AppendCallback(() =>
            {
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = settings.InteractableAfter;
                _bodyCanvasGroup.interactable = settings.InteractableAfter;
            });
        DOTween.Play(_showTween);
    }

    private void FadeText(IScreenData first, IScreenData second, float duration)
    {
        float half = duration * 0.5f;
        _textTween?.Kill();
        _textTween = DOTween.Sequence(this)
            .Append(DOTween.To(() => _bodyCanvasGroup.alpha, value => _bodyCanvasGroup.alpha = value, 0, half))
            .Join(DOTween.To(() => _labelCanvasGroup.alpha, value => _labelCanvasGroup.alpha = value, 0, half))
            .AppendCallback(() =>
            {
                CurrentData = second;
            })
            .Append(DOTween.To(() => _bodyCanvasGroup.alpha, value => _bodyCanvasGroup.alpha = value, 1, half))
            .Join(DOTween.To(() => _labelCanvasGroup.alpha, value => _labelCanvasGroup.alpha = value, 1, half));
        DOTween.Play(_textTween);
    }
}