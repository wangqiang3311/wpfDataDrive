   M S B A M L   `   `   `  ���� O  KPDFFileIETestControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ShareControlShareControl  +clr-namespace:ShareControlShareControl  (    "ShareControl.FullTextDBUserControl   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (sclr-namespace:ShareControl     (uclr-namespace:ShareControl       � 	Resources     CheckboxDataTemplate1 ��Name d� IsThreeState d� 	IsChecked �� Mode �� UpdateSourceTrigger0   *ShareControl.cvtfulltextOperImageConverter  cvtfulltextOperImage/   )ShareControl.CellBackGroundColorConverter  gridCellcvtColor  
pagerStyle �� 
TargetType �� Property �� Value��%F  &	        &	 �     &	 �     &	 �     �� .R�� .+��� $ chkSelected��-   $��Center=�$��Center=�$��Center=� .   �� *
IsSelected+$ TwoWay=�$ PropertyChanged=�    �� " ��.���  � ��$ 13(���  � ��
 �   ���  � ��$ microsoft yahei,宋体)���  � ��$ Center=�.��� -   	 �Name$	 fulltextGridContainer��
 � RowDefinitions
 �� $��40 ��� �� $	��120 �.+��� ���#edf1fa �� Orientation$ 
Horizontal=�.+��� ���#acd7ea$��1q�$��18 5 0 5q�$��Center=�._��� -    ��Name$ btnBachDelete��  btnVisitDBControl#��[ .���� $ 
Horizontal=�.+��� $��images/deletebat.png��$��16��$��16��$��Center=��� $��批量删除�� �� Padding$ 5q�$��13(��� ���#acd7ea$��1q�$��16 5 0 5q�$��Center=�._��� -   $ 
btnAddTask��#��[ .���� $ 
Horizontal=�.+��� $��images/add.png��$��16��$��16��$��Center=��� $��添加测试��$ 5q�$��13(��� ���#acd7ea$��1q�$��16 5 0 5q�$��Center=�._��� -   $ 	btnSearch��#��[ .���� $ 
Horizontal=�.+��� -    ��Name$ searchContent��$��5q�$	��150��$��13(����� *	KeyWord+$ TwoWay=�$ PropertyChanged=��� $��搜索�� � ToolTip$! 按标题或者状态搜索��$ 5q�$��13(��� ���#acd7ea$��1q�$��16 5 0 5q�$��Center=�._��� $ 
Horizontal=�.+��� -    ��Name$ cbxOrder��$	��100�� �� 
IsEditable .  �� DisplayMemberPath$ 	FiledName�� �� SelectedValuePath$ 	FiledName�� �� IsTextSearchEnabled .  �� SelectedValue �� *	FiledName+$ TwoWay=�$ PropertyChanged=�&   System.Windows.Controls.DataGrid  -     Name$ fullTaskDataGrid��$��1��   HeadersVisibility$ Column=�   AutoGenerateColumns .     CanUserAddRows .  $	��Top=�����    Columns 4  .System.Windows.Controls.DataGridCheckBoxColumn  -	   _dtcSelected��,  &System.Windows.Controls.DataGridColumn   Header$  �� & "DataGridCheckboxColumnHeaderStyle1   HeaderStyle# [   DataGridCheckboxCellStyle1   	CellStyle# [    Width5  /System.Windows.Controls.DataGridLengthConverter$ 40    CanUserReorder .      CanUserResize  .  0  *System.Windows.Controls.DataGridTextColumn  $ 标题��$ 6*   documentColumnHead# [   documentCellStyle# [ 1  +System.Windows.Controls.DataGridBoundColumn!   Binding! �� *Title+$ TwoWay=�$ PropertyChanged=�  $ 作者��$ 1* ! �� *AuthorDisplay+$ TwoWay=�$ PropertyChanged=�  $	 卷��$
 1.5* ! �� *Volume+$ OneWay=�$ PropertyChanged=�  $	 期��$ * ! �� *Issue+$ TwoWay=�$ PropertyChanged=�  $ 页码��$
 1.5* ! �� *	PageScope+$ TwoWay=�$ PropertyChanged=�  $	 URL��$
 1.5* ! �� *URL+$ TwoWay=�$ PropertyChanged=�  $	 DOI��$
 1.5* ! �� *DOI+$ TwoWay=�$ PropertyChanged=�  $
 Year��$
 1.5* ! �� *Year+$ TwoWay=�$ PropertyChanged=�  $
 ISSN��$
 1.5* ! �� *ISSN+$ TwoWay=�$ PropertyChanged=�  $ 路径��$
 3.5* ! �� *
FilePath+$ TwoWay=�$ PropertyChanged=�  $ 抽取的标题��$
 3.5* ! �� *	IETitle+$ TwoWay=�$ PropertyChanged=�  $ 测试结果��$
 3.5* ! �� *
TestResult+$ TwoWay=�$ PropertyChanged=�4	  .System.Windows.Controls.DataGridTemplateColumn	  $ 操作��$ 3*  	 OperCellStyle# [	 " 	  CellTemplate" �� .R��� $ 
Horizontal=�.+�
   ShareControl.ShareButton
  # 
 Name$# btnOpenFullTextPath��   ShareControl.ShareButton$   Click$ btnOpenFullTextPath_Click$��20��$��20��$��0 0 13 0q�$ 打开全文所在路径������ % �� ImageSource% �� *CanOpenFullTextPath+& �� 	Converter#& [ ' �� ConverterParameter$' 00��( 
  	MoveBrush( �� % �� *CanOpenFullTextPath+#& [ $' 01��) 
  
EnterBrush) �� % �� *CanOpenFullTextPath+#& [ $' 01��
  $# 	btnDelete��$ btnDelete_Click$��20��$��20��$ 删除��$��0 0 13 0q����� % �� *	CanDelete+#& [ $' 20��( �� % �� *	CanDelete+#& [ $' 21��) �� % �� *	CanDelete+#& [ $' 21��
  $# btnReSummit��$ btnReSummit_Click$��20��$��20��$ 更新标题������ % �� *CanReSummit+#& [ $' 30��( �� % �� *CanReSummit+#& [ $' 31��) �� % �� *CanReSummit+#& [ $' 31���� -
   * ��Name$* fulltextPager��$ 
Horizontal=�$��Center=�$	��Top=�$��2��$
��0 20q�.+��� -   $ prePage�� 
 btnPager#��[
 $ 	上一页���� -   $ bntGoFirstPage��#��[
 $��1���� -   + ��Name$+ predot��$	��...��y��� *
PreVisible+�� -   , ��Name$, btnPagerContainer������ *Pages+���� .R�� .+��� -   #��[
 ���� *Name+���� .R�� - � Orientation$- 
Horizontal=��� -   $+ nextdot��$	��...��y��� *NextVisible+�� -   $ btnGoLastPage��#��[
 ���� *Total+�� -   $ nextPage��$��>>��#��[
 $ 	下一页���� $��当前���� ���#3091f2���� *	PageIndex+�� $	��页���� $��	跳转到��#��[ $��5 0 5 0q��� -   $ wantToGo��$��50��$��25���� $	��页��#��[ �� #��[ �� -   $ goPage��$��go��#��[
 �� #��[ �� $	��共�� �� ���#3091f2���� *	ItemCount+ �� $	��条��