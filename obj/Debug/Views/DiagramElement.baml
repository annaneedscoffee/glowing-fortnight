   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      J FDiagramDesigner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DiagramDesignerDiagramDesigner 5      C&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls 5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35   DiagramElementContextMenu.   (System.Windows.Input.ApplicationCommands
     Cut m� Command m� Icon    Copy    Paste    Delete#  DiagramDesigner.DiagramCanvas   Group   Ungroup$  DiagramDesigner.ConnectionNode �� 
TargetType	 �� Property
 �� Value �� 
TargetType # ConnectionNodeDecoratorTemplate  Name   Orientation  ResizeDecoratorTemplate J� Opacity J� SnapsToDevicePixels,  &DiagramDesigner.Controls.ResizeControl � Cursor*  $DiagramDesigner.Controls.DragControl$  DiagramDesigner.DiagramElement � ContextMenu � DataContext �� RelativeSource  Name ��Name ��Name �� Triggers �� Value �� Binding �� Path �� 
TargetName ^� 
Conditions �� Binding �� Value �� Empty  P� Property! P� Value�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    5      .sys&clr-namespace:System;assembly=mscorlib  5      #sclr-namespace:DiagramDesigner  5      ,c&clr-namespace:DiagramDesigner.Controls  5      %  &	        '  ,    &	 �    &	 Y    '  '
    '  �
    7   7  7  �� 5      .@�5   
   m� $	��Cut��6   # Z  6     5	      �� 5
      $��Images/Cut.png��6   $��16��60   5      m� 5   
   $
��Copy��6   # Z 6!    5      �� 5      $��Images/Copy.png��6   $��16��61   5      m� 5   
   $��Paste��6   # Z 6"    5      �� 5      $��Images/Paste.png��6   $��16��62   5      m� 5   
   $��Delete��6   # Z 6#    5      �� 5      $��Images/Delete.png��6   $��16��63   5      �� 5   
   m� 5   
   $��Grouping��6   .@�5      m� $��Group��6   # Z 6&    5      �� 5       $��Images/Group.png��6   $��16��66   5"      m� 5#      $��Ungroup��6   # Z 6(    5$      �� 5%      $��Images/Ungroup.png��6   $��16��68   5'      5(      5)      �� 5,      "  6   .�5-   
   �� 	 � ��$
 8��6"   �� 5.   
   	 � ��$
 8��6#   �� 5/   
   	 �@�Cursor$
 Cross��6#   �� 50   
   	 �@J�SnapsToDevicePixels
 . 60   �� 51   
   	 � ��
 52      �� 53      "  6"   .R�54      � .+�56      �� 
������ $��-2q�67   �� 57      
�������$��1��64   ���	#AA0000806H   58      59      5;      5<      �� 5?      " ��6>   .R�5@   
   � $��-5q�6   .+�5A        $
 Left��6   $
 Left=�6-   $��Center=�6@   $
��Left=�6[     5B      $	 Top��6   $	 Top=�6,   $	��Top=�6>   $��Center=�6V     5C      $ Right��6   $ Right=�6.   $��Center=�6B   $��Right=�6]     5D      $ Bottom��6   $ Bottom=�6/   $��Bottom=�6D   $��Center=�6_   5E      5F      �� 5I      " ��66   .R�5J   
   � $	 0.7Q�6    . .+�5K        $��3��6   $ SizeNS��6)   $��0 -4 0 0q�69   $	��Top=�5L      $��Stretch=�6.     5M      $��3��6   $ SizeWE��6(   $��-4 0 0 0q�68   $��Stretch=�5N      $
��Left=�62     5O      $��3��6   $ SizeWE��6(   $��0 0 -4 0q�68   $��Stretch=�5P      $��Right=�62     5Q      $��3��6   $ SizeNS��6)   $��0 0 0 -4q�69   $��Bottom=�5R      $��Stretch=�61     5S      $��7��6   $��7��6(   $ SizeNWSE��63   $��	-6 -6 0 0q�6E   $	��Top=�5T      $
��Left=�6.     5U      $��7��6   $��7��6(   $ SizeNESW��63   $��	0 -6 -6 0q�6E   $	��Top=�5V      $��Right=�6.     5W      $��7��6   $��7��6(   $ SizeNESW��63   $��	-6 0 0 -6q�6E   $��Bottom=�5X      $
��Left=�61     5Y      $��7��6   $��7��6(   $ SizeNWSE��63   $��	0 0 -6 -6q�6E   $��Bottom=�5Z      $��Right=�61   5[      5\      �� 5_      "  6   .�5`   
   �� 	 � ��
 5a      �� 5b      "  6"   .R�5c      �� 
������ 6    5d      5f      5g      �� 5j      "  6   .�5k   
   �� 	 � ��$
 10��6%   �� 5l   
   	 � ��$
 10��6&   �� 5m   
   	 �@J�SnapsToDevicePixels
 . 60   �� 5n   
   	 � ��
 5o      �� 5p      "  6"   .R�5q      � 8    6   ��  �� *TemplatedParent+.+�5t        $ DragControl��6(   $ SizeAll��5u   &   �� 5w      $ ResizeDecorator��6"   $y�	Collapsed=�5x   "   8�� 5y   "   �� 5{      $ ContentPresenter��6+   $��Stretch=�5|   +   $��Stretch=�5}   +   #��z��5~   +   #��z��5   +   �� 5�      $ ConnectionNodeDecorator��6"   $y�Hidden=�5�   "   8�� 5�   "   5�       5�      �� 5�      $
 True��6&    63   ��  �� *Self+$ IsGroup�.a�5�      �� $ DragControl��6%   	 � y�$
 	Collapsed=�6T   5�      ^� 5�       5�      �� 5�   "    69   ��  �� *Self+$ 
IsSelected� True6,   69   �� 5�   "   # Z 6,    6N   ��  �� *Self+$ ParentID�.0�5�      �� $ ResizeDecorator��6%   	 � y�$
 Visible=�6X   5�      P� 5�        �@J�IsMouseOver! . . �5�      �� $ ConnectionNodeDecorator��6%   	 � y�$
 Visible=�6`   5�      �� 5�      $
 True��6&    63   ��  �� *Self+$ IsDragConnectionOver�.a�5�      �� $ ConnectionNodeDecorator��6%   	 � y�$
 Visible=�6`   5�      �� 5�      $
 True��6&    63   ��  �� *Self+$ IsGroup�.a�5�      �� $ ConnectionNodeDecorator��6%   	 � y�$
 Hidden=�6`   5�      5�      5�      5�      5�      