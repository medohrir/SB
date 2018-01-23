using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortBus.Tests.Example
{
    public class MyQuestionHandler : IRequestHandler<MyQuestion<string>, string>
    {
        public string Handle(MyQuestion<string> request)
        {
            return "This the response corresponding to MyQuestion";
        }
    }
}
