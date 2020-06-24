﻿using System.IO;

namespace EPlast.BusinessLogicLayer
{
    public class DirectoryManager : IDirectoryManager
    {
        public bool Exists(string path) => Directory.Exists(path);

        public DirectoryInfo CreateDirectory(string path) => Directory.CreateDirectory(path);

        public void Move(string sourceDirName, string destDirName) => Directory.Move(sourceDirName, destDirName);

        public string[] GetFiles(string path) => Directory.GetFiles(path);
    }
}