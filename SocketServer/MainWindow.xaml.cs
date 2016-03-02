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

namespace SocketServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socketWatch = null;
        Socket socketCommunication = null;
        Dictionary<string, Socket> dicSocket;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            if (socketWatch == null)
            {
                dicSocket = new Dictionary<string, Socket>();   //客户端socket字典
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //监听socket
                try
                {
                    IPAddress address = IPAddress.Parse(txtIP.Text.Trim());
                    IPEndPoint endPoint = new IPEndPoint(address, int.Parse(txtPort.Text.Trim()));
                    socketWatch.Bind(endPoint);

                    socketWatch.Listen(20);   //限制20个客户端
                    Listen();
                    ShowMsg("服务器启动完成！");
                }
                catch (Exception er)
                {
                    ShowMsg("服务器启动异常：" + er.ToString());
                }
            }
        }

        private void ShowMsg(string msg)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                lbInfo.Items.Add(msg);
            }));

        }

        private async void Listen()
        {
            await Task.Run(new Action(() =>
            {
                while (true)
                {
                    socketCommunication = socketWatch.Accept();    //监听并创建通讯socket
                    Receive(socketCommunication);                  //接收数据
                    dicSocket.Add(socketCommunication.RemoteEndPoint.ToString(), socketCommunication);   //加入客户端socket字典

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        lbClientList.Items.Add(socketCommunication.RemoteEndPoint.ToString());
                    }));

                    ShowMsg("客户端连接成功！通信地址为：" + socketCommunication.RemoteEndPoint.ToString());
                }
            }));
        }

        private async void Receive(Socket socket)
        {
            await Task.Run(new Action(() =>
            {
                while (true)
                {
                    byte[] bytes = new byte[1024 * 1024 * 2];
                    try
                    {
                        int length = socket.Receive(bytes);   //接收数据

                        if (bytes[0] == 0)                    //约定客户端传输数据字节首位为0是文件
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            if (sfd.ShowDialog() == true)
                            {
                                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                                {
                                    fs.Write(bytes, 1, length - 1);
                                    fs.Flush();
                                    ShowMsg("文件保存成功，路径为：" + sfd.FileName);
                                }
                            }
                        }
                        else
                        {
                            string msg = Encoding.UTF8.GetString(bytes, 0, length);
                            ShowMsg("接收到来自" + socket.RemoteEndPoint.ToString() + "的数据：" + msg);
                        }
                    }
                    catch (SocketException ex)
                    {
                        ShowMsg("出现异常：" + ex.Message);
                        string key = socket.RemoteEndPoint.ToString();
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            lbClientList.Items.Remove(key);
                        }));
                        dicSocket.Remove(key);   //异常，移除客户端socket，并跳出循环线程
                        break;
                    }
                }
            }));
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (lbClientList.SelectedIndex > -1)
            {
                try
                {
                    string msg = txtInput.Text.Trim();
                    dicSocket[lbClientList.SelectedItem.ToString()].Send(Encoding.UTF8.GetBytes(msg));   //给指定客户端发送数据
                    ShowMsg("发送数据：" + msg);
                }
                catch (Exception er)
                {
                    ShowMsg("发送数据异常：" + er.ToString());
                }
            }
            else
            {
                ShowMsg("请先选择需要通讯的客户端！");
            }
        }

        private void btnSendAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = txtInput.Text.Trim();
                foreach (var socket in dicSocket.Values)    //给所有客户端发送数据
                {
                    socket.Send(Encoding.UTF8.GetBytes(msg));
                }
                ShowMsg("群发消息：" + msg);
            }
            catch (Exception er)
            {
                ShowMsg("发送数据异常：" + er.ToString());
            }
        }
    }
}
