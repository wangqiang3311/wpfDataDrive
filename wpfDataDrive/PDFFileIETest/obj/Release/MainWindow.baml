   M S B A M L   `   `   `  ���� O  KPDFFileIETestControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI8clr-namespace:ShareControl;assembly=PDFFileIETestControlShareControl   PDFFileIETest   PDFFileIETest.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Fs8clr-namespace:ShareControl;assembly=PDFFileIETestControl     -     � Title$  pdf批量测试��$	��550��$	��740�� � UseLayoutRounding .  � 
ResizeMode$ CanResizeWithGrip=� � AllowsTransparency .   � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� J� 	AllowDrop . ���#aabed1$��0q� J� SnapsToDevicePixels . .���  � RowDefinitions �� $
��auto ��� .+��� 	 �� Orientation$	 
Horizontal=�.+��� -   
 ��Name$
 	InitTable��$��	创建表��$��0 0 10 0q��� -   $
 ReadFile��$��读取pdf文件��$��0 0 10 0q��� -    ��Name$ title���� -   $
 	StartTest��$��开启测试��$��0 0 10 0q��� -   $
 StartTestPart��$��开启部分测试��$��0 0 10 0q��� -   $

 Stop��$��暂停��$��0 0 10 0q��� -   $
 ReStart��$��重新开始��$��0 0 10 0q��� -	   $
 Learn��$��机器学习��$��0 0 10 0q��� .��通过数：�� -
    ��Name$ 	PassCount������ *	PassCount+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� .��未通过数：�� -   $ UnpassCount������ *UnPassCount+$ TwoWay=�$ PropertyChanged=��� .��空白数：�� -   $ UnpassEmptyCount������ *UnpassEmptyCount+$ TwoWay=�$ PropertyChanged=��� .��标题不完整数：�� -   $ UnpassHalfCount������ *UnpassHalfCount+$ TwoWay=�$ PropertyChanged=��� .��#!已经定位到标题范围数：�� -   $ LocationTitleRange������ *LocationTitleRange+$ TwoWay=�$ PropertyChanged=��� .��定位到但未找出数：�� -   $ CannotFindCount������ *CannotFindCount+$ TwoWay=�$ PropertyChanged=��� .��通过率：�� -   $ PassRate������ *
PassRate+$ TwoWay=�$ PropertyChanged=��� .��除去图片的通过率：�� -   $ ExceptImagePassRate������ *ExceptImagePassRate+$ TwoWay=�$ PropertyChanged=��� .��开始时间：�� -   $ 	StartTime������ *StartTimeFormat+$ TwoWay=�$ PropertyChanged=��� .��结束时间：�� -   $ EndTime������ *EndTimeFormat+$ TwoWay=�$ PropertyChanged=��� .��运行时长：�� -   $ 
DuringTime������ *
DuringTime+$ TwoWay=�$ PropertyChanged=�(   "ShareControl.FullTextDBUserControl  -     Name$ fulltextDBControl��$��1��