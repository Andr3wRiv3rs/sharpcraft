using UnityEngine;
using Crypto;
using System;

class TestCrypto : MonoBehaviour
{
    void Start ()
    {
        Signature.Generate("abc", "example@gmail.com");
        
        // Signature.Import(string signingKey);

        Debug.Log(
            Signature.Validate(
                "hello world",
                Signature.Sign("hello world")
            )
        );

        Debug.Log(Signature.signingKey); // secret
        Debug.Log(Signature.publicKey); // public
    }
}