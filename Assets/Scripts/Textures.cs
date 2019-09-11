using System;
using UnityEngine;
using ZipUtility;
using Ionic.Zip;

namespace Textures 
{
    public static class BlockTexture
    {
        public static string Appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        
        private static ZipFile textures;

        // public static Texture Get ()
        // {
            
        // }

        // public static Texture Request ()
        // {
            
        // } 

        private static void WebImport ()
        {

        }

        private static void LocalImport (string path)
        {
            if (path == null) 
                textures = Zip.Deflate(Appdata + "\\.minecraft\\versions\\1.14.4\\1.14.4.jar");

            foreach (ZipEntry texture in textures)
                Debug.Log(texture.Info);

        }

        public static void Import (string path = null)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) WebImport();
            else LocalImport(path);
        } 

    }
}