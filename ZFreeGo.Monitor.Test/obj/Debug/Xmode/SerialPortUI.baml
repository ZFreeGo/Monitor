   M S B A M L   `   `   `  ����  ZFreeGo.Monitor.Test-    'ZFreeGo.Monitor.Test.Xmode.SerialPortUI   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  SerialPortUI��5      .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��2* �6   6*   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   .+�5   
   �� $��0��6   $��0��6$   $��串口通讯��63   
�������.��5      �� .+�5      � -   5        $��	commAGrid��5       5      �� 5      $��50 �6+   68   �� 5      $��80 �6+   68   �� 5      $��50 �6+   68   �� 5      $��80 �6+   68   �� 5      $��50 �6+   68   �� 5      $��80 �6+   68    5       �� 5!      $��30 �6(   66   �� 5"      $��30 �6(   66   .+�5%      �� $��1q�6   $��0��6)   $��0��69   $��Center=�6F   .��6a   
串口号:6g   �� 5&      -   5        $��portName��5&      $��1��61   $��0��6A   $��30��5'      $��22��6%   $��Center=�61   6U   �� 5(      $��2q�6   $��1q�6(   $��2��64   $��0��6D   $��Center=�6Q   .��6l   
波特率:6r   �� 5)      -   5        $��baudRate��5)      $��3��60   $��0��6@   $��30��5*      $��22��6%   $��Center=�61   6U   �� 5+      $��2q�6   $��1q�6(   $��4��64   $��0��6D   $��Center=�6Q   .��6l   
数据位:6r   �� 5,      -   5        $��dataBits��5,      $��5��61   $��0��6A   $��30��5-      $��22��6%   $��Center=�61   6U   �� 5.      $��2q�6   $��1q�6(   $��0��64   $��1��6D   $��Center=�6Q   .��6l   
校验位:6r   �� 5/      -   5        $��
portParity��5/      $��1��63   $��1��6C   $��30��50      $��22��6%   $��Center=�61   6U   �� 51      $��2q�6   $��1q�6(   $��2��64   $��1��6D   $��Center=�6Q   .��6l   
停止位:6r   �� 52      -   5        $��stopBits��52      $��3��60   $��1��6@   $��30��53      $��22��6%   $��Center=�61   6U   55      �� 56      -   5        $��openSerialPort��56      $��2q�6/   $��3q�6;   $��Center=�6G   $��Center=�57      .��6L   打开串口6R   58      59      �� 5:   
   $��0��6   $��1��6$   $��接收数据��63   
�������.��5;      �  5<      �� 5=      6*   �� 5>      $��70 �6'   64   .+�5A      �� -   5        $��	txtRecive��5A   %   $��3q�6   $��0��66   " �� VerticalScrollBarVisibility$
 Auto=�6G   $ �� HorizontalScrollBarVisibility$
 Auto=�5B      .��6?   5D      �� 5E      $��1��6   .+�5F      �� $��3q�6!   .+�5G      	� -	   5        $��
radioASCII��5G   &   .��6U   ASCII6\   	� 5H      -
   5        $��radioBinary��5H   &   .��6W   BINARY6_   5I      �� 5J      -   5        $��btnClrearRecive��5J   /   $��3,20,3,3q�6   .��6d   清空6h   5K      5L      5N      �� 5O   
   $��0��6   $��2��6$   $��发送��63   
�������.��5P      �  5Q      �� 5R      6*   �� 5S      $��70 �6'   64   .+�5V      �� -   5        $��txtSend��5V   %   $��3q�6   $��0��64   $
 Auto=�6E   $
 Auto=�5W      .��6?   5Y      �� 5Z      $��1��6   .+�5[      �� -   5        $��btnSend��5[   (   $��3q�6   .��6M   发送6Q   �� 5\      -   5        $��btnClearTxt��5\   /   $��3,20,3,3q�6   .��6`   清空6d   5]      5^      5_      5`      5a      