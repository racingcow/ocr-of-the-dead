using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 0.5f;
    public float rotateSpeed = 0.1f;
    public float attackDistance = 4f;

    private bool _walking = true;
    private bool _attacking = false;

    private Animator _animator;

    // Use this for initialization
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = FindObjectOfType<Camera>().transform.position;
        var zombiePos = transform.position;
        var targetDirection = playerPos - zombiePos;
        targetDirection.y = 0; //flying not allowed

        var curDistance = Vector3.Distance(playerPos, zombiePos);
        _attacking = curDistance <= attackDistance;

        _walking = !_attacking;
        _animator.SetBool("IsWalking", _walking);
        _animator.SetBool("IsAttacking", _attacking);

        if (!_walking) return;

        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime*rotateSpeed, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.position += targetDirection.normalized*speed*Time.deltaTime;
    }
}
