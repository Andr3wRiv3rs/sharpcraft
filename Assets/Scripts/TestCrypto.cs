using UnityEngine;
using Crypto;
using System;

class TestCrypto : MonoBehaviour
{
    ECDSA ecdsa;
    void Start ()
    {
        ecdsa = new ECDSA("abc", "example@gmail.com");
        Debug.Log(
            ecdsa.Validate(
                "hello world",
                ecdsa.Sign("hello world")
            )
        );

        Debug.Log(ecdsa.publicKey);
    }
}