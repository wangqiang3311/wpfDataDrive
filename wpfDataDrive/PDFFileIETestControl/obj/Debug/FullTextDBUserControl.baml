   M S B A M L   `   `   `  ����O  KPDFFileIETestControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ShareControlShareControl  5      +clr-namespace:ShareControlShareControl  5      (    "ShareControl.FullTextDBUserControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (sclr-namespace:ShareControl     5      (uclr-namespace:ShareControl     5        � 	Resources  5
         CheckboxDataTemplate1 ��Name d� IsThreeState d� 	IsChecked �� Mode �� UpdateSourceTrigger0   *ShareControl.cvtfulltextOperImageConverter  cvtfulltextOperImage/   )ShareControl.CellBackGroundColorConverter  gridCellcvtColor  
pagerStyle �� 
TargetType �� Property �� Value��%  &	        &	 �     &	     &	     �� 5   
   .R�5      � .+�5      �� $ chkSelected��6   -   5        $��Center=�5      $��Center=�5      $��Center=�5       .   5      �� *
IsSelected+$ TwoWay=�$ PropertyChanged=�5      5        5   
   6*     5   	   6(   �� 5!   
   " ��6#   .�5#      ��  � ��$ 13(�6*   �� 5$       � ��
 �   �6,   �� 5%       � ��$ microsoft yahei,宋体)�6+   �� 5&       � ��$ Center=�63   5(      5+      .��5.      � -   5        	 �Name$	 fulltextGridContainer��5.      
 � RowDefinitions
 50   
   �� 51      $��40 �6   �� 52      �� 53      $	��120 �6   .+�56   
   �� ���#edf1fa �� Orientation$ 
Horizontal=�6*   .+�57      �� ���#acd7ea$��1q�6+   $��18 5 0 5q�6?   $��Center=�6R   ._�5:      �� -   5         ��Name$ btnBachDelete��5:   E     btnVisitDBControl#��[ 6   .��5<      �� $ 
Horizontal=�6!   .+�5>      �� $��images/deletebat.png��6    $��16��6>   $��16��6I   $��Center=�6U   �� 5?      $��批量删除��6$    �� Padding$ 5q�61   $��13(�6=   5@      5A      5C      �� 5E      ���#acd7ea$��1q�6+   $��16 5 0 5q�6?   $��Center=�6R   ._�5H      �� -   5        $ 
btnAddTask��5H   E   #��[ 6   .��5J      �� $ 
Horizontal=�6!   .+�5L      �� $��images/add.png��6    $��16��68   $��16��6C   $��Center=�6O   �� 5M      $��添加测试��6$   $ 5q�61   $��13(�6=   5N      5O      5Q      �� 5T      ���#acd7ea$��1q�6+   $��16 5 0 5q�6?   $��Center=�6R   ._�5W      �� -   5        $ 	btnSearch��5W   E   #��[ 6   .��5Y      �� $ 
Horizontal=�6!   .+�5[      �� -   5         ��Name$ searchContent��5[   :   $��5q�6"   $	��150��6.   $��13(�6Q   ��6_   �� *	KeyWord+$ TwoWay=�$ PropertyChanged=��� 5]      $��搜索��6$    � ToolTip$! 按标题或者状态搜索��6.   $ 5q�6C   $��13(�6O   5^      5_      5a      �� 5d      ���#acd7ea$��1q�6+   $��16 5 0 5q�6?   $��Center=�6R   ._�5g      �� $ 
Horizontal=�6!   .+�5i      �� -   5         ��Name$ cbxOrder��5i   A   $	��100��6#    �� 
IsEditable .  �� DisplayMemberPath$ 	FiledName��6S    �� SelectedValuePath$ 	FiledName��6s    �� IsTextSearchEnabled .  �� SelectedValue 6�   �� *	FiledName+$ TwoWay=�$ PropertyChanged=�5j   G   5m      5p      5s      &   System.Windows.Controls.DataGrid  5u   
   -   5          Name$ fullTaskDataGrid��5u      $��1��6c      HeadersVisibility$ Column=�6p      AutoGenerateColumns .     CanUserAddRows .  $	��Top=�6�   ��6�   ��    Columns 5v      4  .System.Windows.Controls.DataGridCheckBoxColumn  5w      -	   5        _dtcSelected��,  &System.Windows.Controls.DataGridColumn   Header$  ��5y       & "DataGridCheckboxColumnHeaderStyle1   HeaderStyle# [ 5z        DataGridCheckboxCellStyle1   	CellStyle# [ 5{         Width5  /System.Windows.Controls.DataGridLengthConverter$ 40 5|         CanUserReorder .      CanUserResize  .  6;   0  *System.Windows.Controls.DataGridTextColumn  5      $ 标题��6%   $ 6* 62     documentColumnHead# [ 6�     documentCellStyle# [ 6�   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6=   �� *Title+$ TwoWay=�$ PropertyChanged=�6�     5�      $ 作者��6%   $ 1* 62   ! 6=   �� *AuthorDisplay+$ TwoWay=�$ PropertyChanged=�  5�      $	 卷��6%   $
 1.5* 61   ! 6>   �� *Volume+$ OneWay=�$ PropertyChanged=�  5�      $	 期��6%   $ * 61   ! 6;   �� *Issue+$ TwoWay=�$ PropertyChanged=�  5�      $ 页码��6%   $
 1.5* 61   ! 6?   �� *	PageScope+$ TwoWay=�$ PropertyChanged=�  5�      $	 URL��6%   $
 1.5* 62   ! 6@   �� *URL+$ TwoWay=�$ PropertyChanged=�  5�      $	 DOI��6%   $
 1.5* 62   ! 6@   �� *DOI+$ TwoWay=�$ PropertyChanged=�  5�      $
 Year��6%   $
 1.5* 63   ! 6A   �� *Year+$ TwoWay=�$ PropertyChanged=�  5�      $
 ISSN��6%   $
 1.5* 63   ! 6A   �� *ISSN+$ TwoWay=�$ PropertyChanged=�  5�      $ 路径��6%   $
 3.5* 62   ! 6?   �� *
FilePath+$ TwoWay=�$ PropertyChanged=�  5�      $ 抽取的标题��6%   $
 3.5* 65   ! 6B   �� *	IETitle+$ TwoWay=�$ PropertyChanged=�  5�      $ 测试结果��6%   $
 3.5* 64   ! 6A   �� *
TestResult+$ TwoWay=�$ PropertyChanged=�4	  .System.Windows.Controls.DataGridTemplateColumn	  5�      $ 操作��6)   $ 3* 65    	 OperCellStyle# [	 6@   " 	  CellTemplate" 5�      �� 5�      .R�5�      �� $ 
Horizontal=�6)   .+�5�   "   
   ShareControl.ShareButton
  # 
 Name$# btnOpenFullTextPath��60      ShareControl.ShareButton$   Click$ btnOpenFullTextPath_Click6�   $��20��6M   $��20��6X   $��0 0 13 0q�6g   $ 打开全文所在路径��6y   ��5�   &   �� 5�   *   % �� ImageSource% 65   �� *CanOpenFullTextPath+& �� 	Converter#& [ ' �� ConverterParameter$' 00��( 
  	MoveBrush( 5�   &   �� 5�   *   % 65   �� *CanOpenFullTextPath+#& [ $' 01��) 
  
EnterBrush) 5�   &   �� 5�   *   % 65   �� *CanOpenFullTextPath+#& [ $' 01��5�   #   
  5�   "   $# 	btnDelete��60   $ btnDelete_Click6}   $��20��6C   $��20��6N   $ 删除��6[   $��0 0 13 0q�6j   ��5�   &   �� 5�   *   % 65   �� *	CanDelete+#& [ $' 20��( 5�   &   �� 5�   *   % 65   �� *	CanDelete+#& [ $' 21��) 5�   &   �� 5�   *   % 65   �� *	CanDelete+#& [ $' 21��5�   #   
  5�   "   $# btnReSummit��60   $ btnReSummit_Click6m   $��20��6E   $��20��6P   $ 更新标题��6]   ��5�   &   �� 5�   *   % 65   �� *CanReSummit+#& [ $' 30��( 5�   &   �� 5�   *   % 65   �� *CanReSummit+#& [ $' 31��) 5�   &   �� 5�   *   % 65   �� *CanReSummit+#& [ $' 31��5�   #   5�      5�      5�      5�      �� 5�   
   -
   5        * ��Name$* fulltextPager��5�   �   $ 
Horizontal=�6   $��Center=�6.   $	��Top=�6K   $��2��6d   $
��0 20q�6q   .+�5�      �� -   5        $ prePage��5�       
 btnPager#��[
 6<   $ 	上一页��6^   �� 5�      -   5        $ bntGoFirstPage��5�   D   #��[
 6   $��1��68   6]   �� 5�      -   5        + ��Name$+ predot��5�      $	��...��6(   y�63   �� *
PreVisible+�� 5�      -   5        , ��Name$, btnPagerContainer��5�   9   ��6   �� *Pages+��5�      �� 5�      .R�5�      � .+�5�      �� -   5        #��[
 5�   %   ��6H   �� *Name+6b   5�      5�      ��5�      �� 5�      .R�5�      � - � Orientation$- 
Horizontal=�6$   5�      5�      �� 5�      -   5        $+ nextdot��5�      $	��...��6)   y�64   �� *NextVisible+�� 5�      -   5        $ btnGoLastPage��5�   R   #��[
 6   ��68   �� *Total+6j   �� 5�      -   5        $ nextPage��5�      $��>>��6>   #��[
 6K   $ 	下一页��6n   �� 5�      $��当前��6   �� 5�      ���#3091f2��6   �� *	PageIndex+63   �� 5�      $	��页��6   �� 5�      $��	跳转到��6   #��[ 6#   $��5 0 5 0q�6G   �� 5�      -   5        $ wantToGo��5�      $��50��6(   $��25��63   6A   �� 5�      $	��页��6   #��[ 6!   �� 5�      #��[ 6   �� 5�      -   5        $ goPage��5�   %   $��go��6   #��[
 6K   5�      �� 5�      #��[ 6   �� 5�      $	��共��6)    �� 5�      ���#3091f2��6)   �� *	ItemCount+6D    �� 5�      $	��条��6)   5      5     5     5     