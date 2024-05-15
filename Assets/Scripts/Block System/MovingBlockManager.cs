using DG.Tweening;
using UnityEngine;

namespace Game.BlockSystem
{
    public class MovingBlockManager : MonoBehaviour
    {
        [SerializeField] private MovingBlockInitSettings blockSettings;
        [SerializeField] private Transform finish;
        MovingBlock currentBlock, prevBlock;
        bool first = true, canCut = true;

        enum MatchingCondition
        {
            Perfect,
            Match,
            Fail
        }

        private int levelBlockCount = 0;
        private int levelMaxBlockCount = 0;
        private int perfectMatchCount = 0;

        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.MOUSE_CLICK_DOWN, OnMouseClickDown);
            EventManager.StartListening(GameEvents.LEVEL_CREATED, OnLevelCreated);
            EventManager.StartListening(GameEvents.CONTINUE_GAME, OnContinue);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.MOUSE_CLICK_DOWN, OnMouseClickDown);
            EventManager.StopListening(GameEvents.LEVEL_CREATED, OnLevelCreated);
            EventManager.StopListening(GameEvents.CONTINUE_GAME, OnContinue);
        }
        private void OnLevelCreated(object[] obj)
        {
            Level level = (Level)obj[0];
            if (level == null)
            {
                Debug.LogError("Level missing please check code or prefab !!");
                return;
            }
            finish = level.Finish;
            if (level.FirstBlock != null)
            {
                RegisterFirstBlock(level.FirstBlock);
            }
            else
            {
                canCut = false;
            }
            CalculateMaxBlockCount(currentBlock);
        }
        private void OnContinue(object[] obj)
        {
            levelBlockCount = 0;
            first = true;
            float initWidthDif = Mathf.Abs(blockSettings.BlockWidth - currentBlock.Width);
            if (initWidthDif > 0)
                currentBlock.Grow(initWidthDif, .2f, finish.position.x);
            CallNewBlock();
            EventManager.TriggerEvent(GameEvents.BLOCK_RETURN_POOL, new object[] { Camera.main.transform.position.z});
        }
        private void RegisterFirstBlock(MovingBlock targetBlock)
        {
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
            if ((levelBlockCount >= levelMaxBlockCount - 1 && !first) || !canCut)
            {
                canCut = true;
                return;
            }
            levelBlockCount++;
            BlocksEdges blocksEdges = new BlocksEdges(currentBlock, prevBlock);
            MatchingCondition condition = GetBlockMatchingType(blocksEdges);
            currentBlock.StopMovement();
            if (first)
            {
                EventManager.TriggerEvent(GameEvents.START_MOVEMENT, null);
                first = false;
            }
            switch (condition)
            {
                case MatchingCondition.Perfect:
                    PerfectMatch();
                    break;
                case MatchingCondition.Match:
                    Match(blocksEdges);
                    break;
                case MatchingCondition.Fail:
                    Fail();
                    break;
            }
        }
        private void CallNewBlock()
        {
            if (levelBlockCount >= levelMaxBlockCount - 1 && !first) return;
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
            prevBlock.nextBlock = currentBlock;
            Vector3 pos = currentBlock.transform.position;
            pos.x = prevBlock.transform.position.x;
            currentBlock.transform.position = pos;
            currentBlock.Center = pos;
            perfectMatchCount++;
            if (perfectMatchCount >= blockSettings.PerfectMatchBlockToGrow && currentBlock.Width < blockSettings.BlockWidth)
            { 
                currentBlock.Grow(blockSettings.BlockWidth * blockSettings.GrowRatio, blockSettings.GrowDuration, finish.position.x);
            }
            CallNewBlock();
            AudioManager.PlaySound(Sound.PerfectMatch, true, false);
        }
        private void Match(BlocksEdges blocksEdges)
        {
            prevBlock.nextBlock = currentBlock;
            EventManager.TriggerEvent(GameEvents.CUT_BLOCK, new object[] { currentBlock, blocksEdges });
            CallNewBlock();
            AudioManager.PlaySound(Sound.PerfectMatch, false, true);
            perfectMatchCount = 0;
        }
        private void Fail()
        {
            DropCurrentBlock();
        }
        private void DropCurrentBlock()
        {
            currentBlock.transform.DOMoveY(-20, 10).SetSpeedBased(true).SetRelative(true);
        }
        private void CalculateMaxBlockCount(MovingBlock firstBlock)
        {
            float levelLength = Mathf.Abs((finish.transform.position.z + (blockSettings.BlockLength * .1f)) - firstBlock.Center.z);
            levelMaxBlockCount = Mathf.CeilToInt(levelLength / blockSettings.BlockLength);
        }
        private MatchingCondition GetBlockMatchingType(BlocksEdges blocksEdges)
        {
            if (prevBlock == null || currentBlock == null)
            {
                Debug.LogError("Block referances missing please debug!!");
                return MatchingCondition.Fail;
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
                return MatchingCondition.Match;
            }

            return MatchingCondition.Fail;
        }
    }
}