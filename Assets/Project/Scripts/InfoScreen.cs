using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    private static readonly ResourceObjectNamePair k_Resource = new("Info Screen");
    private static InfoScreen _instance;
    public static InfoScreen Main =>
        _instance != null || ResourceInstantiator.TryInstantiateResource(k_Resource, Canvas.Main, out _instance) ? _instance : null;
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private TMP_Text _bodyText;
    [SerializeField] private ScrollRect _scrollRect;
    private IInfoScreenAnimator _animator;

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

    private bool _isShowing;
    public bool IsShowing =>
        _isShowing;
    private IScreenData _currentData;
    public IScreenData CurrentData =>
        _currentData;

    public bool CurrentDataMatches(IScreenData data) =>
        _currentData == data;

    private void SetCurrentData(IScreenData value)
    {
        _currentData = value;
        BodyText = _currentData.BodyText;
        HeaderText = _currentData.HeaderText;
        _scrollRect.verticalNormalizedPosition = 1;
    }

    private void Awake()
    {
        _animator = GetComponent<IInfoScreenAnimator>();
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
        if (IsShowing && CurrentDataMatches(data)) return;
        if (!IsShowing)
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
        _isShowing = true;
        if (_animator != null)
            _animator.Show();
        else
            gameObject.SetActive(true);
    }

    public void Hide()
    {
        _isShowing = false;
        if (_animator != null)
            _animator.Hide();
        else
            gameObject.SetActive(false);
    }
}