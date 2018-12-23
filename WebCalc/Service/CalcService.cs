using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCalc.Context;
using WebCalc.Models;
using WebCalc.ViewModel;

namespace WebCalc.Service
{
    /// <summary>
    /// сервис расчета
    /// </summary>
    public class CalcService
    {
        /// <summary>
        /// получить историю расчетов пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserCalcViewModel> GetUserHistory(UserCalcViewModel model)
        {
            using (var db = new BaseContext())
            {
                var dateFrom =  DateTime.UtcNow.Date;
                var dateTo = dateFrom.AddDays(1);
                model.Calculations = db.UserCalculations
                    .Where(x => x.IpAddress == model.IpAddress && x.DateCreate >= dateFrom && x.DateCreate <= dateTo)
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();
            }
            
            return model;
        }

        /// <summary>
        /// очистка истории расчетов пользователя
        /// </summary>
        /// <param name="ipAddress">ип адрес пользователя</param>
        /// <returns></returns>
        public async Task Clear(string ipAddress)
        {
            using (var db = new BaseContext())
            {
                var calcs = db.UserCalculations.Where(x => x.IpAddress == ipAddress).ToList();
                db.UserCalculations.RemoveRange(calcs);
                db.SaveChanges();
                return;
            }
        }

        /// <summary>
        /// произвести расчет
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserCalcViewModel> Caltulate(UserCalcViewModel model)
        {
            using (var db = new BaseContext())
            {
                var calResult = Calculation.Calculate(model.Input).ToString();
                var calc = new UserCalculation
                {
                    IpAddress = model.IpAddress,
                    Input = model.Input,
                    Result = calResult,
                    DateCreate = DateTime.UtcNow
                };
                model.Result = calc.Result;
                db.UserCalculations.Add(calc);
                db.SaveChanges();
                model = await GetUserHistory(model);
            }

            return model;
        }

        /// <summary>
        /// получить расчет по ид
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserCalcViewModel> GetCalcById(UserCalcViewModel model)
        {
            using (var db = new BaseContext())
            {
                var calc = db.UserCalculations.FirstOrDefault(x => x.Id == model.RepeatId);
                model.Input = calc.Input;
                model = await GetUserHistory(model);
                return model;
            }
        }

        /// <summary>
        /// класс расчета
        /// </summary>
        class Calculation
        {
            /// <summary>
            /// результат расчета
            /// </summary>
            /// <param name="input">формула</param>
            /// <returns></returns>
            static public double Calculate(string input)
            {
                string output = MyTemp(input);
                double result = Calculating(output);
                return result;
            }

            /// <summary>
            /// преоброзование формулы
            /// </summary>
            /// <param name="input">формула</param>
            /// <returns></returns>
            static private string MyTemp(string input)
            {
                string output = string.Empty;
                Stack<char> operators = new Stack<char>();

                for (int i = 0; i < input.Length; i++)
                {
                    if (IsDelimeter(input[i]))
                        continue;

                    if (Char.IsDigit(input[i]))
                    {
                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            output += input[i];
                            i++;

                            if (i == input.Length) break;
                        }

                        output += " ";
                        i--;
                    }

                    if (IsOperator(input[i]))
                    {
                        if (input[i] == '(')
                            operators.Push(input[i]);
                        else if (input[i] == ')')
                        {
                            char s = operators.Pop();
                            while (s != '(')
                            {
                                output += s.ToString() + ' ';
                                s = operators.Pop();
                            }
                        }
                        else
                        {
                            if (operators.Count > 0)
                                if (GetPriority(input[i]) <= GetPriority(operators.Peek()))
                                    output += operators.Pop().ToString() + " ";

                            operators.Push(char.Parse(input[i].ToString()));
                        }
                    }
                }

                while (operators.Count > 0)
                    output += operators.Pop() + " ";

                return output;
            }

            /// <summary>
            /// вычисление преобразованной формулы
            /// </summary>
            /// <param name="input">преобразованная формула</param>
            /// <returns></returns>
            static private double Calculating(string input)
            {
                double result = 0;
                Stack<double> temp = new Stack<double>();

                for (int i = 0; i < input.Length; i++)
                {
                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            a += input[i];
                            i++;
                            if (i == input.Length) break;
                        }
                        temp.Push(double.Parse(a));
                        i--;
                    }
                    else if (IsOperator(input[i]))
                    {
                        double a = temp.Pop();
                        double b = temp.Pop();

                        switch (input[i])
                        {
                            case '+': result = b + a; break;
                            case '-': result = b - a; break;
                            case '*': result = b * a; break;
                            case '/': result = b / a; break;
                        }
                        temp.Push(result);
                    }
                }
                return temp.Peek();
            }
        }

        /// <summary>
        /// проверка на пробел и равно
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }

        /// <summary>
        /// проверка на оператор
        /// </summary>
        /// <param name="с"></param>
        /// <returns></returns>
        static private bool IsOperator(char с)
        {
            if (("+-/*()".IndexOf(с) != -1))
                return true;
            return false;
        }

        /// <summary>
        /// приоритет операторов
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static private byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                default: return 5;
            }
        }
    }
}