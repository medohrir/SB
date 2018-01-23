using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortBus.Tests.Example
{
    public class MyComplexeQuestionHandler : IRequestHandler<MyQuestion<MyComplexeResponse>, MyComplexeResponse>
    {
        public MyComplexeResponse Handle(MyQuestion<MyComplexeResponse> request)
        {
            return new MyComplexeResponse() {Info1 = "info1",Info2 = "info2"};
        }
    }

}