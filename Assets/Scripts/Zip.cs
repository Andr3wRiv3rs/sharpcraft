using System.IO;
using System.IO.Compression;
using UnityEngine;
using Ionic.Zip;

namespace ZipUtility
{
    public static class Zip
    {
        public static ZipFile Deflate ( string filePath )
        {
            using (ZipFile zip = ZipFile.Read( filePath ))
            {
                return zip;
            }
        }
    }
}
