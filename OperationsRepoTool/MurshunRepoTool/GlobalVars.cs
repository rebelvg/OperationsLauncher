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
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;

namespace OperationsLauncherServer
{
    public partial class Form1 : Form
    {
        List<string> presetModsList = new List<string>();

        OperationsLauncherXmlSettings LauncherSettings;

        string server = "";
        string password = "";
        string verifyModsLink = "";
        string verifyModsPassword = "";
        string missionsLink = "";
    }
}
