   M S B A M L   `   `   `  ����G  CSocketClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SocketClientSocketClient  5   	       SocketClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:SocketClient     5   	     � Title$  
MainWindow��5   	   $	��400��6   $	��600��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��80 �6   �� 5      $��* �6   �� 5      $��50 �6   �� 5      $��50 �6   .+�5   
   ]� ��.  $��Center=�6*   $
��20,0q�6E   .+�5      �� $
��Wrap=�6   $	��IP:��6,   $��Center=�67   �� 5      -   5         ��Name$ txtIP��5      $	��120��6%    �� TextWrapping$
 Wrap=�61   $��	127.0.0.1��6E   $��Center=�6V   $
��10,0q�6q   �� 5      $
��Wrap=�6   $��Port:��6,   $��Center=�69   $��20,0,0,0q�6T   �� 5      -   5        $ txtPort��5      $	��120��6'   $
 Wrap=�63   $
��3000��6G   $��Center=�6S   $
��10,0q�6n   �� 5      -   5         ��Name$ btnStartServer��5      $��连接服务��6-   $
��30,0q�6X   5      �� 5   
   -   5         ��Name$ lbInfo��5      $��1��6#   $
��20,0q�60   ]� 5   
   $��2��6   $
��20,0q�6!   .+�5      �� -   5        $ btnSend��5      $��发送消息��6&   $��10,0,0,0q�65   $
��Left=�6G   $��Center=�6b   $��75��6~   $��Right=�6�   �� 5      -   5        $ txtInput��5      $��23��6(   $
 Wrap=�64   $�� ��6H   $��Center=�6P   5      ]� 5   
   $��3��6   $
��20,0q�6!   .+�5      �� -   5        $ btnSendFile��5      $��发送文件��6*   $
��Left=�69   $��Center=�6T   $��75��6p   $��Right=�6�   �� 5      -   5        $ btnSelectFile��5      $��选择文件��6,   $
��10,0q�6<   $
��Left=�6K   $��Center=�6f   $��75��6�   $��Right=�6�   �� 5      -	   5        $ txtPath��5      $��23��6'   $
 Wrap=�63   $�� ��6G   $��Center=�6O    �� 
IsReadOnly . 6j   5       5!      5"      