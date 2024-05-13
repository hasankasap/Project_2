using UnityEngine;
namespace Game.BlockSystem
{
    public class BlocksEdges
    {
        public Vector3 MainLeft, MainRight;
        public Vector3 PrevLeft, PrevRight;

        public BlocksEdges(MovingBlock current, MovingBlock prev)
        {
            prev.Center = prev.transform.position;
            PrevLeft = prev.Center - Vector3.right * (prev.Width / 2);
            PrevRight = prev.Center + Vector3.right * (prev.Width / 2);
            current.Center = current.transform.position;
            MainLeft = current.Center - Vector3.right * (current.Width / 2);
            MainRight = current.Center + Vector3.right * (current.Width / 2);
            current.Center = current.transform.position;
        }
    }
}