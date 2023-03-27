using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [YarnCommand("fadeIn")]
    static IEnumerator FadeIn(GameObject obj)
    {
        float FadeCount = 0;
        Color color;
        color = obj.GetComponent<Image>().color;
        while (FadeCount < 1.0f)
        {
            FadeCount += 0.01f;
            yield return new WaitForSeconds(0.005f);
            color.a = FadeCount;
            obj.GetComponent<Image>().color = color;
        }
    }
    [YarnCommand("fadeOut")]
    static IEnumerator FadeOut(GameObject obj)
    {
        float FadeCount = 1f;
        Color color;
        color = obj.GetComponent<Image>().color;
        while (FadeCount > 0)
        {
            FadeCount -= 0.01f;
            yield return new WaitForSeconds(0.005f);
            color.a = FadeCount;
            obj.GetComponent<Image>().color = color;
        }
    }
    [YarnCommand("textFadeIn")]
    static IEnumerator TextFadeIn(GameObject obj)
    {
        float FadeCount = 0;
        Color color;
        color = obj.GetComponent<TextMeshProUGUI>().color;
        while (FadeCount < 1.0f)
        {
            FadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            color.a = FadeCount;
            obj.GetComponent<TextMeshProUGUI>().color = color;
        }
    }
    [YarnCommand("textFadeOut")]
    static IEnumerator TextFadeOut(GameObject obj)
    {
        float FadeCount = 1f;
        Color color;
        color = obj.GetComponent<TextMeshProUGUI>().color;
        while (FadeCount > 0)
        {
            FadeCount -= 0.01f;
            yield return new WaitForSeconds(0.02f);
            color.a = FadeCount;
            obj.GetComponent<TextMeshProUGUI>().color = color;
        }
    }
    [YarnCommand("objectAppear")]
    public static void objectAppear(GameObject obj)
    {
        Color color;
        color = obj.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        obj.GetComponent<SpriteRenderer>().color = color;
    }
    [YarnCommand("objectDisappear")]
    public static void ObjectDisappear(GameObject obj)
    {
        Color color;
        color = obj.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        obj.GetComponent<SpriteRenderer>().color = color;
    }

    [YarnCommand("screenAppear")]
    public static void ScreenAppear()
    {
        GameObject.Find("GameObject").transform.Find("Screen").gameObject.SetActive(true);
    }
    [YarnCommand("screenDisappear")]
    public static void ScreenDisappear()
    {
        GameObject.Find("GameObject").transform.Find("Screen").gameObject.SetActive(false);
    }
}
