using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeedX = 0.5f;  // X轴滚动速度
    public float scrollSpeedY = 0.5f;  // Y轴滚动速度
    private Renderer rend;

    void Start()
    {
        // 获取物体的Renderer组件
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // 计算偏移量，使用Time.deltaTime确保速度一致
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // 应用偏移到主纹理
        rend.material.mainTextureOffset = new Vector2(offsetX/50, offsetY/50);
    }
}