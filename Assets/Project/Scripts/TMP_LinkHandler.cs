using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))] public class TMP_LinkHandler : MonoBehaviour, IPointerClickHandler
{
    public event Action<string> OnLinkClicked;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePosition = new (eventData.position.x, eventData.position.y, 0);
        if (TryGetIntersectingLink(mousePosition, out string result))
            OnLinkClicked?.Invoke(result);
    }

    private bool TryGetIntersectingLink(Vector3 position, out string result)
    {
        result = null;
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, position, null);
        if (linkIndex == -1) return false;

        TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[linkIndex];
        if (linkInfo.textComponent == null) return false;

        result = linkInfo.GetLinkText();
        return true;
    }
}