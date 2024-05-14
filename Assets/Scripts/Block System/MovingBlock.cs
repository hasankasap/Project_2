using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BlockSystem
{
    public class MovingBlock : MonoBehaviour
    {
        public float Width, Length;
        public Vector3 Center;
        public Material BlockMat;
        public MovingBlock nextBlock;

        private Tween movementTween;
        [SerializeField] private GameObject blockModel;
        [SerializeField] private Renderer blockRenderer;
        [SerializeField] private BoxCollider jumpTrigger;

        public void Initialize(float targetWidth, float targetLenth)
        {
            Vector3 tmpScale = blockModel.transform.localScale;
            tmpScale.x = targetWidth;
            tmpScale.z = targetLenth;
            Width = targetWidth;
            Length = targetLenth;
            blockModel.transform.localScale = tmpScale;
            tmpScale.z = targetLenth * .1f;
            jumpTrigger.size = tmpScale;
            Vector3 colCenter = jumpTrigger.center;
            colCenter.z = targetLenth - .1f;
            Center = transform.position;
        }

        public void MoveX(float start, float end, float movementSpeed)
        {
            Vector3 oldPos = transform.position;
            oldPos.x = start;
            transform.position = oldPos;
            movementTween = transform.DOMoveX(end, movementSpeed).SetLoops(-1, LoopType.Yoyo).SetSpeedBased(true).SetEase(Ease.Linear);
        }
        public void ChangeColliderActive(bool status)
        {
            jumpTrigger.enabled = status;
        }
        public void StopMovement()
        {
            if (movementTween == null) return;
            movementTween.Kill();
            movementTween = null;
        }
        public GameObject GetBlockModel()
        {
            return blockModel;
        }
        public void SetWidth(float wantedWidth)
        {
            Width = wantedWidth;
            Vector3 tmpScale = blockModel.transform.localScale;
            tmpScale.x = wantedWidth;
            blockModel.transform.localScale = tmpScale;
        }
        public void SetModelColor(Material wantedColor)
        {
            if (blockRenderer == null)
            {
                blockRenderer = blockModel.GetComponentInChildren<Renderer>();
            }
            blockRenderer.material = wantedColor;
        }
    }
}