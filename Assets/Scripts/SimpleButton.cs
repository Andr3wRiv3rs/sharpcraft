using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    public string Text = "";
    void Start()
    {
        gameObject.GetComponent<Text>().text = Text;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Text>().text = Text;
    }

    public void MouseEnter()
    {
        gameObject.GetComponent<Text>().text = "|" + Text;
    }

    public void MouseExit()
    {
        gameObject.GetComponent<Text>().text = Text;
    }
}