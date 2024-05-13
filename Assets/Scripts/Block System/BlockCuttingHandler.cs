using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BlockSystem
{
    public class BlockCuttingHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.CUT_BLOCK, OnCut);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.CUT_BLOCK, OnCut);
        }
        private void OnCut(object[] obj)
        {
            MovingBlock movingBlock = (MovingBlock)obj[0];
            BlocksEdges blockCuttingInfo = (BlocksEdges)obj[1];
            if (movingBlock == null || blockCuttingInfo == null) return;

            CutBlock(movingBlock, blockCuttingInfo);
        }

        private void CutBlock(MovingBlock movingBlock, BlocksEdges blockCuttingInfo)
        {
            Vector3 newMainLeft = blockCuttingInfo.MainLeft.x <= blockCuttingInfo.PrevLeft.x ? blockCuttingInfo.PrevLeft : blockCuttingInfo.MainLeft;
            Vector3 newMainRight = blockCuttingInfo.MainRight.x >= blockCuttingInfo.PrevRight.x ? blockCuttingInfo.PrevRight : blockCuttingInfo.MainRight;
            float newMainWidth;
            Vector3 tmpCenter;
            newMainWidth = Mathf.Abs(newMainRight.x - newMainLeft.x);
            tmpCenter = newMainLeft;
            tmpCenter.x += newMainWidth / 2f;
            tmpCenter.z = blockCuttingInfo.MainLeft.z;
            movingBlock.transform.position = tmpCenter;
            movingBlock.SetWidth(newMainWidth);
            movingBlock.Center = tmpCenter;
            float cuttedWidth = 0;
            if (newMainLeft == blockCuttingInfo.PrevLeft)
            {
                // block on left
                cuttedWidth = blockCuttingInfo.PrevLeft.x - blockCuttingInfo.MainLeft.x;
                tmpCenter = blockCuttingInfo.MainLeft;
                tmpCenter.x += cuttedWidth / 2f;
            }
            else if (newMainRight == blockCuttingInfo.PrevRight)
            {
                // block on right
                cuttedWidth = blockCuttingInfo.MainRight.x - blockCuttingInfo.PrevRight.x;
                tmpCenter = blockCuttingInfo.PrevRight;
                tmpCenter.x += cuttedWidth / 2f;
            }
            tmpCenter.z = blockCuttingInfo.MainLeft.z;
            CreateCuttedPart(tmpCenter, cuttedWidth, movingBlock.BlockMat);
        }
        private void CreateCuttedPart(Vector3 center, float wantedWidth, Material wantedColor, float delay = 0)
        {
            MovingBlock movingBlock = BlockPool.Instance.GetPooledBlock(wantedColor);
            movingBlock.transform.position = center;
            movingBlock.Center = center;
            movingBlock.SetWidth(wantedWidth);
            movingBlock.SetModelColor(wantedColor);
            movingBlock.transform.DOMoveY(-20, 10).SetSpeedBased(true).SetRelative(true).OnComplete(()=> BlockPool.Instance.ReturnToPool(movingBlock, wantedColor));
        }
    }
}