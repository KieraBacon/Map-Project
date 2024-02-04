using UnityEngine;

namespace Project.Scripts.Clickables
{
    public class ProjectTextureOnSurface : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _scale;
        private Vector2Int pos;

        public void Update()
        {
            pos = new Vector2Int(Random.Range(0, _sprite.texture.width), Random.Range(0, _sprite.texture.height));
            Vector3 vectorPos = new Vector3(pos.x, 1, pos.y) * _scale;
            Color c = _sprite.texture.GetPixel(pos.x, pos.y);
            Debug.DrawRay(vectorPos, Vector3.up * 0.1f, c, 100);
        }
    }
}