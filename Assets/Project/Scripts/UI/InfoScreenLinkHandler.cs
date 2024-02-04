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

    private void OnLinkClicked(string path)
    {
        if (LinksManager.Main.TryGetLinkAtPath(path, out ILinkable linkable) && 
            linkable is IDescribable describable)
        {
            _infoScreen.Show(describable);
        }
        else
        {
            Debug.LogError($"Unable to follow link: {path}.");
        }
    }
}