using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform target;  // 拖入玩家

    void Update()
    {
        if (target == null) return;
        // 摄像机跟着玩家走，但保持高度不变
        transform.position = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );
    }
}