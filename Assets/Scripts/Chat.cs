using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public List<GameObject> Bubbles;

    public InputField inputField;

    public GameObject ChatBack;

    public GameObject BubblePrefab;

    public Color DarkBubbles;
    public Color LightBubbles;

    public int MessageLimit = 10;

    public int LettersPerLine = 10;

    public float BubbleHeight = 19.8f;

    private void Awake()
    {
        HideInput();
    }

    private void Start()
    {
        HideInput();
    }

    public void HideInput()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnlockPlayer();
        inputField.text = "";
        inputField.gameObject.SetActive(false);
        ColorBubbles(LightBubbles);
    }

    public void ShowInput()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LockPlayer();
        inputField.gameObject.SetActive(true);
        ColorBubbles(DarkBubbles);
    }

    public void ColorBubbles(Color color)
    {
        for(int i = 0; i < Bubbles.Count; i++)
        {
            Bubbles[i].gameObject.GetComponent<Image>().color = color;
        }
    }

    bool isChat(string str)
    {
        if (wsClient.Split(str)[0] == "MSG")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void onMessage(string str)
    {
        if (isChat(str))
        {
            NewBubble(str);

            if (Bubbles.Count > MessageLimit)
            {
                GameObject Removed = Bubbles[Bubbles.Count - 1];
                Bubbles.Remove(Bubbles[Bubbles.Count - 1]);
                Destroy(Removed.gameObject);
            }
        }
    }

    public void NewBubble(string str)
    {
        GameObject bubble = Instantiate(BubblePrefab);
        bubble.transform.SetParent(ChatBack.transform);
        float newHeight = BubbleHeight;// * (str.Length / LettersPerLine);

        bubble.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatBack.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, bubble.GetComponent<RectTransform>().sizeDelta.y / 2);
        bubble.transform.GetChild(0).GetComponent<Text>().text = str;

        //move all bubbles up
        for(int i = 0; i < Bubbles.Count; i++)
        {
            Bubbles[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(0, bubble.GetComponent<RectTransform>().sizeDelta.y);
        }

        Bubbles.Add(bubble);
    }

    public bool FieldSwitch = false;
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Slash)) && !inputField.isFocused && ServerMaster.inGame)
        {
            if (FieldSwitch)
            {
                FieldSwitch = false;
                HideInput();
            }
            else
            {
                FieldSwitch = true;
                ShowInput();
                if (Input.GetKeyDown(KeyCode.Slash))
                {
                    inputField.text = "/";
                }
                inputField.Select();
                inputField.ActivateInputField();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && ServerMaster.inGame)
        {
            if(inputField.text != "")
            {
                wsClient.Send(inputField.text);
                inputField.text = "";
                FieldSwitch = false;
                HideInput();
            }
            else
            {
                FieldSwitch = false;
                HideInput();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(FieldSwitch == true)
            {
                FieldSwitch = false;
                HideInput();
            }
        }
    }
}