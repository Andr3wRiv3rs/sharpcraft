using UnityEngine;
using Crypto;

class TestCrypto : MonoBehaviour
{
    RSA rsa;
    void Start ()
    {
        rsa = new RSA();
        string encrypted = rsa.Encrypt("abc");

        Debug.Log(encrypted);

        string decrypted = rsa.Decrypt(encrypted);

        Debug.Log(decrypted);
    }
}