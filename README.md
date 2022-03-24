# MathStringParser

This .dll library is expandable math equation parser. If function or sign you want to recognize in your string is not implemented already in it, you can easly add needed functions by yourself.

By default it supports such algebraic actions as addition, subtraction, multiplication, division adn raising to the power. Beside that sin function.

Every algebraic action can be represented by single sign(char). Function can be represented by sequence of any length(string). Every sign has its own precedence(+, -: 1; *, /: 2, ^: 3). Precedence decides wich action would be performed next. 
Those it also can be considering as restrictions of this pasrser. You can not implement operations which affect only one left-handed value, for example increment. You also can not add any function with parameters, at least yet.

Parser takes string with equation and ref on start index, that should be always == 0 at the begining and returns double. 

Usage examples:
1) Normal usage

![image](https://user-images.githubusercontent.com/49982100/159823237-f111c322-9c27-4fc6-a711-b6523ae574bb.png)

2) User defined algebraic actions. On integer division example.
![image](https://user-images.githubusercontent.com/49982100/159823301-4a333d7a-e7f2-4996-97a2-dd4bcdb11593.png)
![image](https://user-images.githubusercontent.com/49982100/159823418-8eee3557-e3a0-4fb9-ba92-2bae75efe89e.png)

3) User defined function. On cos calculation example.
![image](https://user-images.githubusercontent.com/49982100/159823474-2b02341b-361c-41e7-a7db-c24765c56455.png)
![image](https://user-images.githubusercontent.com/49982100/159823539-c604e311-658e-4190-8800-01a724ce0b7d.png)
