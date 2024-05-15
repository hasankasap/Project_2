using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab, additiveLevelPrefab;
        public Level GenerateLevel(bool additive = false, Vector3 targetPos = default)
        {
            Level prefab = additive ? additiveLevelPrefab : levelPrefab;
            Level createdLevel = Instantiate(prefab, targetPos, levelPrefab.transform.rotation);
            return createdLevel;
        }
    }
}