   M S B A M L   `   `   `  ���� E  ABib-Master, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Bib_Master
Bib_Master      Bib_Master.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:Bib_Master       � Title$  
Bib-Master��$
��73.5��$	��500��$	��450��$	��800�� � 	Resources  �� 
TargetType   MenuItemIconImage �� Property �� Value��%9   &	        �� " ��.���  � ��$ -3,-17,-31,-17q�.���  � ColumnDefinitions �� $	��22* ��� $	��70* ��� $	��27* � � RowDefinitions �� $��60 ��� $	��50* ��� $��4* ��� $��* �.+�o� -   $��menubar��$��0��$��3��$��0��.@�m� -   $��menuitemAdd��$��50��$��50��$��	30,5,30,0q� � ToolTip$ 	Add eBook�� m� Icon �� -   $��imageAdd��#��[  m� -   $��menuitemDelete��$��50��$��50��$��	15,5,30,0q�$ Delete selected category�� �� -   $��imageDelete��#��[  m� -   $��menuitemRead��$��50��$��50��$��	15,5,30,0q�$ Read selected eBook�� �� -   $��	imageRead��#��[  m� -   $��menuitemEdit��$��50��$��50��$��	15,5,30,0q�$' !Edit metadata of a selected eBook�� �� -	   $��	imageEdit��#��[  m� -
   $��menuitemCategory��$��50��$��50��$��	15,5,30,0q�$ Edit Categories�� �� -   $��imageCategory��#��[  R� -   $��tvCategories��$��0��$��1��$
��12.5(�$��8,10,0,0q��� -   $��btnAddCategory��$��Bottom=�$��0��$��2��$��8,5,0,0q�$��45��.��(� .���� $��Add Category��$
��Wrap=�	 �� TextAlignment$	 Center=�$��3q��� -   $��
txtboxSeek��$��1��$��1��$��20��$	��Top=�$��15,10,80,270q��� $��1��$��1��
 J� IsHitTestVisible
 .  $
��Seek��$	��Top=�$
��Left=�$��	19,12,0,0q�
����������� " ��.���  � y�$ 	Collapsed=� �� Triggers ��  �� Binding �� *Text+ �� ElementName$ 
txtboxSeek�� �� Value  .a���  � y�$ Visible=��� -   $��
buttonSeek��$
��Seek��$ Click to apply filter��$��1��$��1��$��Right=�$	��Top=�$��35��$��20��$��20��$��0,10,15,270q��� -   $��buttonClearFilter��$��1��$��1��$��Right=�$	��Top=�$��18��$��20��$��18��$��20��$��0,10,57,270q�$��0q�#��[" �$ Clear filter��.���� -   $��imgClearFilter���� -   $��listviewBooks��$��1��$��1��$��2��$��
15,50,15,0q� �� SelectionMode$ Single=� � ContextMenu �� -   $��contextmenuGrid�� �� 	StaysOpen . 
������� J� BitmapEffect �� .@�m� -   $��contxtCategory��$��Assign Category��m� -   $��
contxtPath��$��	Open Path��m� -   $��contxtDelete��$��Delete�� �� View �� -   gridView��.P��� $��Title�� �� Width$	 320�� �� DisplayMemberBinding �� *Title+�� $��Author��$	 230�� �� *Author+�� $��Format��$	 120�� �� *Format+�� $��Added On��$	 160�� �� *	DateAdd+�� $��2��$��1��$��2��$
��Info��$��0,10,8,0q�.���� .���� $	��Top=� �� CanVerticallyScroll . .+�(� $��0,0,0,0q�$	��Top=�.���� -   $��
imageCover��$	��350��$��10,25,10,30q��� -   $��txtTitle��$��WrapWithOverflow=�$��Title: ��$	��Top=�$��0,0,0,20q��� -   $��txtCategory��$��WrapWithOverflow=�$��
Category: ��$	��Top=�$��	0,10,0,20q��� -   $��txtPath��$��WrapWithOverflow=�$��Path: ��$	��Top=�$��	0,10,0,20q�