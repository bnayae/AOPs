using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    public interface ICalculatorAsync
    {
        Task<int> AddAsync(int a, int b);
        Task<int> SubAsync(int a, int b);
    }
}
