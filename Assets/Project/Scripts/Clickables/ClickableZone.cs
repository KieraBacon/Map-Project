using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerMoveHandler
{
    public event Action<PointerEventData> OnPointerMoved;
    [SerializeField] private Outline _outline;

    private void Awake()
    {
        _outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        InfoScreen infoScreen = InfoScreen.Main;
        
        string path = gameObject.name;
        if (LinksManager.Main.TryGetLinkAtPath(path, out ILinkable linkable) && 
            linkable is IDescribable describable)
        {
            if (infoScreen.CurrentDataMatches(describable))
                infoScreen.Toggle();
            else
                infoScreen.Show(describable);
        }
        else
        {
            Debug.LogError($"Unable to follow link: {path}.");
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        OnPointerMoved?.Invoke(eventData);
    }
}