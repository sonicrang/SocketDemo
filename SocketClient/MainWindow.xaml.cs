using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socketClient = null;   //通讯socket
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = txtInput.Text.Trim();
                if (socketClient != null && !string.IsNullOrEmpty(msg))
                {
                    socketClient.Send(Encoding.UTF8.GetBytes(msg));  //发送数据
                    ShowMsg("发送数据：" + msg);
                }
            }
            catch (Exception er)
            {
                ShowMsg("发送数据异常：" + er.ToString());
            }

        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                txtPath.Text = ofd.FileName;
            }
        }

        private void btnSendFile_Click(object sender, RoutedEventArgs e)
        {
            if (socketClient != null && !string.IsNullOrEmpty(txtPath.Text) && File.Exists(txtPath.Text))
            {
                try
                {
                    using (FileStream fs = new FileStream(txtPath.Text, FileMode.Open))
                    {
                        byte[] bytes = new byte[1024 * 1024 * 2];
                        int length = fs.Read(bytes, 0, bytes.Length);
                        byte[] newBytes = new byte[length + 1];
                        newBytes[0] = 0;           //约定首位为0是文件，所以newBytes长度是文件数据长度+1
                        Buffer.BlockCopy(bytes, 0, newBytes, 1, length);      //将文件数据拷贝到newBytes中，向后偏移1位，首位为0
                        socketClient.Send(newBytes);
                    }
                }
                catch (Exception er)
                {
                    ShowMsg("发送文件异常：" + er.ToString());
                }
            }
        }

        private async void Recive()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        byte[] bytes = new byte[1024 * 1024 * 2];
                        int length = socketClient.Receive(bytes);
                        string msg = Encoding.UTF8.GetString(bytes, 0, length);
                        ShowMsg("接收到数据：" + msg);
                    }
                    catch (Exception er)
                    {
                        ShowMsg("接收数据异常：" + er.ToString());
                        break;
                    }
                }
            });
        }

        private void btnLinkServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (socketClient == null)
                {
                    socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketClient.Connect(IPAddress.Parse(txtIP.Text.Trim()), int.Parse(txtPort.Text.Trim()));
                    ShowMsg(string.Format("连接服务器（{0}:{1}）成功！", txtIP.Text.Trim(), txtPort.Text.Trim()));
                    Recive();
                }
            }
            catch (Exception er)
            {
                ShowMsg("连接服务器异常：" + er.ToString());
                socketClient = null;
            }
        }

        private void ShowMsg(string msg)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                lbInfo.Items.Add(msg);
            }));
        }
    }
}
