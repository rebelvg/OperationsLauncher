using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace MurshunUploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label2.Text = "Version " + version;
        }

        string version = "1.0";

        static byte[] TempArrayHex(int bytecount, byte[] importarray, int offsetinarray)
        {
            byte[] tempbytearr = new byte[bytecount];
            for (int i = 0; i <= bytecount - 1; i++)
            {
                tempbytearr[i] = importarray[offsetinarray];
                offsetinarray += 1;
            }

            return tempbytearr;
        }

        static int ParseOffset(byte[] input, int adr)
        {
            string first = input[adr + 3].ToString("X").ToString();
            string second = input[adr + 2].ToString("X").ToString();
            string third = input[adr + 1].ToString("X").ToString();
            string forth = input[adr].ToString("X").ToString();

            int firstint = Convert.ToInt32(input[adr + 3]);
            int secondint = Convert.ToInt32(input[adr + 2]);
            int thirdint = Convert.ToInt32(input[adr + 1]);
            int forthint = Convert.ToInt32(input[adr]);

            string resultparse = first;

            if (secondint < 16) { resultparse = resultparse + 0; resultparse = resultparse + second; } else { resultparse = resultparse + second; }
            if (thirdint < 16) { resultparse = resultparse + 0; resultparse = resultparse + third; } else { resultparse = resultparse + third; }
            if (forthint < 16) { resultparse = resultparse + 0; resultparse = resultparse + forth; } else { resultparse = resultparse + forth; }


            int resultint = int.Parse(resultparse, System.Globalization.NumberStyles.HexNumber);

            return resultint;
        }

        static byte[] ReturnToSender(int dec)
        {
            string Hex = dec.ToString("X");
            string Hex4 = null;

            if (Hex.Length % 2 == 1)
            {
                Hex4 = Hex4 + "0";
                Hex4 = Hex4 + Hex;
                Hex = null;
                Hex = Hex4;
            }

            byte[] HexArr2 = new byte[4];
            byte[] HexArr = new byte[4];
            int m = 0;

            if (Hex.Length < 8)
            {
                m = (8 - Hex.Length) / 2;
            }

            for (int i = 0; i < Hex.Length; i = i + 2)
            {
                HexArr[m] = Convert.ToByte(Hex.Substring(i, 2), 16);
                m += 1;
            }

            for (int i = 0; i < 4; i++)
                HexArr2[i] = HexArr[3 - i];

            return HexArr2;
        }

        public void AppendAllBytes(string path, byte[] bytes)
        {
            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void EditMissionBriefName(string path, string newName)
        {
            byte[] archive = File.ReadAllBytes(path);
            int missionsqmid = 0;
            string briefingName;
            int originalbriefingnamelg = 0;
            int briefinglocation = 0;
            int fileposition = 0;
            int MLdatastart = 9;

            List<FileArch> FileList = new List<FileArch>();
            var pattern = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var patternbriefing = new byte[] { 0x62, 0x72, 0x69, 0x65, 0x66, 0x69, 0x6E, 0x67, 0x4E, 0x61, 0x6D, 0x65, 0x3D, 0x22 };

            try
            {
                MLdatastart = archive.Locate(pattern, 0)[0] + 21;
            }
            catch
            {
                MessageBox.Show("Can't read pbo header.");
                return;
            }

            while (fileposition < MLdatastart - 21)
            {
                int wordlength = 0;

                while (archive[fileposition + wordlength] != 0x00)
                {
                    wordlength++;
                }

                if (wordlength != 0)
                    FileList.Add(new FileArch(Encoding.UTF8.GetString(TempArrayHex(wordlength, archive, fileposition)), ParseOffset(archive, fileposition + wordlength + 17), fileposition + wordlength + 17));

                fileposition += wordlength + 21;
            }

            MLdatastart = 0;

            foreach (FileArch Filoo in FileList)
            {
                MLdatastart += Filoo.Filename.Length + 21;
            }

            MLdatastart += 21;

            for (int i = 0; i < FileList.Count; i++)
            {
                if (FileList[i].Filename != "mission.sqm")
                {
                    MLdatastart += FileList[i].Filesize;
                }
                else
                {
                    missionsqmid = i;
                    break;
                }
            }

            try
            {
                briefinglocation = archive.Locate(patternbriefing, MLdatastart)[0] + 14;
            }
            catch
            {
                MessageBox.Show("Can't find briefing name. Mission is either binarized or doesn't have any briefing name.");
                return;
            }

            while (archive[briefinglocation + originalbriefingnamelg] != 0x22 && archive[briefinglocation + originalbriefingnamelg + 1] != 0x3B)
            {
                originalbriefingnamelg++;
            }

            briefingName = Encoding.UTF8.GetString(TempArrayHex(originalbriefingnamelg, archive, briefinglocation));

            for (int i = 0; i < 4; i++)
                archive[FileList[missionsqmid].Descpos + i] = ReturnToSender(FileList[missionsqmid].Filesize + newName.Length)[i];

            MessageBox.Show("Old briefing name - " + briefingName + ".\n" + "New briefing name - " + briefingName + newName + ".");

            string tempPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Path.GetFileName(path);

            try
            {
                File.WriteAllBytes(tempPath, TempArrayHex(briefinglocation + originalbriefingnamelg, archive, 0));
                AppendAllBytes(tempPath, Encoding.UTF8.GetBytes(newName));
                AppendAllBytes(tempPath, TempArrayHex(archive.Length - briefinglocation - originalbriefingnamelg, archive, briefinglocation + originalbriefingnamelg));
            }
            catch (Exception e)
            {
                MessageBox.Show("Writing error.\n" + e);
                return;
            }

            Upload(tempPath);
        }

        public void Upload(string file)
        {
            try
            {
                WebClient Client = new WebClient();

                byte[] result = Client.UploadFile("http://dedick.podkolpakom.net/arma/upload/upload.php?password=" + password_textBox.Text + "&version=" + version, file);

                string webReturn = System.Text.Encoding.UTF8.GetString(result, 0, result.Length);

                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

                htmlDoc.LoadHtml(webReturn);
                webReturn = htmlDoc.DocumentNode.InnerText;

                MessageBox.Show(webReturn);
            }
            catch (Exception e)
            {
                MessageBox.Show("Upload error.\n" + e.Message);
            }

            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                MessageBox.Show("Writing error.\n" + e);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select mission file";
            selectFile.Filter = "Executable File (.pbo) | *.pbo";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                EditMissionBriefName(selectFile.FileName, " " + DateTime.Now.ToString("yyyyMMddHmmss"));
            }
        }
    }
}
