   M S B A M L   `   `   `  ����   ZFreeGo.Monitor.Test-    'ZFreeGo.Monitor.Test.Xmode.SerialPortUI   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       N� Title$  SerialPortUI��.���  � RowDefinitions �� $
��Auto ��� $��2* ��� $��* � � ColumnDefinitions �� $��* �.+��� $��0��$��0��$��串口通讯��
�������.���� .+�� -   $��	commAGrid�� �� $��50 ��� $��80 ��� $��50 ��� $��80 ��� $��50 ��� $��80 � �� $��30 ��� $��30 �.+��� $��1q�$��0��$��0��$��Center=�.��
串口号:�� -   $��portName��$��1��$��0��$��30��$��22��$��Center=��� $��2q�$��1q�$��2��$��0��$��Center=�.��
波特率:�� -   $��baudRate��$��3��$��0��$��30��$��22��$��Center=��� $��2q�$��1q�$��4��$��0��$��Center=�.��
数据位:�� -   $��dataBits��$��5��$��0��$��30��$��22��$��Center=��� $��2q�$��1q�$��0��$��1��$��Center=�.��
校验位:�� -   $��
portParity��$��1��$��1��$��30��$��22��$��Center=��� $��2q�$��1q�$��2��$��1��$��Center=�.��
停止位:�� -   $��stopBits��$��3��$��1��$��30��$��22��$��Center=��� -   $��openSerialPort��$��2q�$��3q�$��Center=�$��Center=�.��打开串口�� $��0��$��1��$��接收数据��
�������.���  �� �� $��70 �.+��� -   $��	txtRecive��$��3q�$��0��" �� VerticalScrollBarVisibility$
 Auto=�$ �� HorizontalScrollBarVisibility$
 Auto=�.���� $��1��.+��� $��3q�.+�	� -	   $��
radioASCII��.��ASCII	� -
   $��radioBinary��.��BINARY�� -   $��btnClrearRecive��$��3,20,3,3q�.��清空�� $��0��$��2��$��发送��
�������.���  �� �� $��70 �.+��� -   $��txtSend��$��3q�$��0��$
 Auto=�$
 Auto=�.���� $��1��.+��� -   $��btnSend��$��3q�.��发送�� -   $��btnClearTxt��$��3,20,3,3q�.��清空