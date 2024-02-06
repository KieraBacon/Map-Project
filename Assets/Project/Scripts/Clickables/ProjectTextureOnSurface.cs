using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

[RequireComponent(typeof(BoxCollider))]
public class ProjectTextureOnSurface : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    private List<ClickableZone> _intersectingZones = new();
    private BoxCollider _boxCollider;
    [SerializeField] private Shader _shader;
    [SerializeField] private Color _color;
    private Dictionary<MeshRenderer, Material> _addedMaterials = new();
    private Bounds _boundsLastFrame;
    private static readonly int _boundsMin = Shader.PropertyToID("_Bounds_Min");
    private static readonly int _boundsMax = Shader.PropertyToID("_Bounds_Max");
    private static readonly int _texture = Shader.PropertyToID("_Texture");
    private static readonly int _color1 = Shader.PropertyToID("_Color");

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boundsLastFrame = _boxCollider.bounds;
    }

    private void Update()
    {
        Bounds currentBounds = _boxCollider.bounds;
        if (currentBounds != _boundsLastFrame)
        {
            _boundsLastFrame = _boxCollider.bounds;
            foreach (Material material in _addedMaterials.Values)
            {
                material.SetVector(_boundsMin, _boxCollider.bounds.min);
                material.SetVector(_boundsMax, _boxCollider.bounds.max);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ClickableZone zone = other.GetComponentInParent<ClickableZone>();
        if (zone == null) return;
        if (other.TryGetComponent(out MeshRenderer meshRenderer))
        {
            Material[] materials = new Material[meshRenderer.materials.Length + 1];
            meshRenderer.materials.CopyTo(materials, 0);
            Material material = new Material(_shader);
            material.SetTexture(_texture, _sprite.texture);
            material.SetVector(_boundsMin, _boxCollider.bounds.min);
            material.SetVector(_boundsMax, _boxCollider.bounds.max);
            material.SetColor(_color1, _color);
            materials[^1] = material;
            meshRenderer.materials = materials;
            _addedMaterials[meshRenderer] = material;
        }
        _intersectingZones.Add(zone);
        zone.OnPointerMoved += OnPointerMoved;
    }

    private void OnTriggerExit(Collider other)
    {
        ClickableZone zone = other.GetComponentInParent<ClickableZone>();
        if (zone == null) return;
        if (other.TryGetComponent(out MeshRenderer meshRenderer) && _addedMaterials.TryGetValue(meshRenderer, out Material material))
        {
            //Material[] materials = meshRenderer.materials.Except(new Material[]{material}).ToArray();
            meshRenderer.materials = new[] { meshRenderer.materials[0] };
            //meshRenderer.materials = materials;
            _addedMaterials.Remove(meshRenderer);
        }
        _intersectingZones.Remove(zone);
        zone.OnPointerMoved -= OnPointerMoved;
    }

    public void OnPointerMoved(PointerEventData eventData)
    {
        Vector3 hitPoint = eventData.pointerCurrentRaycast.worldPosition;
        Bounds bounds = _boxCollider.bounds;
        if (hitPoint.x < bounds.min.x || hitPoint.z < bounds.min.z || hitPoint.x > bounds.max.x || hitPoint.z > bounds.max.z) return;

        Vector3 zeroedBoundMax = (bounds.max - bounds.min);
        Vector3 zeroedHitPoint = (hitPoint - bounds.min);
        if (zeroedBoundMax.x == 0 || zeroedBoundMax.y == 0 || zeroedBoundMax.z == 0) return;

        Vector3 normalizedHitPoint = new(zeroedHitPoint.x / zeroedBoundMax.x, zeroedHitPoint.y / zeroedBoundMax.y, zeroedHitPoint.z / zeroedBoundMax.z);
        Vector2Int normalizedUVPosition = new Vector2Int((int)(normalizedHitPoint.x * _sprite.texture.width), (int)(normalizedHitPoint.z * _sprite.texture.height));
        float f = _sprite.texture.GetPixel(normalizedUVPosition.x, normalizedUVPosition.y).grayscale;
        Debug.DrawLine(hitPoint + Vector3.down * 0.1f, hitPoint, new Color(f, f, f, f), 100);
    }
}