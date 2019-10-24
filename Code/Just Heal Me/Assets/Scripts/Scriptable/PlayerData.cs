using Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "New Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("General Player Info")]
        public string playerName;

        public bool isPlayerCharacter = false;

        [Header("Player Stats")]
        public int health;

        [Conditional("isPlayerCharacter", true, true)]
        public int damage;

        public int attackSpeed;

        [Range(3, 20)]
        public float walkSpeed = 3;
    }
}