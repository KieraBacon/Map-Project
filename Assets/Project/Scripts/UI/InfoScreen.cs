using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour, IWindow
{
    private static readonly ResourceObjectNamePair k_Resource = new("Info Screen");
    private static InfoScreen _instance;
    public static InfoScreen Main =>
        _instance != null || ResourceInstantiator.TryInstantiateResource(k_Resource, Canvas.Main, out _instance) ? _instance : null;
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private TMP_Text _bodyText;
    [SerializeField] private ScrollRect _scrollRect;
    private IWindowAnimator _animator;

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

    private bool _isVisible;
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (value)
                Show();
            else
                Hide();
        }
    }
    public void Toggle() =>
        IsVisible = !IsVisible;

    private IDescribable _currentData;
    public IDescribable CurrentData =>
        _currentData;

    public bool CurrentDataMatches(IDescribable data) =>
        _currentData == data;

    private void SetCurrentData(IDescribable value)
    {
        _currentData = value;
        HeaderText = _currentData.Name;
        BodyText = _currentData.Description;
        _scrollRect.verticalNormalizedPosition = 1;
    }

    private void Awake()
    {
        _animator = GetComponent<IWindowAnimator>();
        _animator.Init();
    }

    public void Show(IDescribable data)
    {
        if (IsVisible && CurrentDataMatches(data)) return;
        if (!IsVisible)
        {
            SetCurrentData(data);
            Show();
        }
        else
        {
            if (_animator != null)
                _animator.Crossfade(() => SetCurrentData(data));
            else
                SetCurrentData(data);
        }
    }

    public void Show()
    {
        _isVisible = true;
        if (_animator != null)
            _animator.Show();
        else
            gameObject.SetActive(true);
    }

    public void Hide()
    {
        _isVisible = false;
        if (_animator != null)
            _animator.Hide();
        else
            gameObject.SetActive(false);
    }
}