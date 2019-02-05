using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    public interface ICalculator
    {
        Task<int> AddAsync(int a, int b);
        int Add(int a, int b);
        int Sub(int a, int b);
    }
}
