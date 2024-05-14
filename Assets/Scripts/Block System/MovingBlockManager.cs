using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.BlockSystem
{
    public class MovingBlockManager : MonoBehaviour
    {
        [SerializeField] private MovingBlockInitSettings blockSettings;
        [SerializeField] private Transform finish;
        MovingBlock currentBlock, prevBlock;
        bool first = true;

        enum MatchingCondition
        {
            Perfect,
            Inside,
            Outside
        }

        private int levelBlockCount = 0;
        private int levelMaxBlockCount = 0;

        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.MOUSE_CLICK_DOWN, OnMouseClickDown);
            EventManager.StartListening(GameEvents.REGISTER_FIRST_BLOCK, RegisterFirstBlock);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.MOUSE_CLICK_DOWN, OnMouseClickDown);
            EventManager.StopListening(GameEvents.REGISTER_FIRST_BLOCK, RegisterFirstBlock);
        }
        private void RegisterFirstBlock(object[] obj)
        {
            MovingBlock targetBlock = (MovingBlock)obj[0];
            if (targetBlock == null)
            {
                Debug.LogError("First block missing please check level prefab or code !!");
                return;
            }
            
            currentBlock = targetBlock;
            currentBlock.Initialize(blockSettings.BlockWidth, blockSettings.BlockLength);
            currentBlock.ChangeColliderActive(true);
            CallNewBlock();
        }
        private void OnMouseClickDown(object[] obj)
        {
            levelBlockCount++;
            BlocksEdges blocksEdges = new BlocksEdges(currentBlock, prevBlock);
            MatchingCondition condition = GetBlockMatchingType(blocksEdges);
            currentBlock.StopMovement();
            if (first)
            {
                EventManager.TriggerEvent(GameEvents.START_MOVEMENT, null);
                CalculateMaxBlockCount(currentBlock);
                first = false;
            }
            switch (condition)
            {
                case MatchingCondition.Perfect:
                    prevBlock.nextBlock = currentBlock;
                    PerfectMatch();
                    CallNewBlock();
                    break;
                case MatchingCondition.Inside:
                    prevBlock.nextBlock = currentBlock;
                    EventManager.TriggerEvent(GameEvents.CUT_BLOCK, new object[] { currentBlock, blocksEdges });
                    CallNewBlock();
                    break;
                case MatchingCondition.Outside:
                    DropCurrentBlock();
                    break;
            }
        }
        private void CallNewBlock()
        {
            if (levelBlockCount >= levelMaxBlockCount && !first) return;
            MovingBlock newBlock = BlockPool.Instance.GetPooledBlock(blockSettings.BlockMaterials[levelBlockCount % blockSettings.BlockMaterials.Length]);
            newBlock.ChangeColliderActive(true);
            newBlock.SetWidth(currentBlock.Width);
            Vector3 pos = currentBlock.transform.position;
            pos.z += currentBlock.Length;
            pos.x += (blockSettings.BlockWidth * blockSettings.BlockMovementStartPosMultiplier) * (Mathf.Pow(-1, levelBlockCount));
            float targetX = pos.x + ((blockSettings.BlockWidth * blockSettings.BlockMovementStartPosMultiplier * 2) * (Mathf.Pow(-1, levelBlockCount + 1)));
            newBlock.transform.position = pos;
            prevBlock = currentBlock;
            currentBlock = newBlock;
            currentBlock.MoveX(pos.x, targetX, blockSettings.BlockSpeed);
        }
        private void PerfectMatch()
        {
            Vector3 pos = currentBlock.transform.position;
            pos.x = prevBlock.transform.position.x;
            currentBlock.transform.position = pos;
            currentBlock.Center = pos;
            // TODO: call sound and vfx here
        }
        private void DropCurrentBlock()
        {
            currentBlock.transform.DOMoveY(-20, 10).SetSpeedBased(true).SetRelative(true);
        }
        private void CalculateMaxBlockCount(MovingBlock firstBlock)
        {
            float levelLength = Mathf.Abs(finish.transform.position.z - firstBlock.Center.z);
            levelMaxBlockCount = Mathf.CeilToInt(levelLength / blockSettings.BlockLength);
        }
        private MatchingCondition GetBlockMatchingType(BlocksEdges blocksEdges)
        {
            if (prevBlock == null || currentBlock == null)
            {
                Debug.LogError("Block referances missing please debug!!");
                return MatchingCondition.Outside;
            }
            float dist = Mathf.Abs(blocksEdges.MainLeft.x - blocksEdges.PrevLeft.x);
            float insideDist = (currentBlock.Width * (1 - blockSettings.PerfectMatchingThreshold));
            float perfectThreshold = currentBlock.Width * blockSettings.PerfectMatchingThreshold;
            if (dist <= perfectThreshold)
            {
                return MatchingCondition.Perfect;
            }
            else if (dist < insideDist && (blocksEdges.MainLeft.x > blocksEdges.PrevLeft.x || blocksEdges.MainRight.x < blocksEdges.PrevRight.x))
            {
                return MatchingCondition.Inside;
            }

            return MatchingCondition.Outside;
        }
    }
}