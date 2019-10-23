using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(3, 20)] private float speed = 3f;

    public float Speed => speed;

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