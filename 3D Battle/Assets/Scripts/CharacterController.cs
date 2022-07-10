using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpPower;
    private Animator animator;
    private Rigidbody rigid;

    private bool isJumping;
    private bool isGrounded;
    private bool isMove;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        if (Input.GetMouseButton(0))
        {
            LookAround();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
    }
    private void FixedUpdate()
    {
        move();
        if (isJumping&& isGrounded)
        {
            Jump();
        }
        animator.SetBool("isGrounded", isGrounded);
        if (rigid.velocity.y < 0&&!isGrounded) animator.SetBool("isFalling", true);
        

    }
    private void move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMove = moveInput.magnitude != 0;
        animator.SetBool("iswalk", isMove);
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float dx = camAngle.x - mouseDelta.y* cameraSpeed;
        if (dx < 180f)
        {
            dx = Mathf.Clamp(dx, 0, 60f);
        }
        else
        {
            dx = Mathf.Clamp(dx, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(dx, camAngle.y + mouseDelta.x* cameraSpeed, camAngle.z);
    }

    private void Jump()
    {
        animator.SetBool("isJumping", true);
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isJumping = false;
    }

    private void CheckGround()
    {
        Debug.Log(rigid.velocity.y);
        RaycastHit Hit;
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, 0.1f))
        {
            if (Hit.transform.tag =="Ground")
            {
                isGrounded = true;
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
                return;
            }
        }
        isGrounded = false;
    }

}
