   M S B A M L   `   `   `  ����@  <Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:UtilsUtils  5   	       Utils.dialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Utils     5   	   -   5          � Title$  	Translate��5	   	   $	��600��6   $
��1000��6(    � WindowStyle$
 None=�66    � 
ResizeMode$ NoResize=�6I    � AllowsTransparency .  � ShowInTaskbar .  
������ .��5
      � -   5         � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� -   5         ��Name$ back1��5   $   $��0��6   
������� J� Opacity$	 0.5Q�6?   6O   �� 5   
   -   5        $
 line��5   $   $��0��6   
�������$��2��6>   $��Right=�6H   6f   �� 5   
   -   5        $ line1��5   $   $��1��6   
�������$��2��6?   $
��Left=�6I   6f   �� 5   
   -   5        $ back2��5   $   $��1��6   
�������$	 0.5Q�6?   6O   �� 5   
   -   5         ��Name$ origin��5   k   $��
14,14,7,14q�6   $��0��6&   "	 �� VerticalScrollBarVisibility$
	 Auto=�66   
 �� 
IsReadOnly
 .  �� TextWrapping$
 Wrap=�6�   ���	#7F000000
�������$��16(�6�   $��Consolas)�6�   ���	#FF4059A4��6{   ��  �� Path$ 
OriginText� �� Mode$ TwoWay=�6%  �� 5   
   -   5        $
 goal��5   k   $��
7,14,14,14q�6   $��1��6&   $
	 Auto=�66   
 . $
 Wrap=�6�   ���	#7F000000
�������$��16(�6�   $��Consolas)�6�   ���	#FF4059A4��6y   �� $ GoalText�$ TwoWay=�6!  5      5      