using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Outline _outline;
    [SerializeField] private ZoneData _data;

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

        if (infoScreen.CurrentDataMatches(_data))
            infoScreen.Toggle();
        else
            infoScreen.Show(_data);
    }
}