using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XunFeiSpeech.TTS;
using UnityEngine.SceneManagement;
using System.IO;

public class SpeechSpeak : MonoBehaviour
{
    //企鹅声音  x4_ningning     旁白声音   catherine
    private TextAudioBehaiver audioBehaiver;
    //public Animator qiEObj;
    //public string gsMsg;

    private string qiEVoice = "x4_ningning";
    private string pangBaiVoice = "catherine";
    public string firstQiESpeek;
    public GameObject firstPangBaiText;
    public GameObject secondBG;

    public Button setBtn;
    public Button stopBtn;
    public Button returnSetBtn;
    public Button returnStopBtn;

    public GameObject setObj;
    public GameObject stopObj;

    public GameObject qieList;

    public GameObject pingjiaParent;
    public GameObject qipaoParent;

    public int allScore;   //perfact 3      great 2    good 1

    public Image jinduSlider;
    public Text jinduText;

    public int jinduInt;

    int haysNum = 0;

    public int dlNum = 0;

    public GameObject starObj;

    public Button refashBtn;
    public Transform diqiu;
    public bool isRotate;

    public GameObject helloObj;
    void Start()
    {
        //StartCoroutine(ReadTxt());
        //allScore = 0;
        refashBtn.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(0);
        });
        setBtn.onClick.AddListener(delegate
        {
            setObj.SetActive(true);
        });
        stopBtn.onClick.AddListener(delegate
        {
            stopObj.SetActive(true);
        });
        returnSetBtn.onClick.AddListener(delegate
        {
            setObj.SetActive(false);
        });
        returnStopBtn.onClick.AddListener(delegate
        {
            stopObj.SetActive(false);
        });

        audioBehaiver = this.GetComponent<TextAudioBehaiver>();
        audioBehaiver.RegistCallBack(OnCallBack);
        StartCoroutine(FirstStart());
        //audioBehaiver.ctrl.defultParams.voice_name = "x4_ningning";
    }
    IEnumerator ReadTxt()
    {
        UnityWebRequest unityWeb = UnityWebRequest.Get(Application.streamingAssetsPath + "/a.txt");
        yield return unityWeb.SendWebRequest();
        string a = unityWeb.downloadHandler.text;
        //print(a);
        StatusMsg statusMsg = JsonUtility.FromJson<StatusMsg>(a);
        print(statusMsg.audio.emotion);
        print(statusMsg.audio.score);
        print(statusMsg.pose.actions[0]);
    }
    void Update()
    {
        if (isRotate)
        {
            diqiu.Rotate(Time.deltaTime*5,0,0);
        }
        if (jinduSlider.fillAmount < (float)jinduInt / 100)
        {
            jinduSlider.fillAmount += Time.deltaTime/20;
        }
        else
        {
            jinduSlider.fillAmount = (float)jinduInt / 100;
        }
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    audioBehaiver.PlayAudio(gsMsg);
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{ 
        //    ReciveScore("1,100");
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    ReciveScore("2,100");
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    ReciveScore("3,60");
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    ReciveScore("4,80");
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    ReciveScore("5,80");
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ReciveScore("6,80");
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    ReciveScore("7,80");
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    ReciveScore("8,80");
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ReciveScore("9,80");
        //}
    }
    public void UseAIVoice(string voiceName,string voiceStr)
    {
        audioBehaiver.ctrl.defultParams.voice_name = voiceName;
        audioBehaiver.PlayAudio(voiceStr);
    }
    private void OnCallBack(string arg0)
    {
        Debug.Log("Complete:" + arg0);
        if(arg0 == "Hello! My name is Pip")
        {
            
            StartCoroutine(SpeedFirstPangBai());
        }
        else if (arg0 == "A group of penguins live together on the vast Antarctic ice sheet. Among them is Pip, a curious baby penguin with dreams of exploring the world beyond. One day, Pip decides to set out on an adventure.")
        {
            StartCoroutine(EnterFirstSpeek(0, "Pip says happily", "开心"));
        }
        else if (arg0 == "Pip says happily")
        {
            QiPaoCtrl("3");
            StartCoroutine(EnterQiESpeek("That’s great! I’m so excited to go on an adventure. I really want to know what the outside world is like!"));
        }
        else if (arg0 == "With a big smile, Pip claps his flippers and jumps up and down, then leaves the group.")
        {
            StartCoroutine(EnterFirstSpeek(1, "After walking for a long time, Pip arrives in an unfamiliar area. Suddenly, he slips on a hidden patch of ice and falls with a heavy thud into a hole in the ice.", "走路"));
        }
        else if (arg0 == "After walking for a long time, Pip arrives in an unfamiliar area. Suddenly, he slips on a hidden patch of ice and falls with a heavy thud into a hole in the ice.")
        {
            StartCoroutine(EnterFirstSpeek(2, "Pip is startled. His beak opens wide, flippers stretch out, and he quickly hops upright.", "惊讶"));
        }
        else if (arg0 == "Pip is startled. His beak opens wide, flippers stretch out, and he quickly hops upright.")
        {
            QiPaoCtrl("5");
            StartCoroutine(EnterQiESpeek("How did I end up in a place like this? I wasn’t expecting this at all!"));

        }
        else if (arg0 == "he says.")
        {
            if (haysNum == 0)
            {
                StartCoroutine(EnterFirstSpeek(3, "Pip starts to feel afraid in this strange and unfamiliar place. He frowns, bends down, lowers his head, and holds it tightly as he shivers all over.", "害怕"));
            }
            else if(haysNum == 1)
            {
                StartCoroutine(EnterFirstSpeek(4, "Pip has been stuck in the hole for a long time. He feels cold, hungry, and all alone.", "悲伤"));
            }
            haysNum += 1;
        }
        else if (arg0 == "Pip starts to feel afraid in this strange and unfamiliar place. He frowns, bends down, lowers his head, and holds it tightly as he shivers all over.")
        {
            QiPaoCtrl("6");
            StartCoroutine(EnterQiESpeek("This place is so unfamiliar. I’m really scared!"));

        }
        else if (arg0 == "Pip has been stuck in the hole for a long time. He feels cold, hungry, and all alone.")
        {
            QiPaoCtrl("7");
            StartCoroutine(EnterQiESpeek("I’m trapped down here, and it makes me really sad."));
        }
        else if (arg0 == "he says. Pip frowns and lowers his head, worried that he might never get out. He holds his head, shakes it slowly, then looks up and begins to cry.")
        {
            StartCoroutine(EnterFirstSpeek(5, "Pip tries to climb toward the opening, but the ice is too slippery. He slips again and again. Feeling angry, he frowns, plants his flippers firmly on his sides, and stamps his feet.", "生气"));
        }
        else if (arg0 == "Pip tries to climb toward the opening, but the ice is too slippery. He slips again and again. Feeling angry, he frowns, plants his flippers firmly on his sides, and stamps his feet.")
        {
            QiPaoCtrl("8");
            StartCoroutine(EnterQiESpeek("I can’t get out right now, and that makes me really angry!"));
        }
        else if (arg0 == "he shouts.")
        {
            StartCoroutine(EnterFirstSpeek(6, "Suddenly, a cold wind blows through, and Pip smells a strange odor. He sees a pile of rubbish nearby and a dirty plastic bag floating towards him.", "待机"));
        }
        else if (arg0 == "Suddenly, a cold wind blows through, and Pip smells a strange odor. He sees a pile of rubbish nearby and a dirty plastic bag floating towards him.")
        {
            StartCoroutine(EnterFirstSpeek(7, "Feeling disgusted, Pip turns his body to the side, frowning and shaking his head as he waves his wings in refusal. He says,", "厌恶"));
        }
        else if (arg0 == "Feeling disgusted, Pip turns his body to the side, frowning and shaking his head as he waves his wings in refusal. He says,")
        {
            QiPaoCtrl("10");
            StartCoroutine(EnterQiESpeek("This is so disgusting! I really hate this rubbish! I don’t want to stay here any longer!"));
        }
        else if (arg0 == "Suddenly, Pip hears a voice from above: “How did you fall down there?")
        {
            StartCoroutine(EnterFirstSpeek(9, "Pip looks up and sees a penguin poking its head down into the hole. He jumps up and down and claps his flippers excitedly. A big smile spreads across his face as he says,", "开心"));
        }
        else if (arg0 == "Pip looks up and sees a penguin poking its head down into the hole. He jumps up and down and claps his flippers excitedly. A big smile spreads across his face as he says,")
        {
            QiPaoCtrl("12");
            StartCoroutine(EnterQiESpeek("Great! I’m not alone anymore! I accidentally fell into this ice hole. Can you help me get out?"));
        }
        else if (arg0 == "Don't be afraid, I'm here to help you! The penguin brought a long rope, Hold on to this and I'll pull you up.")
        {
            StartCoroutine(EnterFirstSpeek(11, "Pip holds onto the rope tightly. The penguin pulls with all his strength, stepping backward, and finally pulls Pip out of the ice hole. Pip claps his flippers and jumps up and down with joy, his face beaming with a bright smile. He says,", "开心"));
        }
        else if (arg0 == "Pip holds onto the rope tightly. The penguin pulls with all his strength, stepping backward, and finally pulls Pip out of the ice hole. Pip claps his flippers and jumps up and down with joy, his face beaming with a bright smile. He says,")
        {
            QiPaoCtrl("14");
            StartCoroutine(EnterQiESpeek("Thank you for your help! I did it! I’m so happy!"));
        }
        else if (arg0 == "After Pip gets out of the ice hole, sunlight shines through the clouds. Pip feels warm inside.")
        {
            StartCoroutine(EnterFirstSpeek(13, "The penguin leads Pip back home, and Pip finally sees his penguin group and his mum. Pip claps his flippers, bouncing with joy, his face glowing with a big smile. He says,", "开心"));
        }
        else if (arg0 == "The penguin leads Pip back home, and Pip finally sees his penguin group and his mum. Pip claps his flippers, bouncing with joy, his face glowing with a big smile. He says,")
        {
            QiPaoCtrl("16");
            StartCoroutine(EnterQiESpeek("Mum, I’m back! I even made a new friend! We overcame the difficulties together! I’m proud of myself!"));
        }
        else if (arg0 == "Mum hugs Pip and says, “Welcome home, Pip. You’ve grown up and become even braver.")
        {
            StartCoroutine(EnterFirstSpeek(15, "Through this adventure, Pip learned to understand his emotions and discovered that expressing how he feels and asking for help are important steps to overcome challenges and achieve his dreams.", "待机"));
        }
        else if (arg0 == "Through this adventure, Pip learned to understand his emotions and discovered that expressing how he feels and asking for help are important steps to overcome challenges and achieve his dreams.")
        {
            print("结束");
            starObj.SetActive(true);
        }
        //轮到小朋友说
        else if(arg0 == "That’s great! I’m so excited to go on an adventure. I really want to know what the outside world is like!"|| arg0== "How did I end up in a place like this? I wasn’t expecting this at all!"||arg0== "This place is so unfamiliar. I’m really scared!"||arg0== "I’m trapped down here, and it makes me really sad."||arg0== "I can’t get out right now, and that makes me really angry!"||arg0== "This is so disgusting! I really hate this rubbish! I don’t want to stay here any longer!"||arg0== "Great! I’m not alone anymore! I accidentally fell into this ice hole. Can you help me get out?"||arg0== "Thank you for your help! I did it! I’m so happy!"||arg0== "Mum, I’m back! I even made a new friend! We overcame the difficulties together! I’m proud of myself!")
        {
            this.GetComponent<TCPClient>().SendMessageToServer("start");
        }
        //else if (arg0 == "Pip starts to feel afraid in this strange and unfamiliar place. He frowns, bends down, lowers his head, and holds it tightly as he shivers all over.")
        //{
        //    QiPaoCtrl("6");
        //}
    }
    IEnumerator FirstStart()
    {
        yield return new WaitForSeconds(3.0f);
        //qiEObj.SetBool("e-huishou", true);
        helloObj.SetActive(true);
        QiEObjSelect("招手");
        UseAIVoice(qiEVoice, firstQiESpeek);
    }
    IEnumerator SpeedFirstPangBai()
    {
        yield return new WaitForSeconds(2.0f);
        QiEObjSelect("待机");
        helloObj.SetActive(false);
        firstPangBaiText.SetActive(true);
        UseAIVoice(pangBaiVoice, firstPangBaiText.transform.GetChild(0).GetComponent<TypewriterEffect>().fullText);
    }
    IEnumerator EnterFirstSpeek(int thisNum,string str,string aniName)
    {
        yield return new WaitForSeconds(2.0f);
        firstPangBaiText.SetActive(false);
        secondBG.SetActive(true);
        if (aniName != "")
        {
            QiEObjSelect(aniName);
        }
        for (int i = 0; i < secondBG.transform.childCount; i++)
        {
            if (i == thisNum)
            {
                secondBG.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                secondBG.transform.GetChild(i).gameObject.SetActive(false);
            }
        }


        yield return new WaitForSeconds(2.0f);
       
        UseAIVoice(pangBaiVoice, str);
        //if (isOpenPython)
        //{
        //    print("调用python");
        //}
    }
    IEnumerator EnterQiESpeek(string str)
    { 
        yield return new WaitForSeconds(2.0f);

        UseAIVoice(qiEVoice, str);
        
    }
    private void QiEObjSelect(string qieName)
    {
        if (qieName == "走路")
        {
            isRotate = true;
        }
        else
        {
            isRotate = false;
        }
        for (int i = 0; i < qieList.transform.childCount; i++)
        {
            if(qieList.transform.GetChild(i).name == "小企鹅"+qieName)
            {
                qieList.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                qieList.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }  
    public void ReciveScore(string score)
    {
        AppendTextToFile(score);
        StatusMsg statusMsg = JsonUtility.FromJson<StatusMsg>(score);

        //string[] scoreL = score.Split(',');
        //string dzStr = scoreL[0];
        //int dlNum = int.Parse(scoreL[0]);
        //double scoreInt = statusMsg.audio.score * 100;
        double scoreInt = 0;
        string faceBQ = statusMsg.audio.emotion;
        string faceBQ1 = statusMsg.pose.actions[0];
        //print("接收到序号:" + dlNum + "，分数:" + scoreInt);
        dlNum += 1;
        if ((dlNum == 1 && faceBQ1 == "Happy") || (dlNum == 2 && faceBQ1 == "Surprise") || (dlNum == 3 && faceBQ1 == "Fear") || (dlNum == 4 && faceBQ1 == "Sad") || (dlNum == 5 &&  faceBQ1 == "Angry") || (dlNum == 6 && faceBQ1 == "Disgust") || (dlNum == 7 && faceBQ1 == "Happy") || (dlNum == 8 && faceBQ1 == "Happy") || (dlNum == 9 && faceBQ1 == "Happy"))
        {
            print("动作通过");
            scoreInt = 50;
            if ((dlNum == 1 && faceBQ == "happy") || (dlNum == 2 && faceBQ == "surprise") || (dlNum == 3 && faceBQ == "fear") || (dlNum == 4 && faceBQ == "sad") || (dlNum == 5 && faceBQ == "angry") || (dlNum == 6 && faceBQ == "disgust") || (dlNum == 7 && faceBQ == "happy") || (dlNum == 8 && faceBQ == "happy") || (dlNum == 9 && faceBQ == "happy"))
            {
                print("表情通过" + statusMsg.audio.score * 100);
                scoreInt += statusMsg.audio.score * 50;
                
            }
            if (scoreInt >= 70)
            {
                PingJiaCtrl("perfact");
                allScore += 3;
            }
            else if (scoreInt < 69 && scoreInt >= 60)
            {
                PingJiaCtrl("great");
                allScore += 2;
            }
            else if (scoreInt < 59 && scoreInt >= 50)
            {
                PingJiaCtrl("good");
                allScore += 1;
            }
            else if (scoreInt < 50)
            {
                this.GetComponent<TCPClient>().SendMessageToServer("start");
                PingJiaCtrl("bad");
                dlNum -= 1;
                //print("调用python");
            }
            if (scoreInt >= 50)
            {

                QiPaoCtrl("");
                if (dlNum == 1)
                {
                    StartCoroutine(EnterFirstSpeek(0, "With a big smile, Pip claps his flippers and jumps up and down, then leaves the group.", ""));
                }
                else if (dlNum == 2)
                {
                    StartCoroutine(EnterFirstSpeek(2, "he says.", ""));
                }
                else if (dlNum == 3)
                {
                    StartCoroutine(EnterFirstSpeek(3, "he says.", ""));
                }
                else if (dlNum == 4)
                {
                    StartCoroutine(EnterFirstSpeek(4, "he says. Pip frowns and lowers his head, worried that he might never get out. He holds his head, shakes it slowly, then looks up and begins to cry.", ""));
                }
                else if (dlNum == 5)
                {
                    StartCoroutine(EnterFirstSpeek(5, "he shouts.", ""));
                }
                else if (dlNum == 6)
                {
                    StartCoroutine(EnterFirstSpeek(8, "Suddenly, Pip hears a voice from above: “How did you fall down there?", "待机"));
                }
                else if (dlNum == 7)
                {
                    StartCoroutine(EnterFirstSpeek(10, "Don't be afraid, I'm here to help you! The penguin brought a long rope, Hold on to this and I'll pull you up.", "待机"));
                }
                else if (dlNum == 8)
                {
                    StartCoroutine(EnterFirstSpeek(12, "After Pip gets out of the ice hole, sunlight shines through the clouds. Pip feels warm inside.", "待机"));
                }
                else if (dlNum == 9)
                {
                    StartCoroutine(EnterFirstSpeek(14, "Mum hugs Pip and says, “Welcome home, Pip. You’ve grown up and become even braver.", "待机"));
                }
                if (dlNum < 9)
                {
                    jinduInt += 10;
                }
                else if (dlNum == 9)
                {
                    jinduInt += 20;
                }
                jinduText.text = jinduInt.ToString() + "%";
            }
        }
        else
        {
            print("动作未通过");
            dlNum -= 1;
            PingJiaCtrl("bad");
            this.GetComponent<TCPClient>().SendMessageToServer("start");
        }
        //if ((dlNum == 1 &&(faceBQ == "happy" || faceBQ1 == "happy"))|| (dlNum == 2 && (faceBQ == "surprise" || faceBQ1 == "surprise")) || (dlNum == 3 && (faceBQ == "fear" || faceBQ1 == "fear")) || (dlNum == 4 && (faceBQ == "sad" || faceBQ1 == "sad")) || (dlNum == 5 && (faceBQ == "angry" || faceBQ1 == "angry")) || (dlNum == 6 && (faceBQ == "disgust" || faceBQ1 == "disgust")) || (dlNum == 7 && (faceBQ == "happy" || faceBQ1 == "happy")) || (dlNum == 8 && (faceBQ == "happy" || faceBQ1 == "happy")) || (dlNum == 9 && (faceBQ == "happy" || faceBQ1 == "happy")))
        //{
        //    print("动作通过" + score);
        //    if (scoreInt <= 100 && scoreInt >= 90)
        //    {
        //        PingJiaCtrl("perfact");
        //        allScore += 3;
        //    }
        //    else if (scoreInt < 90 && scoreInt >= 80)
        //    {
        //        PingJiaCtrl("great");
        //        allScore += 2;
        //    }
        //    else if (scoreInt < 80 && scoreInt >= 60)
        //    {
        //        PingJiaCtrl("good");
        //        allScore += 1;
        //    }
        //    else if (scoreInt < 60)
        //    {
        //        this.GetComponent<TCPClient>().SendMessageToServer("start");
        //        PingJiaCtrl("bad");
        //        dlNum -= 1;
        //        //print("调用python");
        //    }
        //    if (scoreInt >= 60)
        //    {

        //        QiPaoCtrl("");
        //        if (dlNum == 1)
        //        {
        //            StartCoroutine(EnterFirstSpeek(0, "With a big smile, Pip claps his flippers and jumps up and down, then leaves the group.", ""));
        //        }
        //        else if (dlNum == 2)
        //        {
        //            StartCoroutine(EnterFirstSpeek(2, "he says.", ""));
        //        }
        //        else if (dlNum == 3)
        //        {
        //            StartCoroutine(EnterFirstSpeek(3, "he says.", ""));
        //        }
        //        else if (dlNum == 4)
        //        {
        //            StartCoroutine(EnterFirstSpeek(4, "he says. Pip frowns and lowers his head, worried that he might never get out. He holds his head, shakes it slowly, then looks up and begins to cry.", ""));
        //        }
        //        else if (dlNum == 5)
        //        {
        //            StartCoroutine(EnterFirstSpeek(5, "he shouts.", ""));
        //        }
        //        else if (dlNum == 6)
        //        {
        //            StartCoroutine(EnterFirstSpeek(8, "Suddenly, Pip hears a voice from above: “How did you fall down there?", "待机"));
        //        }
        //        else if (dlNum == 7)
        //        {
        //            StartCoroutine(EnterFirstSpeek(10, "Don't be afraid, I'm here to help you! The penguin brought a long rope, Hold on to this and I'll pull you up.", "待机"));
        //        }
        //        else if (dlNum == 8)
        //        {
        //            StartCoroutine(EnterFirstSpeek(12, "After Pip gets out of the ice hole, sunlight shines through the clouds. Pip feels warm inside.", "待机"));
        //        }
        //        else if (dlNum == 9)
        //        {
        //            StartCoroutine(EnterFirstSpeek(14, "Mum hugs Pip and says, “Welcome home, Pip. You’ve grown up and become even braver.", "待机"));
        //        }
        //        if (dlNum < 9)
        //        {
        //            jinduInt += 10;
        //        }
        //        else if (dlNum == 9)
        //        {
        //            jinduInt += 20;
        //        }
        //        jinduText.text = jinduInt.ToString() + "%";
        //    }
            
        //}
        //else
        //{
        //    dlNum -= 1;
        //    PingJiaCtrl("bad");
        //    this.GetComponent<TCPClient>().SendMessageToServer("start");
        //}
        
        
    }
    private void PingJiaCtrl(string pjName)
    {
        for (int i = 0; i < pingjiaParent.transform.childCount; i++)
        {
            if (pingjiaParent.transform.GetChild(i).name == pjName)
            {
                pingjiaParent.transform.GetChild(i).gameObject.SetActive(true);
                pingjiaParent.transform.GetChild(i).GetComponent<ImageTrue>().ThisActive();
            }
            else
            {
                pingjiaParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void QiPaoCtrl(string qpName)
    {
        
        for (int i = 0; i < qipaoParent.transform.childCount; i++)
        {

            if (qipaoParent.transform.GetChild(i).name == qpName)
            {
               
                qipaoParent.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {

                qipaoParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void AppendTextToFile(string text)
    {
        string filePath = Application.streamingAssetsPath + "/Logs.txt";
        try
        {
            // 使用AppendAllText方法追加文本，自动处理文件创建和追加
            File.AppendAllText(filePath, text + "\n"); // 添加换行符使内容更清晰
            Debug.Log("文本已成功追加到文件: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("写入文件时出错: " + e.Message);
        }
    }
}
[Serializable]
public class StatusMsg
{
    public AudioMsg audio;
    public PoseMsg pose;
}
[Serializable]
public class AudioMsg
{
    public string emotion;
    public double score;
}
[Serializable]
public class PoseMsg
{
    public List<string> actions;
}
