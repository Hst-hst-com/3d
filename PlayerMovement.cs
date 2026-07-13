using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("拖拽赋值组件")]
    public CharacterController cc;
    public Animator playerAnim;

    [Header("移动速度参数")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float turnSpeed = 6f;

    [Header("跳跃与重力")]
    public float gravity = -19.6f;
    public float jumpHeight = 2f;

    private Vector3 fallVelocity;

    void Update()
    {
        // 1. 获取WASD输入
        float horizontal = Input.GetAxisRaw("Horizontal");
        float forward = Input.GetAxisRaw("Vertical");

        // 以相机朝向计算移动方向
        Transform mainCam = Camera.main.transform;
        Vector3 camForward = mainCam.forward;
        Vector3 camRight = mainCam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * forward + camRight * horizontal;
        if (moveDir.magnitude > 1)
            moveDir.Normalize();

        // 判断是否跑步
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // 仅向前移动时才自动转向，后退不转
        if (moveDir.magnitude > 0.01f && forward > 0)
        {
            Quaternion targetRotate = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, turnSpeed * Time.deltaTime);
        }

        // 水平移动
        cc.Move(moveDir * currentSpeed * Time.deltaTime);

        // 落地缓冲：只要在地面，强制锁住下坠力，防止站立时isGrounded闪断
        if (cc.isGrounded)
        {
            fallVelocity.y = -2f;
            // 地面状态下才能跳
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fallVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }
        // 空中施加重力
        else
        {
            fallVelocity.y += gravity * Time.deltaTime;
        }

        cc.Move(fallVelocity * Time.deltaTime);

        // 走路跑步动画参数
        playerAnim.SetFloat("Speed", moveDir.magnitude * currentSpeed);
    }
}