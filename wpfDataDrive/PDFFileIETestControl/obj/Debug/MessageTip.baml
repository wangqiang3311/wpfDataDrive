   M S B A M L   `   `   `  ����O  KPDFFileIETestControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ShareControlShareControl  5   	       ShareControl.MessageTip   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   (sclr-namespace:ShareControl     5   	   -   5          � Title$   ��5   	   $	��216��6   $	��350��6"    � WindowStartupLocation$ CenterOwner=�6/    � 
ResizeMode$ NoResize=�6U    � WindowStyle$
 None=�6l    � AllowsTransparency .  � Icon$ Images/Share.ico��6�   
����������#aabed1$��1q�6�    � 	Resources 5       �� 
TargetType   formLabelStyle �� Property	 �� Value��%�   &	        �� 5	   
   " ��6   .�5
      ��  � ��$	 Right=�64   �� 5       �@��Padding$	 0 0 15 8q�6(   �� 5       � ��$	 Center=�62   5      5      .��5      � 
 � RowDefinitions
 5   
   �� 5      $��32 �6   �� 5      $��* �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��3* �6   .+�5   
   �� ���#4592f0 �� Orientation$ 
Horizontal=�6*   $��2��6C   .+�5!      �� -   5         ��Name$ title��5!   .   ���#4592f0$
��Left=�6>   $	��276��6Y   $��15(�6e   
�������$
��16 0q�6�      ShareControl.ShareButton  5#      -   5          Name$ btnCloseWindow��5#         ShareControl.ShareButton   Click$ btnCloseWindow_Click��6�     	windowBtn#��[ 66   $��	10,4,10,0q�6Y   $��Right=�6l      	MoveBrush 5$      �� 5%       �� ImageSource$ images/xm.png��6!      
EnterBrush 5(      �� 5)      $ images/xe.png��6!   ��5,      �� 5-      $ images/xn.png��6!   50      51      �� 54   
   $��1��6   �� 56   
   -   5         ��Name$ ContentContainer��56   6   $��1��6   $��2��6"   $��Center=�6P   .+�57      �� -   5        $ content��57      $��Center=�6*   $��16(�6G   58      �� 5:   
   $��2��6   $��2��6$   $ 
Horizontal=�69   $��Center=�6R   $��Bottom=�6o   $��0 0 0 23q�6�   .+�5;      �� -   5         ��Name$ btnmakeSure��5;      $��0 0 12 0q�6+     SummitButton#��[ 6>   $��Center=�6d   �� 5<      -   5        $ 	btnCancel��5<        CancelButton#��[ 6*   $��Center=�6P   5=      5?      5@      