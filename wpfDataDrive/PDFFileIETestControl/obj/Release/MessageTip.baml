   M S B A M L   `   `   `  ���� O  KPDFFileIETestControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ShareControlShareControl      ShareControl.MessageTip   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     (sclr-namespace:ShareControl     -     � Title$   ��$	��216��$	��350�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=� � AllowsTransparency .  � Icon$ Images/Share.ico��
����������#aabed1$��1q� � 	Resources  �� 
TargetType   formLabelStyle �� Property	 �� Value��%n   &	        �� " ��.���  � ��$	 Right=���  �@��Padding$	 0 0 15 8q���  � ��$	 Center=�.��� 
 � RowDefinitions
 �� $��32 ��� $��* ��� $
��auto � � ColumnDefinitions �� $��* ��� $��3* �.+��� ���#4592f0 �� Orientation$ 
Horizontal=�$��2��.+��� -    ��Name$ title�����#4592f0$
��Left=�$	��276��$��15(�
�������$
��16 0q�   ShareControl.ShareButton  -     Name$ btnCloseWindow��   ShareControl.ShareButton   Click$ btnCloseWindow_Click��  	windowBtn#��[ $��	10,4,10,0q�$��Right=�   	MoveBrush ��  �� ImageSource$ images/xm.png��   
EnterBrush �� $ images/xe.png������ $ images/xn.png���� $��1���� -    ��Name$ ContentContainer��$��1��$��2��$��Center=�.+��� -   $ content��$��Center=�$��16(��� $��2��$��2��$ 
Horizontal=�$��Center=�$��Bottom=�$��0 0 0 23q�.+��� -    ��Name$ btnmakeSure��$��0 0 12 0q�  SummitButton#��[ $��Center=��� -   $ 	btnCancel��  CancelButton#��[ $��Center=�