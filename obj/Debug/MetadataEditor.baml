   M S B A M L   `   `   `  ����E  ABib-Master, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Bib_Master
Bib_Master  5   	       Bib_Master.MetadataEditor   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Bib_Master     5   	     � Title$  MetadataEditor��5   	   $	��450��6    $	��750��6-    � 
ResizeMode$ NoResize=�69   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��1��6.   .+�5      �� $��2q�6   
�������$	��Top=�6A   $	��300��6Y   $	��230��6f   $��
10,20,80,0q�6r   ._�5      �� -   5        $��imgCover��5      $��Stretch=�6(   $��Stretch=�6F   $
��Fill=�6b   5      �� 5      $ Vertical=�6   $	��400��60   .+�5      �� $��Title��6   $��20(�6)   $	��Top=�67   $��0,20,0,0q�6O   �� 5      -   5        $��txtboxTitle��5      �� 5      $��Author��6   $��20(�6*   $	��Top=�68   $��0,40,0,0q�6P   �� 5      -   5        $��txtboxAuthor��5      �� 5      $��Keywords��6   $��20(�6,   $	��Top=�6:   $��0,40,0,0q�6R   �� 5      -   5        $��txtboxKeywords��5      �� 5       $��	0,30,0,30q�6   $��Options��6.   $	��120��6?   $	��Top=�6L   .��5!      �� $ 
Horizontal=�6!   $��0,0,0,0q�6:   $��70��6K   .+�5"      �� -   5        $��
buttonSave��5"   !   $
��Save��6L   $��80��6[   $��100,20,0,20q�6f   �� 5#      -   5        $��buttonClose��5#   !   $��Close��6N   $��80��6^   $��
40,20,0,20q�6i   5$      5%      5&      5'      5(      5)      