using UnityEngine;

public class MinimapDot : MonoBehaviour
{
    public Transform target;        // 玩家
    public RectTransform mapRect;   // 小地图
    public float mapSize = 200f;    // 小地图大小
    public float worldSize = 100f;  // 场景大小

    void Update()
    {
        if (target == null) return;

        // 计算百分比位置
        float xPercent = target.position.x / worldSize;
        float zPercent = target.position.z / worldSize;

        // 转成 UI 坐标
        Vector2 pos = new Vector2(
            xPercent * mapSize,
            zPercent * mapSize
        );
        // 限制在小地图范围内
        pos.x = Mathf.Clamp(pos.x, -mapSize/2, mapSize/2);
        pos.y = Mathf.Clamp(pos.y, -mapSize/2, mapSize/2);
        
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}