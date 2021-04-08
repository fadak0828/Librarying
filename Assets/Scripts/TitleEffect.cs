using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEffect : MonoBehaviour
{
    public GameObject[] talkBubble;
    void Start()
    {
        TalkBubbleActive(false);                //��ǳ�� ����
        StartCoroutine(TalkBubble());      //���ʷ� ���ֱ�
    }
    IEnumerator TalkBubble()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < talkBubble.Length; i++)
        {
            talkBubble[i].SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }
    void TalkBubbleActive(bool active)
    {
        for (int i = 0; i < talkBubble.Length; i++)
        {
            talkBubble[i].SetActive(active);
        }
    }
}
