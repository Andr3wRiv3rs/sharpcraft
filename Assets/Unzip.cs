using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Unzip : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void UnzipFilePrompt();

    void Start()
    {
        UnzipFilePrompt();
    }
}
