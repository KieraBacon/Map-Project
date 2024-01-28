using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InfoScreen))]
public class InfoScreenLinkHandler : MonoBehaviour
{
    [SerializeField] private TMP_LinkHandler _linkHandler;
    private InfoScreen _infoScreen;

    private void Awake()
    {
        _infoScreen = GetComponent<InfoScreen>();
    }

    private void OnEnable()
    {
        _linkHandler.OnLinkClicked += OnLinkClicked;
    }

    private void OnDisable()
    {
        _linkHandler.OnLinkClicked -= OnLinkClicked;
    }

    private void OnLinkClicked(string link)
    {
        IScreenData linkData = _infoScreen.CurrentData.Links.FirstOrDefault(x => x.HeaderText == link);
        if (linkData != null)
        {
            _infoScreen.Show(linkData);
        }
    }
}