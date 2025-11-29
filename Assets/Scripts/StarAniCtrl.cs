using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAniCtrl : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void StarAni(int num)
    {
        int thisNum = 0;
        GameObject obj = GameObject.Find("GameManager");
        if (obj.GetComponent<SpeechSpeak>().allScore >= 23)
        {
            thisNum = 3;
        }
        else if (obj.GetComponent<SpeechSpeak>().allScore >= 16 && obj.GetComponent<SpeechSpeak>().allScore < 23)
        {
            thisNum = 2;
        }
        else if (obj.GetComponent<SpeechSpeak>().allScore >= 0 && obj.GetComponent<SpeechSpeak>().allScore < 16)
        {
            thisNum = 1;
        }
        if (num == thisNum)
        {
            this.GetComponent<Animator>().speed = 0;
        }
    }
}
