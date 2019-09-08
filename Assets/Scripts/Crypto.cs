using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
            if (XMLPublicOrPrivateKey != null) {
                rsa.FromXmlString(XMLPublicOrPrivateKey);
            } else {
                rsa.FromXmlString(publicPrivateKey);
            }

            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(data), true));
        }

        public string Decrypt (string data, string XMLPublicOrPrivateKey = null) 
        {
            if (XMLPublicOrPrivateKey != null) {
                rsa.FromXmlString(XMLPublicOrPrivateKey);
            } else {
                rsa.FromXmlString(publicPrivateKey);
            }

            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(data), true));
        }
    }
}