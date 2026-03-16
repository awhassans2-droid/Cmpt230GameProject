using UnityEngine;

public class CatController : MonoBehaviour
{
    public float walkSpeed = 2.2f;
    public float runSpeed = 4.2f;
    public float turnSpeed = 12f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    public Transform cameraTransform;

    private Animator animator;
    private bool isGrounded = true;
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) input += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) input += Vector3.back;
        if (Input.GetKey(KeyCode.A)) input += Vector3.left;
        if (Input.GetKey(KeyCode.D)) input += Vector3.right;

        input = input.normalized;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * input.z + camRight * input.x).normalized;

        bool isMoving = move.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (isMoving)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
        
        if (controller.isGrounded && velocity.y < 0)
	{
    		velocity.y = -2f;
	}

	if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
	{
    		velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    		isGrounded = false;

    	if (animator != null)
        	animator.SetTrigger("Jump");
	}

	velocity.y += gravity * Time.deltaTime;
	controller.Move(velocity * Time.deltaTime);
	isGrounded = controller.isGrounded;


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (animator != null)
                animator.SetTrigger("Smack");
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }
}

