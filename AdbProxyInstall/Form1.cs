using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AdbProxyInstall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string adbPath = folderBrowserDialog1.SelectedPath + "\\adb.exe";
                string adbWinApiPath = folderBrowserDialog1.SelectedPath + "\\AdbWinApi.dll";
                string adbWinUsbApi = folderBrowserDialog1.SelectedPath + "\\AdbWinUsbApi.dll";
                string adbProxyPath = folderBrowserDialog1.SelectedPath + "\\ADBProxy.exe";
                string adbProxyRegPath = folderBrowserDialog1.SelectedPath + "\\adbProxyReg.reg";
                string unInstallRegPath = folderBrowserDialog1.SelectedPath + "\\unInstallReg.reg";

                StringBuilder adbProxyRegSb = new StringBuilder("Windows Registry Editor Version 5.00\n");
                adbProxyRegSb.AppendLine("[HKEY_CLASSES_ROOT\\adbshell]");
                adbProxyRegSb.AppendLine("@=\"URL: adbshell Protocol Handler\"");
                adbProxyRegSb.AppendLine("\"URL Protocol\"=\"\"");
                adbProxyRegSb.AppendLine("[HKEY_CLASSES_ROOT\\adbshell\\DefaultIcon]");
                adbProxyRegSb.AppendLine("@=\"" + adbProxyPath.Replace("\\", "\\\\") + "\"");
                adbProxyRegSb.AppendLine("[HKEY_CLASSES_ROOT\\adbshell\\shell]");
                adbProxyRegSb.AppendLine("[HKEY_CLASSES_ROOT\\adbshell\\shell\\open]");
                adbProxyRegSb.AppendLine("[HKEY_CLASSES_ROOT\\adbshell\\shell\\open\\command]");
                adbProxyRegSb.AppendLine("@=\"\\\"" + adbProxyPath.Replace("\\", "\\\\") + "\\\" \\\"%1\\\"\"");
                string adbProxyReg = adbProxyRegSb.ToString();
                byte[] adbProxyRegData = System.Text.Encoding.Default.GetBytes(adbProxyReg);

                StringBuilder unInstallRegSb = new StringBuilder("Windows Registry Editor Version 5.00\n");
                unInstallRegSb.AppendLine("[-HKEY_CLASSES_ROOT\\adbshell]");
                string unInstallReg = unInstallRegSb.ToString();
                byte[] unInstallRegData = System.Text.Encoding.Default.GetBytes(unInstallReg);

                WriteByteToFile(Properties.Resources.adb, adbPath);
                WriteByteToFile(Properties.Resources.AdbWinApi, adbWinApiPath);
                WriteByteToFile(Properties.Resources.AdbWinUsbApi, adbWinUsbApi);
                WriteByteToFile(Properties.Resources.ADBProxy, adbProxyPath);
                WriteByteToFile(adbProxyRegData, adbProxyRegPath);
                WriteByteToFile(unInstallRegData, unInstallRegPath);

                System.Diagnostics.Process.Start("regedit.exe", "/s " + adbProxyRegPath);

                MessageBox.Show("安装成功");
                Application.Exit();
            }
        }

        /// <summary>
        /// 写byte[]到fileName
        /// </summary>
        /// <param name="pReadByte">byte[]</param>
        /// <param name="fileName">保存至硬盘路径</param>
        /// <returns></returns>
        public static bool WriteByteToFile(byte[] pReadByte, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            return true;
        }
    }
}
