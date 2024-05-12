using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BlockSystem
{
    public class BlockCuttingHandler : MonoBehaviour
    {
        public class CuttingInfo
        {
            public Vector3 MainLeft, MainRight;
            public Vector3 PrevLeft, PrevRight;
        }
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
            CuttingInfo blockCuttingInfo = (CuttingInfo)obj[1];
            if (movingBlock == null || blockCuttingInfo == null) return;

            CutBlock(movingBlock, blockCuttingInfo);
        }

        [SerializeField] List<MovingBlock> movingBlocks = new List<MovingBlock>();
        int count = 1;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && count < movingBlocks.Count)
            {
                CuttingInfo cuttingInfo = new CuttingInfo();
                movingBlocks[count - 1].Center = movingBlocks[count - 1].transform.position;
                cuttingInfo.PrevLeft = movingBlocks[count - 1].Center - (Vector3.right * (movingBlocks[count - 1].Width / 2));
                cuttingInfo.PrevRight = movingBlocks[count - 1].Center + (Vector3.right * (movingBlocks[count - 1].Width / 2));
                movingBlocks[count].Center = movingBlocks[count].transform.position;
                cuttingInfo.MainLeft = movingBlocks[count].Center - (Vector3.right * (movingBlocks[count].Width / 2));
                cuttingInfo.MainRight = movingBlocks[count].Center + (Vector3.right * (movingBlocks[count].Width / 2));
                movingBlocks[count].Center = movingBlocks[count].transform.position;

                CutBlock(movingBlocks[count], cuttingInfo);
                count++;
            }
        }
        private void CutBlock(MovingBlock movingBlock, CuttingInfo blockCuttingInfo)
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