using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BlockSystem
{
    public class BlockPool : Singleton<BlockPool>
    {

        private Dictionary<Material, List<MovingBlock>> pooledBlocks = new Dictionary<Material, List<MovingBlock>>();

        [SerializeField] private MovingBlockInitSettings settings;

        private void Awake()
        {
            instance = this;
        }

        public MovingBlock GetPooledBlock(Material wantedColor)
        {
            return CheckPoolAndGetBlock(wantedColor);
        }

        private MovingBlock CheckPoolAndGetBlock(Material wantedColor)
        {
            if (!pooledBlocks.ContainsKey(wantedColor))
            {
                pooledBlocks.Add(wantedColor, new List<MovingBlock>());
            }
            var tempBlocks = pooledBlocks[wantedColor];
            MovingBlock pooledBlock;
            if (tempBlocks.Count == 0)
            {
                pooledBlock = CreateNewBlock(wantedColor);
            }
            else
            {
                pooledBlock = tempBlocks[tempBlocks.Count - 1];
            }
            pooledBlock.gameObject.SetActive(true);
            return pooledBlock;
        }
        private MovingBlock CreateNewBlock(Material wantedColor) 
        {
            MovingBlock createdBlock = Instantiate(settings.BlockPrefab);
            if (wantedColor == null) wantedColor = settings.BlockMaterials[0];
            createdBlock.SetModelColor(wantedColor);
            createdBlock.Initialize(settings.BlockWidth, settings.BlockLength);
            return createdBlock;
        }

        public void ReturnToPool(MovingBlock movingBlock, Material wantedColor)
        {
            if (movingBlock == null) return;
            if (!pooledBlocks.ContainsKey(wantedColor))
            {
                pooledBlocks.Add(wantedColor, new List<MovingBlock>());
            }
            movingBlock.ChangeColliderActive(false);
            pooledBlocks[wantedColor].Add(movingBlock);
            movingBlock.gameObject.SetActive(false);
        }
    }
}