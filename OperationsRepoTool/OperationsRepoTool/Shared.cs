using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharedNamespace
{
    public struct LauncherConfigJsonFile
    {
        public string filePath;
        public long size;
        public string date;
        public string md5;
    }

    public class LauncherConfigJson
    {
        public string serverHost;
        public string serverPassword;
        public string verifyLink;
        public string missionsLink;
        public string[] mods = new string[0];
        public string[] steamMods = new string[0];
        public List<LauncherConfigJsonFile> files = new List<LauncherConfigJsonFile>();
        public List<LauncherConfigJsonFile> steamFiles = new List<LauncherConfigJsonFile>();
    }

    public struct RepoConfigJson
    {
        public string serverHost;
        public string serverPassword;
        public string verifyLink;
        public string verifyPassword;
        public string missionsLink;
        public string[] mods;
        public string[] steamMods;
        public string modsFolder;
        public string steamModsFolder;
        public string syncFolder;
    }

    interface IMissionResponse {
        string file { get; }
        string hash { get; }
    }

    class CustomReadStream : Stream
    {
        Stream inner;
        int maxBytes;
        int bytesRead = 0;

        public CustomReadStream(Stream inner, int maxBytes)
        {
            this.inner = inner;
            this.maxBytes = maxBytes;
        }

        public override bool CanRead => inner.CanRead;

        public override bool CanSeek => inner.CanSeek;

        public override bool CanWrite => inner.CanWrite;

        public override long Length => inner.Length;

        public override long Position { get => inner.Position; set => inner.Position = value; }

        public override void Flush()
        {
            inner.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var result = inner.Read(buffer, offset, count);

            if (this.bytesRead > this.maxBytes)
            {
                return 0;
            }

            this.bytesRead += count;


            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            inner.Write(buffer, offset, count);
        }
    }

    class Shared
    {
        public static string GetMD5(string filename, bool getFullHash)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    if (getFullHash)
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }

                    long fileSize = new FileInfo(filename).Length;

                    var shortStream = new CustomReadStream(stream, Convert.ToInt32(fileSize * 0.1));

                    return BitConverter.ToString(md5.ComputeHash(shortStream)).Replace("-", "").ToLower();
                }
            }
        }

        public static string GetMD5FromBuffer(string text)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.Default.GetBytes(text);

                return BitConverter.ToString(md5.ComputeHash(inputBytes)).Replace("-", "").ToLower();
            }
        }

        public static Int32 GetUnixTime(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static List<string> GetFolderFilesToHash(string folderToParse, string[] modsList)
        {
            List<string> folderFiles = Directory.GetFiles(folderToParse, "*", SearchOption.AllDirectories).ToList();

            folderFiles = folderFiles.Select(a => a.Replace(folderToParse, "")).Select(b => b.ToLower()).ToList();

            folderFiles = folderFiles.Where(a => modsList.Any(b => a.StartsWith("\\" + b.ToLower() + "\\"))).Where(c => c.EndsWith(".pbo") || c.EndsWith(".dll")).ToList();

            return folderFiles;
        }

        public static void CheckSyncFolderSize(string repoFolderPath)
        {
            string archivePath = repoFolderPath + "\\.sync\\Archive";

            if (Directory.Exists(archivePath))
            {
                string[] archiveFilesArray = Directory.GetFiles(archivePath, "*", SearchOption.AllDirectories).ToArray();

                long bytes = 0;
                foreach (string name in archiveFilesArray)
                {
                    bytes += new FileInfo(name).Length;
                }

                if ((bytes / 1024 / 1024 / 1024) >= 1)
                {
                    MessageBox.Show("Your BTsync archive folder is too large. It's size is over " + (bytes / 1024 / 1024 / 1024) + " GB. You can clear it and disable archiving in the BTsync client.");

                    System.Diagnostics.Process.Start(archivePath);
                }
            }
        }
    }
}
