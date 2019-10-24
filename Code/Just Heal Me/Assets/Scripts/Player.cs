using Scriptable;
using UnityEngine;

public class Player : MonoBehaviour
{
//    [SerializeField, Range(3, 20)] private float speed = 3f;
    [SerializeField] private PlayerData playerData;

    public float Speed => playerData.walkSpeed;

    public CharacterController CharacterController { get; private set; }

    public Animator Animator { get; private set; }

    public Animation Animation { get; private set; }

    #region -----[ Unity Lifecycle ]-------------------------------------------

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        Animation = GetComponent<Animation>();
    }

    #endregion
}