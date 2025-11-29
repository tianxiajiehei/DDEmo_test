using UnityEngine;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(SpriteRenderer))]
public class PNGSequencePlayer : MonoBehaviour
{
    [Header("序列帧设置")]
    [Tooltip("Resources文件夹下的图片文件夹路径")]
    public string folderPath;
    [Tooltip("动画播放速度(每秒帧数)")]
    public float frameRate = 30f;
    [Tooltip("是否自动播放")]
    public bool autoPlay = true; 
    [Tooltip("是否循环播放")]
    public bool loop = true;

    private SpriteRenderer spriteRenderer;
    private List<Sprite> frames = new List<Sprite>();
    private float frameInterval;
    private float timer;
    private int currentFrameIndex;
    private bool isPlaying;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 加载所有序列帧
        LoadFrames();

        // 计算帧间隔时间
        frameInterval = 1f / frameRate;

        if (autoPlay)
        {
            Play();
        }
    }

    void Update()
    {
        if (isPlaying && frames.Count > 0)
        {
            timer += Time.deltaTime;

            // 检查是否需要切换到下一帧
            if (timer >= frameInterval)
            {
                timer -= frameInterval;
                currentFrameIndex++;

                // 检查是否播放完毕
                if (currentFrameIndex >= frames.Count)
                {
                    if (loop)
                    {
                        currentFrameIndex = 0;
                    }
                    else
                    {
                        Stop();
                        return;
                    }
                }

                // 更新当前显示的帧
                spriteRenderer.sprite = frames[currentFrameIndex];
            }
        }
    }

    /// <summary>
    /// 从Resources文件夹加载所有帧
    /// </summary>
    private void LoadFrames()
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("请设置文件夹路径");
            return;
        }

        // 加载指定路径下的所有精灵
        Object[] loadedObjects = Resources.LoadAll(folderPath);

        foreach (Object obj in loadedObjects)
        {
            if (obj is Sprite sprite)
            {
                frames.Add(sprite);
            }
        }

        // 按名称排序（确保播放顺序正确）
        frames.Sort((a, b) => a.name.CompareTo(b.name));

        Debug.Log($"加载了 {frames.Count} 帧");

        if (frames.Count > 0)
        {
            spriteRenderer.sprite = frames[0];
        }
    }

    /// <summary>
    /// 开始播放动画
    /// </summary>
    public void Play()
    {
        isPlaying = true;
    }

    /// <summary>
    /// 暂停播放
    /// </summary>
    public void Pause()
    {
        isPlaying = false;
    }

    /// <summary>
    /// 停止播放并重置到第一帧
    /// </summary>
    public void Stop()
    {
        isPlaying = false;
        currentFrameIndex = 0;
        timer = 0;
        if (frames.Count > 0)
        {
            spriteRenderer.sprite = frames[0];
        }
    }
}
