using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

namespace Project.Scripts.Clickables
{
    [RequireComponent(typeof(BoxCollider))]
    public class ProjectTextureOnSurface : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        private List<ClickableZone> _intersectingZones = new();
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            ClickableZone zone = other.GetComponentInParent<ClickableZone>();
            if (zone == null) return;
            _intersectingZones.Add(zone);
            zone.OnPointerMoved += OnPointerMoved;
        }

        private void OnTriggerExit(Collider other)
        {
            ClickableZone zone = other.GetComponentInParent<ClickableZone>();
            if (zone == null) return;
            _intersectingZones.Remove(zone);
            zone.OnPointerMoved -= OnPointerMoved;
        }

        public void OnPointerMoved(PointerEventData eventData)
        {
            Vector3 hitPoint = eventData.pointerCurrentRaycast.worldPosition;
            Bounds bounds = _boxCollider.bounds;
            if (hitPoint.x < bounds.min.x || hitPoint.z < bounds.min.z || hitPoint.x > bounds.max.x || hitPoint.z > bounds.max.z)
                return;

            Vector3 zeroedBoundMax = (bounds.max - bounds.min);
            Vector3 zeroedHitPoint = (hitPoint - bounds.min);
            if (zeroedBoundMax.x == 0 || zeroedBoundMax.y == 0 || zeroedBoundMax.z == 0)
                return;

            Vector3 normalizedHitPoint = new (zeroedHitPoint.x / zeroedBoundMax.x, zeroedHitPoint.y / zeroedBoundMax.y, zeroedHitPoint.z / zeroedBoundMax.z);
            Vector2Int normalizedUVPosition = new Vector2Int((int)(normalizedHitPoint.x * _sprite.texture.width), (int)(normalizedHitPoint.z * _sprite.texture.height)); 
            Color c = _sprite.texture.GetPixel(normalizedUVPosition.x, normalizedUVPosition.y);
            float f = c.grayscale;
            Debug.Log($"normalizedHitPoint: {normalizedHitPoint}. normalizedUVPosition: {normalizedUVPosition}. Color: {c}.");
            Debug.DrawLine(hitPoint + Vector3.down * 0.1f, hitPoint, new Color(f, f, f, f), 100);
        }
    }
}