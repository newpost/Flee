using Flee.CalcEngine.PublicTypes;
using Flee.PublicTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using DynamicExpresso;

namespace Flee.Console
{
    delegate bool TestDelegate(int i, int j, int k);
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            ExpressionContext context2 = new ExpressionContext();
            VariableCollection variables2 = context2.Variables;
            variables2["a"] = 100;
            variables2["b"] = 1;
            variables2["c"] = 24;
            IGenericExpression<bool> ge = context2.CompileGeneric<bool>("(a = 100 OR b > 0) AND c <> 2");
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                //Sample Scenario 2

                bool bl = func(100, 1, 24);

                //  bool result2 = ge.Evaluate();

            }
            stopwatch.Stop();
            double d1 = stopwatch.ElapsedMilliseconds;
            System.Console.WriteLine($"委托调用：{d1}");




            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 1000000; i++)
            {

                bool bl = Test(100, 1, 24);

            }
            stopwatch2.Stop();
            double d2 = stopwatch2.ElapsedMilliseconds;
            System.Console.WriteLine($"直接调用：{d2}");
            System.Console.WriteLine(d1 / d2);


            double d3 = Test2();
            System.Console.WriteLine($"CalculationEngine：{d3}");
            System.Console.WriteLine(d3/ d2);
            

            //double d4 = Test3();
            //System.Console.WriteLine(d4);
            //System.Console.WriteLine(d4 / d2);

            double d5 = Test4();
            System.Console.WriteLine($"DynamicExpresso:{d5}");
            System.Console.WriteLine(d5/ d2);

            double d6 = Test5();
            System.Console.WriteLine($"Delegate.CreateDelegate:{d6}");
            System.Console.WriteLine(d6/ d2);
            System.Console.ReadKey();



        }

        /// <summary>
        /// 构建一次ExpressionContext，CalculationEngine
        /// </summary>
        /// <returns></returns>
        public static double Test2()
        {
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            CalculationEngine calculationEngine = new CalculationEngine();
            ExpressionContext context = new ExpressionContext();
            VariableCollection variables = context.Variables;
            variables.Add("x", 100);
            variables.Add("y", 200);

            calculationEngine.Add("a", "x+y", context);
            for (int i = 0; i < 1000000; i++)
            {
                variables["x"] = i;
                calculationEngine.Recalculate("a");
                int j = calculationEngine.GetResult<int>("a");
            }

            stopwatch2.Stop();
            return stopwatch2.ElapsedMilliseconds;
        }

        private static Func<int, int, int, bool> func = (i, j, k) => (i == 100 || j > 0) && k != 2;

        /// <summary>
        /// 直接函数调用
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        static bool Test(int i, int j, int k)
        {
            return (i == 100 || j > 0) && k != 2;
        }

        /// <summary>
        /// 每次构造ExpressionContext
        /// </summary>
        /// <returns></returns>
        public static double Test3()
        {
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 1000000; i++)
            {
                //Sample Scenario 1
                ExpressionContext context = new ExpressionContext();
                VariableCollection variables = context.Variables;
                variables.Add("a", 1);
                variables.Add("b", 1);

                IGenericExpression<bool> e = context.CompileGeneric<bool>("a=1 AND b=0");
                bool result = e.Evaluate();
            }
            stopwatch2.Stop();
            return stopwatch2.ElapsedMilliseconds;

        }

        public static double Test4()
        {
            var target = new Interpreter();

            var parameters = new[] {
                new Parameter("x", typeof(int)),
                new Parameter("y", typeof(int))
            };

            var myFunc = target.Parse("x + y", parameters);
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 1000000; i++)
            {
                myFunc.Invoke(23, 7);
            }
            stopwatch2.Stop();
            return stopwatch2.ElapsedMilliseconds;
            
        }

        public static double Test5()
        {
            

            
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();

            Delegate del = Delegate.CreateDelegate(typeof(TestDelegate), typeof(Program), "Test", false);

            for (int i = 0; i < 1000000; i++)
            {

                del.DynamicInvoke(100, 1, 24);

            }

            stopwatch2.Stop();
            return stopwatch2.ElapsedMilliseconds;
        }

    }
}
