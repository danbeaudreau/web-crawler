using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Permissions;

namespace WebCrawler
{
    class FileManager
    {
        public bool validateFilePath(string path)
        {
            if (pathIsWellFormed(path) && pathExists(path) && pathHasWriteAccess(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool pathIsWellFormed(string path)
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

        private bool pathExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        private bool pathHasWriteAccess(string path)
        {
            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);
            permissionSet.AddPermission(writePermission);
            bool hasWriteAccess = permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            return hasWriteAccess;
        }
    }
}
