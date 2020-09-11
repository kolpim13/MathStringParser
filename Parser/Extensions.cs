using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringParser
{
    //делегат, отвечающий за математические операции
    public delegate void MathActions(Cell left, Cell right);
    public partial class Cell
    {
        ///словарь старшинства операций
        private static Dictionary<char, int> ActionPrecedence = new Dictionary<char, int>
        {
            { '+', 1 },
            { '-', 1 },
            { '*', 2 },
            { '/', 2 },
            { '^', 3 },
            { Parser.END_STR, 0 }
        };
        //начальная инициализация словаря простых математичиских действий между 2 "Cell"
        private static Dictionary<char, MathActions> Actions = new Dictionary<char, MathActions>
        {
            { '+', new MathActions(AddCells) },
            { '-', new MathActions(MinCells) },
            { '*', new MathActions(MultCells) },
            { '/', new MathActions(DerivCells) },
            { '^', new MathActions(PowerCells) },

        };

        //первоначальные действия для словаря относительно того, что делать с клетками
        private static void AddCells(Cell left, Cell right)
        {
            left.Value = left.Value + right.Value;
        }
        private static void MinCells(Cell left, Cell right)
        {
            left.Value = left.Value - right.Value;
        }
        private static void MultCells(Cell left, Cell right)
        {
            left.Value = left.Value * right.Value;
        }
        private static void DerivCells(Cell left, Cell right)
        {
            left.Value = left.Value / right.Value;
        }
        private static void PowerCells(Cell left, Cell right)
        {
            left.Value = Math.Pow(left.Value, right.Value);
        }
        ///////////////////////////////////////////////////////
        //добавление в словарь новых пар (знак - метод) и в словарь старшинства (знак - старшиснтво операции)
        public static void AddAction(char sign, MathActions action, int precedence = 1)
        {
            //Если символ, что пытаются добавить занят под нужды библиотеки, то не даем этого сделать
            foreach (char c in ReservedSigns)
            {
                if (c == sign)
                {
                    //добавить событие, которое говорит о неудачной попытке добавления знака из-за чего-либо
                    return;
                }
            }
            //Если все проверки были пройдены, то добаляем знак
            Actions.Add(sign, action);
            ActionPrecedence.Add(sign, precedence);
            ReservedSigns.Add(sign);
        }

        ///////////////////////////////////////////////////////
        ///Символы, что уже используются в программе.
        private static List<char> ReservedSigns = new List<char>
        {
            { '+' },
            { '-' },
            { '*' },
            { '/' },
            { '^' },
            { Parser.START_BLOCK },
            { Parser.END_BLOCK },
            { Parser.END_STR }
        };
    }

    public delegate void MathFunctions(Cell cell);
    public partial class Parser
    {
        private static Dictionary<string, MathFunctions> Functions = new Dictionary<string, MathFunctions>
        {
            { "sin", new MathFunctions(Sin) },
        };

        //первоначальные функции
        private static void Sin(Cell cell)
        {
            cell.Value = Math.Sin(cell.Value / 180D * Math.PI);
        }

        ///////////////////////////////////////////////////////
        //добавление новых пар строка - функция.
        public static void AddFunction(string name, MathFunctions func)
        {
            foreach(string s in ReservedFunctions)
            {
                if(s == name)
                {
                    return;
                }
            }
            //если проверка пройдена, то добовляем функцию
            Functions.Add(name, func);
            ReservedFunctions.Add(name);
        }

        ///////////////////////////////////////////////////////
        ///Функции, что уже используются в программе.
        private static List<string> ReservedFunctions = new List<string>
        {
            { "sin" },
        };
    }
}
