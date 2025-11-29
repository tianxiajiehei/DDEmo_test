using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    [Tooltip("要显示的完整文本")]
    public string fullText;

    [Tooltip("打字速度（字符/秒）")]
    public float typingSpeed = 20f;

    [Tooltip("是否自动开始打字")]
    public bool autoStart = true;

    [Tooltip("打字音效（可选）")]
    public AudioClip typeSound;

    private Text textComponent;  // 老版本Text组件
    private AudioSource audioSource;
    private Coroutine typingCoroutine;
    private string currentText = "";

    void Awake()
    {
        // 获取老版本Text组件
        textComponent = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        // 初始化文本为空
        if (textComponent != null)
        {
            textComponent.text = "";
        }
    }

    void Start()
    {
        if (autoStart)
        {
            StartTyping();
        }
    }

    /// <summary>
    /// 开始打字效果
    /// </summary>
    public void StartTyping()
    {
        // 如果正在打字，先停止
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // 重置文本
        currentText = "";
        textComponent.text = currentText;

        // 开始打字协程
        typingCoroutine = StartCoroutine(TypeText());
    }

    /// <summary>
    /// 打字效果的协程
    /// </summary>
    private IEnumerator TypeText()
    {
        foreach (char c in fullText.ToCharArray())
        {
            currentText += c;
            textComponent.text = currentText;

            // 播放打字音效
            if (typeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSound);
            }

            // 控制打字速度
            yield return new WaitForSeconds(1f / typingSpeed);
        }

        typingCoroutine = null;
    }

    /// <summary>
    /// 立即显示全部文本
    /// </summary>
    public void ShowAllText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        currentText = fullText;
        textComponent.text = currentText;
    }

    /// <summary>
    /// 从代码设置文本并开始打字
    /// </summary>
    public void SetTextAndType(string newText)
    {
        fullText = newText;
        StartTyping();
    }
}