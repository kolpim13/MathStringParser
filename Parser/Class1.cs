using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringParser
{
    //Делегат для уведомления
    public delegate void MessageHandler(string mes);

    public partial class Cell
    {
        //значение ячейки
        public double Value { get; set; }
        //знак ячейки
        public char Sign { get; set; }
        //является ли ячейка аргументом
        public bool IsArgument { get; set; }

        public Cell(double value, char sign, bool isArgue = false)
        {
            Value = value;
            Sign = sign;
            IsArgument = isArgue;
        }

        //////////////////////////////////////////////////////
        //проверка, есть ли данный знак в словаре
        public static bool IsSign(char c)
        {
            foreach (char sign in ActionPrecedence.Keys)
            {
                if (c == sign)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsDigit(char c)
        {
            if (char.IsDigit(c) || c == '.' || c == ',')
            {
                return true;
            }
            return false;
        }

        //////////////////////////////////////////////////////
        //обьеденить (согласно знаку левой) 2 ячейки (правую и левую) и сохранить результат в левой, а знак оставить правой
        public static bool Merge(Cell leftCell, Cell rightCell)
        {
            if (IsSign(leftCell.Sign))
            {
                Actions[leftCell.Sign](leftCell, rightCell);
                leftCell.Sign = rightCell.Sign;
                return true;
            }
            return false;
        }

        //Сравниваем старшинство операций. Если левая больше, то true, наоборот - false
        public static bool IsSenior(Cell leftCell, Cell rightCell)
        {
            if (ActionPrecedence[leftCell.Sign] >= ActionPrecedence[rightCell.Sign])
                return true;
            return false;
        }
    }

    public partial class Function
    {
        public Cell Arg;
        public string Func { get; set; }

        public Function(Cell cell, string func)
        {
            Arg = cell;
            Func = func;
        }
    }

    public partial class Parser
    {
        //Событие для уведомления об ошибках ипрочем
        public static event MessageHandler Notify;

        //список ячеек, что удалось выделить из строки
        private static List<Cell> listToMerge = new List<Cell>();

        //зарезервированные сиволы
        public const char END_STR = '}';
        public const char START_BLOCK = '(';
        public const char END_BLOCK = ')';

        public Parser(){}

        ///////////////////////////////////////////////////////////
        ///Разложть строку на ячейк и посчитать
        public static double SplitAndCalc(string data, ref int index, double arg = 1, int listIndex = 0)
        {
            //Блок первоначальной проверки строки на корректность 
            if (index >= data.Length)
            {
                Notify?.Invoke("индекс символа стоки больше, чем символов в строке");
                return 0;
            }

            //если первый вход в функцию, то
            if (index == 0)
            {
                //проверяем правильно ли расставлены скобки
                if (!IsBracketsCorrect(data))
                {
                    Notify?.Invoke("скобки расставлены некоректно");
                    return 0;
                }
                //очищаем список
                listToMerge.Clear();
            }

            //Еслив конце строки нет символа конец строки, то добавить его
            if (data[data.Length - 1] != END_STR)
                data = data + END_STR;            

            //переменные, что нам понадобятся
            //для заполнения
            StringBuilder number = new StringBuilder();
            StringBuilder function = new StringBuilder();
                //для создания Cell
            char sign = END_STR;
            bool isArg = false;
            double var;
                //дополнительные
            int listIndexNext = listIndex;
            bool wasDigit = false;
            char c;

            //разбитие строки на блоки
            do
            {
                c = data[index];

                if (c == START_BLOCK)
                {
                    index++;
                    SplitAndCalc(data, ref index, arg, listIndexNext);
                    //если мы зашли в рекусрсию после функции, то выполняем текст функции.
                    if(function.Length != 0)
                    {
                        if (IsFunction(function.ToString())){
                            //
                            CalcArgument(listToMerge[listIndexNext], arg);
                            //
                            Functions[function.ToString()](listToMerge[listIndexNext]);
                            //function.Clear();
                            listIndexNext = listToMerge.Count;
                        }
                        else
                        {
                            //удалить текущий первый элемент, если функция была введена неправильно
                            listToMerge.RemoveAt(listIndexNext);
                            //function.Clear();
                            //Добавить событие, сигнализирующее о неправильно введенной функции
                        }
                        function.Clear();
                    }  
                    continue;
                }
                if (c == END_BLOCK)
                {
                    if (function.Length == 0)
                    {
                        int i = -1;
                        while (!Cell.IsSign(data[index++]))
                            i++;
                        index--;
                        sign = data[index];
                        if (i > 0) index -= (i+1);
                        if (double.TryParse(number.ToString(), out var))
                            listToMerge.Add(new Cell(var, sign, isArg));

                        if (listToMerge.Count - listIndex > 1)
                            return MergeList(listToMerge, listToMerge[listIndex], listIndex, arg);
                        return 0;
                    }
                }

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (Cell.IsDigit(c))
                {
                    if (c == '.') c = ',';
                    number.Append(c);
                    wasDigit = true;
                    continue;
                }

                if (char.IsLetter(c))
                {
                    if (wasDigit)
                        isArg = true;
                    else
                        function.Append(c);
                    continue;
                }

                if (Cell.IsSign(c))
                {
                    if (!wasDigit && (c == '+' || c == '-'))
                    {
                        number.Append(c);
                        wasDigit = true;
                        continue;
                    }
                    sign = c;
                }

                //добавление в список нового элемента
                    //если не было введено цифры(в том числе и после '.'), то приравниваем ее к "0" 
                if (!wasDigit || number[number.Length-1] == '.' || number[number.Length - 1] == ',')
                    number.Append("0");
                    //или цифры др '.', то дописываем 0
                if (number[0] == '.' || number[0] == ',')
                    number.Insert(0, "0", 1);
                    //проверяем число перед преобразованием на всякий случай
                if (double.TryParse(number.ToString(), out var))
                {
                    listToMerge.Add(new Cell(var, sign, isArg));
                    listIndexNext = listToMerge.Count;
                }

                //очищаем используемые переменныедля следующего захода
                number.Clear();
                function.Clear();
                sign = END_STR;
                wasDigit = false;
                isArg = false;

            } while (++index < data.Length);

            if (listToMerge.Count - listIndex > 0)
                return MergeList(listToMerge, listToMerge[listIndex], listIndex, arg);
            return 0;
        }
        
        //Обьеденить ListToMerge в 1 ячейку. index - текущий номер элемента в списке
        private static double MergeList(List<Cell> listToMerge, Cell current, int index, double arg)
        {
            CalcArgument(current, arg);
            while (index < listToMerge.Count - 1)
            {
                Cell next = listToMerge[index + 1];
                CalcArgument(next, arg);
                if (!Cell.IsSenior(current, next))
                {
                    MergeList(listToMerge, next, index + 1, arg);
                }

                Cell.Merge(current, next);
                listToMerge.RemoveAt(index + 1);

                //если это вложенныая итерация выполнения функции, то выйти из нее после одного пробега, что б функция просчитывалась строго слева направо
                if (index != 0)
                    return current.Value;
            }
            return current.Value;
        }

        //посчитать значение ячейки, если она аргумент
        private static void CalcArgument(Cell cell, double arg)
        {
            if (cell.IsArgument)
            {
                cell.Value = cell.Value * arg;
                cell.IsArgument = false;
            }
        }

        ///////////////////////////////////////////////////////////
        ///Посмотреть, правильно ли расставлены открывающие и закрывающие скобки
        private static bool IsBracketsCorrect(string data)
        {
            Stack<char> brackets = new Stack<char>();
            char c;
            
            for (int i=0; i<data.Length; i++)
            {
                c = data[i];

                if(c == '(')
                {
                    brackets.Push(c);
                    continue;
                }
                
                if(c == ')')
                {
                    if (brackets.Count > 0)
                        brackets.Pop();
                    else
                        return false;
                }
            }

            if (brackets.Count == 0)
                return true;
            return false;
        }

        //смотрим есть ли такая функция и если есть, то выполняем, нет - пропуск
        private static bool IsFunction(string func)
        {
            foreach(string str in Functions.Keys)
            {
                if (str == func)
                    return true;
            }
            return false;
        }
    }
}
