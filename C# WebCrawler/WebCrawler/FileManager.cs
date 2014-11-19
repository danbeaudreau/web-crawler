using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.IO;

namespace WebCrawler
{
    class FileManager
    {

        public Constants.DownloadStatus PipeHtmlDataToLocalFile(string pageHtmlAsString, Uri uri, FileInfo localDirectory)
        {
            string uriAbsolutePath = uri.AbsolutePath.Replace("/", "\\");
            if (uriAbsolutePath.Substring(uriAbsolutePath.LastIndexOf('\\') + 1) == "")
            {
                uriAbsolutePath += "index";
            }
            string pipedPath = localDirectory.Directory + localDirectory.Name + "\\" + uri.Host + uriAbsolutePath;
            if (PathIsWellFormed(pipedPath))
            {
                string directory = localDirectory.Directory + localDirectory.Name + "\\" + uri.Host + uriAbsolutePath.Substring(0, uriAbsolutePath.LastIndexOf("\\"));
                Directory.CreateDirectory(directory);
                if (PathExists(directory) && PathHasWriteAccess(directory))
                {
                    try
                    {
                        using (FileStream fs = File.Create(pipedPath + ".html", 1024))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(pageHtmlAsString);
                            fs.Write(info, 0, info.Length);
                        }
                        return Constants.DownloadStatus.Success;
                    }
                    catch (IOException)
                    {
                        return Constants.DownloadStatus.FileWriteError;

                    }
                }
            }
            return Constants.DownloadStatus.FileWriteError;
        }

        
        public bool ValidateFilePath(string path)
        {
            if (PathIsWellFormed(path) && PathExists(path) && PathHasWriteAccess(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PathIsWellFormed(string path)
        {
            bool isValidPath = false;
            try
            {
                new System.IO.FileInfo(path);
                isValidPath = true;
            }
            catch (Exception)
            {
                isValidPath = false;
            }
            return isValidPath;
        }

        private bool PathExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        private bool PathHasWriteAccess(string path)
        {
            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);
            permissionSet.AddPermission(writePermission);
            bool hasWriteAccess = permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            return hasWriteAccess;
        }
    }
}
