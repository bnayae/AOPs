using System;
using Bnaya.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AopTests
{
    [TestClass]
    public class DynamicProxyFactory_Tests
    {
        private ICalculator _calc = A.Fake<ICalculator>();
        private ICalculatorAsync _calcAsync = A.Fake<ICalculatorAsync>();

        [TestInitialize]
        public void Setup()
        {
            A.CallTo(() => _calc.Add(A<int>.Ignored, A<int>.Ignored))
                .ReturnsLazily<int, int, int>((a, b) =>
                {
                    Trace.WriteLine($"Calling Mock {a} + {b}");
                    return a + b;
                });
            A.CallTo(() => _calcAsync.AddAsync(A<int>.Ignored, A<int>.Ignored))
                .ReturnsLazily<Task<int>, int, int>(async (a, b) =>
                {
                    await Task.Delay(1000);
                    Trace.WriteLine($"Calling Mock {a} + {b}");
                    return a + b;
                });
        }

        [TestMethod]
        public void DynamicProxyFactory_SimpleAOP_Test()
        {
            ICalculator decorate = DynamicProxyFactory<ICalculator>.Create(_calc,
                info => Trace.WriteLine($"Before {info}"),
                info => Trace.WriteLine($"After {info}"),
                info => Trace.WriteLine($"Error {info}"));

            int i = decorate.Add(1, 2);
            Trace.WriteLine($"Result = {i}");
        }

        [TestMethod]
        public async Task DynamicProxyFactory_SimpleAOP_Async_Test()
        {
            ICalculatorAsync decorate = DynamicProxyFactory<ICalculatorAsync>.Create(_calcAsync,
                info => Trace.WriteLine($"Before {info}"),
                info => Trace.WriteLine($"After {info}"),
                info => Trace.WriteLine($"Error {info}"));

            int i = await decorate.AddAsync(1, 2);
            Trace.WriteLine($"Result = {i}");
        }
    }
}
