using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("玩家根物体")]
    public Transform player;
    [Header("角色头部空物体（眼睛高度）")]
    public Transform headPoint;
    [Header("第三人称镜头参数")]
    public float distance = 6f;
    public float height = 2.8f; // 拉高相机高度，避免贴地面
    [Header("镜头平滑过渡速度")]
    public float smoothSpeed = 10f;
    [Header("鼠标转动灵敏度")]
    public float mouseSensitivity = 2f;
    [Header("角色身体渲染组件（第一人称隐藏）")]
    public Renderer bodyRender;

    private bool isThirdPerson = true;
    private float yRot;
    private float xRot;

    void Update()
    {
        // V快捷键切换视角
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchView();
        }
    }

    void LateUpdate()
    {
        // 鼠标旋转镜头
        xRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        yRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        yRot = Mathf.Clamp(yRot, -60f, 60f);

        if (isThirdPerson)
        {
            RunThirdPerson();
            bodyRender.enabled = true;
        }
        else
        {
            RunFirstPerson();
            bodyRender.enabled = false;
        }
    }

    void RunThirdPerson()
    {
        Quaternion orbitRot = Quaternion.Euler(yRot, xRot, 0);
        Vector3 offsetPos = orbitRot * Vector3.back * distance;
        Vector3 targetPos = player.position + offsetPos;
        targetPos.y += height;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        // 核心修改：看向头部，不再看脚底player
        transform.LookAt(headPoint);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void RunFirstPerson()
    {
        transform.position = Vector3.Lerp(transform.position, headPoint.position, smoothSpeed * Time.deltaTime);
        Quaternion targetViewRot = Quaternion.Euler(yRot, xRot, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetViewRot, smoothSpeed * Time.deltaTime);
        headPoint.rotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // UI按钮绑定用的公开切换函数
    public void SwitchView()
    {
        isThirdPerson = !isThirdPerson;
    }
}