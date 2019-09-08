using UnityEngine;
using Crypto;
using System;

class TestCrypto : MonoBehaviour
{
    RSA rsa;
    void Start ()
    {
        rsa = new RSA();
        string encrypted = rsa.Encrypt("abc");

        Debug.Log(encrypted);

        string decrypted = rsa.Decrypt(encrypted);
    }
}