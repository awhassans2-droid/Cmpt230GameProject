using UnityEngine;

public class MaxMovements : MonoBehaviour
{
    public Transform target; // Max
    public float followRange = 5f; // Following Clementine
    public float loseInterestDistance = 10f; // When Max goes back to his initial spot
    public float stopDistance = 1.5f; // Distance between him and clementine
    public float behindDistance = 1.5f; // How far Max is from CLementine

    // Max speeds
    public float moveSpeed = 2f; 
    public float runSpeed = 6f;
    public float turnSpeed = 5f;

    // Speed when return back to spawn position
    public float returnSpeed = 2.5f; 

    private Animator animator;
    private CatController clementine;

    private bool isFollowing = false;
    private bool returningHome = false;

    // Records the initial position
    private Vector3 spawnPosition;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (target != null)
            clementine = target.GetComponent<CatController>();

        spawnPosition = transform.position;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 flatTarget = target.position;
        flatTarget.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, flatTarget);

        // When Clementine smacks Max, he goes back to his initial position
        if (clementine != null && clementine.didSmack)
        {
            isFollowing = false;
            returningHome = true;
        }

        // Start following
        if (!returningHome && !isFollowing && distance <= followRange)
        {
            isFollowing = true;
        }

        // Too far → go home
        if (isFollowing && distance > loseInterestDistance)
        {
            isFollowing = false;
            returningHome = true;
        }

        bool shouldRun = false;
        float speed = moveSpeed;

        if (clementine != null)
        {
            shouldRun = clementine.isRunning;
            speed = shouldRun ? runSpeed : moveSpeed;
        }

        // Follows Clementine (stays behind her)
        Vector3 behindTarget = target.position - target.forward * behindDistance;
        behindTarget.y = transform.position.y;

        float distBehind = Vector3.Distance(transform.position, behindTarget);

        if (isFollowing && distBehind > stopDistance)
        {
            Vector3 direction = (behindTarget - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            Rotate(direction);

            SetAnim(true, shouldRun);
            return;
        }

        // Goes back to initial spot
        if (returningHome)
        {
            Vector3 homeTarget = spawnPosition;
            homeTarget.y = transform.position.y;

            float distToHome = Vector3.Distance(transform.position, homeTarget);

            if (distToHome > 0.2f)
            {
                Vector3 direction = (homeTarget - transform.position).normalized;
                transform.position += direction * returnSpeed * Time.deltaTime;

                Rotate(direction);

                SetAnim(true, false);
            }
            else
            {
                returningHome = false;
                SetAnim(false, false);
            }

            return;
        }

        // Idle
        SetAnim(false, false);
    }

    void Rotate(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    void SetAnim(bool moving, bool running)
    {
        if (animator == null) return;

        animator.SetBool("IsMoving", moving);
        animator.SetBool("IsRunning", running);
    }
}

