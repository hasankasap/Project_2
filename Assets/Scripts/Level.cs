using Game.BlockSystem;
using UnityEngine;

namespace Game
{
    public class Level : MonoBehaviour
    {
        public Transform Finish;
        public MovingBlock FirstBlock;

        public void SetFinishWidth(float width)
        {
            Vector3 scale = Finish.transform.localScale;
            scale.x = width;
            Finish.transform.localScale = scale;
            Renderer renderer = Finish.GetComponentInChildren<Renderer>();
            renderer.material.mainTextureScale = new Vector2 (scale.x, 1);
        }
    }
}