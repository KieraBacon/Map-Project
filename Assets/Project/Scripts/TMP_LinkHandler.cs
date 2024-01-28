using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_LinkHandler : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _text;
        private Camera _camera;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _camera = Camera.main;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, mousePosition, null);
            Debug.Log(linkIndex);

            if (linkIndex == -1) return;
            TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[linkIndex];
            if (linkInfo.textComponent == null) return;
            Debug.Log($"yeah, {linkInfo.GetLinkText()}");
        }
    }
}