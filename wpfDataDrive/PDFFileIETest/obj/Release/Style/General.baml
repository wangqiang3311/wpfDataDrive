   M S B A M L   `   `   `  ���� \  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� Property �� Value   btnBaseStyle �� 
TargetType ��Name J� Opacity �� CornerRadius ��Name J� BitmapEffect	 F� Color
 F� 	Direction F� ShadowDepth F� Softness F� Opacity �� Triggers P� Property P� Value N� EnterActions �� HandoffBehavior	 W� To g� Duration �� 
TargetName �� TargetProperty N� ExitActions	 �� To �� Triggers&     System.Windows.Controls.DataGrid �� Color=   7System.Windows.Controls.Primitives.DataGridColumnHeader  ColumnBaseStyle �� BasedOn)   #System.Windows.Controls.DataGridRow*   $System.Windows.Controls.DataGridCell  CellBaseStyle J� SnapsToDevicePixels
 � Tag � ToolTip �� RelativeSource  �� Path ( $DataGridCheckboxCellControlTemplate1! �� ContentStringFormat" �� ContentStringFormat# }� Location  	windowBtn  ComboxStyleBtn$ J� RenderTransformOrigin% �� Stretch& �� Angle' �� 
TargetName  ComboBoxStyle( � ColumnDefinitions) ��Name* �� 
IsReadOnly+ �� 
IsReadOnly, �� Mode- �� UpdateSourceTrigger. �� 	ClickMode/ d� 	IsChecked0 �Name1 �� IsDropDownOpen2 � AllowsTransparency3 �� MaxDropDownHeight4 � ActualWidth5 J� Effect3  -System.Windows.Media.Effects.DropShadowEffect6   Color7   
BlurRadius8   ShadowDepth9   Opacity  ScrollViewerStyle: K� IsItemsHost-   'System.Windows.Input.KeyboardNavigation;   DirectionalNavigation  DocumentTypeComboBoxStyle�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      %�  '��       '�� 4     &	  �     '   }    &	 {    '  �	    7  '  �	    &	 Z    '  �    7  &	 �    '�� 4    &	 �    &	 F    &	 �    7  &	 �    7  �� "  ��.���  �@�FocusVisualStyle Z� �� "  ��.���  � ��$ Center=���  � ��$ Microsoft YaHei,宋体)��� "  ��.���  � ��$ Microsoft YaHei,宋体)���  � ��$ 13(���  � ��
 �   ���  �@�Cursor$
 Hand����  � ��$ LeftToRight=���  � ��$ 1q���  � �� �#c5b8b8��  � ��
 �������  � ��$ Center=���  � �� �� " ��.R��� $
 back��$	 0.8Q�$ 3��#��z��#��z��#��z��._��� $
 fore��$��0q�._��� $ content��$��Center=�$��Center=�#��z�� F� $
	 #000��$	
 -90Q�$ 2Q�$	 0.1Q�$	 0.3Q� P�  �@J�IsMouseOver .  �� $ Compose=�.���� .��W� $ 1X�$ 0:0:0.1D�$
 back��$ Opacity� �� $ SnapshotAndReplace=�.���� .��W� $ 0:0:0.1D�$
 back��$ Opacity�P�  � �� .  �� .���� .��W� $	 0.6X�$ 0:0:0.3D�$
 back��$ Opacity� �� .���� .��W� $ 0:0:0.3D�$
 back��$ Opacity�P�  � {� .   �� .���� .��W� $ 1X�$ 0:0:0.2D�$
 back��$ Opacity�W� $ 1X�$ 0:0:0.1D�$ content��$F @(ContentPresenter.BitmapEffect).(DropShadowBitmapEffect.Opacity)�W� $
 -135X�$ 0:0:0.1D�$ content��$H B(ContentPresenter.BitmapEffect).(DropShadowBitmapEffect.Direction)��� $
 #FFFX�$ 0:0:0.3D�$ content��$D >(ContentPresenter.BitmapEffect).(DropShadowBitmapEffect.Color)� �� .���� .��W� $ 0:0:0.2D�$
 back��$ Opacity�W� $ 0:0:0.1D�$ content��$F @(ContentPresenter.BitmapEffect).(DropShadowBitmapEffect.Opacity)�W� $ 0:0:0.1D�$ content��$H B(ContentPresenter.BitmapEffect).(DropShadowBitmapEffect.Direction)��� $ 0:0:0.3D�$ content��$D >(ContentPresenter.BitmapEffect).(DropShadowBitmapEffect.Color)� P�  �@J�IsMouseOver . . ���  � �� �#3091f2P�  � �� . . ���  � �� �#3091f2�� "    .���  �@  CanUserResizeColumns . ��  � �� �#edf2f8��  �@��AlternationCount$ 2����  � �� �#d8e6f3��   �@  HorizontalGridLinesBrush �� $ #d8e6f3����  �@  VerticalGridLinesBrush Z� �� "   .���  �@J�SnapsToDevicePixels . ��  � ��$ 40����  � �� �#323433��  � ��$ 13(���  � ��$
 Bold"���  �@�Cursor$
 Hand����  � ��$ 40����  � ��$
 Left=���  � ��$ Center=���  � �� .  ��  � �� �#d9e5f3��  � ��$ 0 0 1 0q���  � ��$	 5 0q��� "   8   �� "   .���  � ��
 �������  � ��$ 40����  � ��
 �   � P�  �@��AlternationIndex$ 0��. ���  � �� �#edf2f8P�  �@��AlternationIndex$ 1��. ���  � ��
 �����P�  �@J�IsMouseOver . . ���  � �� �#cde7ffP�  �@ 
IsSelected . . ���  � ��
 �   ���  � �� �#cde7ff�� "   .���  � ��$	 5 0q� P�  �@ 
IsSelected . . ���  � ��
 �   ���  � �� �#cde7ff�� "   8   .���  � �� �� "  .R��� #��z�� . $��0q�#��z ._��� # z $
��Left=�$��Center=�#��z��$��CharacterEllipsis=� ��  �� *TemplatedParent+$  Content.Text�����  �� *TemplatedParent+$  Content.Text��� "  .R��� #��z��#��z��#��z�� . ._��� #��z��#��z��#" z! # z �� "  ��.���  � ��
 �   ���  � �� Z�  P�  �@��IsMouseOver . . ���  � �� �#F27D00��  � �� |� }� $# 	Underline=��� "  ��.���  � ��$ Right=���  � ��$	 Top=���  � ��$ 22����  � ��$ 27���� "  d�.���  � �� �� .R��� $
 Back�����#F7FDF7$��1q�
������ ._�F� $��PathFill�����#3798f9$��8��$��6��$��0��"����@KL     р�� ��� �    ��� 	$$ 0.5,0.5&�$
% Fill=�z�V� .~��� �� �� $	& 180Q�T�  P�  �@J�IsMouseOver . . ��� $' PathFill�� � ��
 ������� $
' Back�� � �� �#3798f9�� $
' Back�� � �� �#3798f9�� "  ��.���  � �� �� "  ��.���  � ��$ 22����  � ��$ 60����  � �� �� " ��.R��� $
��Back��
������ $��0,0,0,0q����#81D779._��� $��Center=�$
��Left=�$��5,0,0,0q����� *Source+ P�  �@J�IsMouseOver . . ��� $
' Back�� � �� �#d8e9faP�  �@��IsHighlighted . . ��� $
' Back�� � �� �#d8e9fa��  � �� �� " ��.R�� ���#d6d6d6( �� $
��0.7* ��� $
��0.3* �$��30��.+��� $) cbxText��$��0��$��Center=�#+ z* ���� *
Foreground+$, TwoWay=�$- PropertyChanged=����� *	FiledName+$, TwoWay=�$- PropertyChanged=��� $ dropText��$��0��$��1,1,0,1q����#d6d6d6$ 1,0,0,1���� $ dropRightBtn��$��1��$��0,1,1,1q����#d6d6d6$ 0,1,1,0��._�d� 8��  $. Press=�/ �� $  IsDropDownOpen�$, TwoWay=� �� *TemplatedParent+� $0 Popup��#��z1 $��Bottom=�|�.  2 . $��Slide=�.���� $ DropDown��$ 1��#��z3 #��z4  . 5   $6 Black��$7 2Q�$8 0Q�$	9 0.5Q�._��� $��4,6,4,6q�#���  #��z3  . $
��Auto=�$
��Auto=���. .���� : . $; 	Contained=�
������� P�  �@J�IsMouseOver . . ��� $' dropRightBtn�� � �� �#3798f9�� $' dropText�� � �� �#3798f9�� "  ��.���  � �� �� "  ��.���  � ��$ 22����  � ��$ 60����  � �� �� " ��.R��� $
��Back��
������ $��0,0,0,0q����#81D779._��� $��Center=�$
��Left=�$��5,0,0,0q����� *Source+ P�  �@J�IsMouseOver . . ��� $
' Back�� � �� �#d8e9faP�  �@��IsHighlighted . . ��� $
' Back�� � �� �#d8e9fa��  � �� �� " ��.R�� ���#d6d6d6( �� $
��0.7* ��� $
��0.3* �$��30��.+��� $) cbxText��$��0 3 0 0q�$��0��#+ z* ���� *
Foreground+$, TwoWay=�$- PropertyChanged=����� *BibTypeName+$, TwoWay=�$- PropertyChanged=��� $ dropText��$��0��$��1,1,0,1q����#d6d6d6$ 1,0,0,1���� $ dropRightBtn��$��1��$��0,1,1,1q����#d6d6d6$ 0,1,1,0��._�d� 8��  $. Press=�/ �� $  IsDropDownOpen�$, TwoWay=� �� *TemplatedParent+� $0 Popup��#��z1 $��Bottom=�|�.  2 . $��Slide=�.���� $ DropDown��$ 1��#��z3 #��z4  . 5   $6 Black��$7 2Q�$8 0Q�$	9 0.5Q�._��� $��4,6,4,6q�#���  #��z3  . $
��Auto=�$
��Auto=���. .���� : . $; 	Contained=�
������� P�  �@J�IsMouseOver . . ��� $' dropRightBtn�� � �� �#3798f9�� $' dropText�� � �� �#3798f9