   M S B A M L   `   `   `  ����J  FDiagramDesigner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DiagramDesignerDiagramDesigner  5   	   C&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls  5   	        DiagramDesigner.MainWindow   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   )sclr-namespace:DiagramDesigner    5   	   2c&clr-namespace:DiagramDesigner.Controls    5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Diagram Designer��5   	   $	��800��5   	   $
��1000��6    � 	Resources 5
         DiagramCanvasContextMenu.  (System.Windows.Input.ApplicationCommands   Paste m� Command m� Icon#   DiagramDesigner.DiagramCanvas   	SelectAll��%�   &	        �� 5   
   .@�5      m� $��Paste��6   # Z 6&    5      �� 5      $��Views/Images/Paste.png��6   $��16��6<   5      m� 5      $��
Select All��6   # Z 6+   5      5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��   	MyToolbar#��[ 6   � 5   
   $��1��6   $��0,10,0,0q�6    � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5       .+�5#      �� $��0��6   $��0,0,8,0q�6)   .+�5$      �� $��Diagram Elements��6   $��0,0,0,8q�66     DiagramElements#��[ 6G   �� 5%      $��Logical Operators��6     LogicalOperatorStencils#��[ 67   5&      �� 5)      $��Diagram��6   $��1��6(   .��5*      �� $
��Auto=�6   $
��Auto=�5+      .��5,        -   5        	  Name$	 DiagramDesigner��5,   7   |�.   WindowBackgroundBrush#��[ 5-      $��10q�5.      
 � ContextMenu#
 [  5/       � FocusVisualStyle 5.   )   Z� 5/      50      51      52      53      54      