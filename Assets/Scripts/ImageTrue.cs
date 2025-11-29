using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTrue : MonoBehaviour
{
    public bool isStart;
    public float a;
    void Start()
    {
        //ThisActive();
    }

    void Update()
    {
        
        if (isStart)
        {
            a += Time.deltaTime * 200 / 255f;
            this.GetComponent<Image>().color = new Color(1, 1, 1, a);
            this.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, a);
        }
    }
    public void ThisActive()
    {
        isStart = true;
        StartCoroutine(CloseThisObj());
    }
    IEnumerator CloseThisObj()
    {
        yield return new WaitForSeconds(4.0f);
        isStart = false;
        a = 0;
        this.GetComponent<Image>().color = new Color(1, 1, 1, a);
        this.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, a);
        this.gameObject.SetActive(false);
    }
}
