using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Numerics;
using ECC;

namespace Crypto 
{
    public class RSA 
    {
        public string publicKey;
        private RSACryptoServiceProvider rsa;
        private string publicPrivateKey;

        public RSA (string XMLPublicPrivateKey = null) {
            if (XMLPublicPrivateKey != null) {
                rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(XMLPublicPrivateKey);
                publicPrivateKey = XMLPublicPrivateKey;
            } else {
                rsa = new RSACryptoServiceProvider(2048);
                publicPrivateKey = rsa.ToXmlString(true);
            }

            publicKey = rsa.ToXmlString(false);
        }

        public string Encrypt (string data, string XMLPublicOrPrivateKey = null) 
        {
            if (XMLPublicOrPrivateKey != null) rsa.FromXmlString(XMLPublicOrPrivateKey);
            else rsa.FromXmlString(publicPrivateKey);

            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(data), true));
        }

        public string Decrypt (string data, string XMLPublicOrPrivateKey = null) 
        {
            if (XMLPublicOrPrivateKey != null) rsa.FromXmlString(XMLPublicOrPrivateKey);
            else rsa.FromXmlString(publicPrivateKey);

            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(data), true));
        }
    }

    public class ECDSA 
    {
        private string signingKey;
        public string publicKey;
        private Ed25519 ed25519 = new Ed25519();

        public ECDSA (string key, string salt) 
        {
            signingKey = Convert.ToBase64String(
                new Rfc2898DeriveBytes(
                    Encoding.UTF8.GetBytes(key), 
                    Encoding.UTF8.GetBytes(salt), 
                    10
                ).GetBytes(32)
            );

            publicKey = Convert.ToBase64String(
                Ed25519.PublicKey(
                    Encoding.UTF8.GetBytes(signingKey)
                )
            );
        }

        public string Sign (string message)
        {
            return Convert.ToBase64String(
                Ed25519.Signature(
                    Encoding.UTF8.GetBytes(message), 
                    Encoding.UTF8.GetBytes(signingKey), 
                    Convert.FromBase64String(publicKey)
                )
            );
        }

        public bool Validate (string message, string signature)
        {
            return Ed25519.CheckValid(
                Convert.FromBase64String(signature), 
                Encoding.UTF8.GetBytes(message), 
                Convert.FromBase64String(publicKey)
            );
        }
    }
}