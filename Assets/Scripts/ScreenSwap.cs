using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSwap : MonoBehaviour
{
    public RectTransform Object1;
    public RectTransform Object2;

    private void Swap(RectTransform Object_1, RectTransform Object_2)
    {
        Object_1.gameObject.SetActive(false);
        Object_2.gameObject.SetActive(true);
    }

    public void Click()
    {
        Swap(Object1, Object2);
    }
}