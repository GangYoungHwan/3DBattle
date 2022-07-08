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
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            LookAround();
        }
        move();
    }
    private void move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
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
}
