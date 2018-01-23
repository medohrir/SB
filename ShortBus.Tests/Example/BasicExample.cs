using System;
using System.Diagnostics;
using Ninject;
using NUnit.Framework;
using ShortBus.Ninject;
using ShortBus.Tests.Containers;
using StructureMap;

namespace ShortBus.Tests.Example
{
    [TestFixture]
    public class BasicExample
    {
        private IKernel _kernel;
        public BasicExample()
        {
            _kernel = new StandardKernel();
            //kernel.Bind<IKernel>().To<StandardKernel>().InSingletonScope();
            _kernel.Bind<IMediator>().To<Mediator>().InSingletonScope();
            _kernel.Bind<IDependencyResolver>().To<NinjectDependencyResolver>();
            _kernel.Bind<IRequestHandler<PrintText, UnitType>>().To<ConsoleWriter>();
            //_kernel.Bind<IRequest<string>>().To<Ping>();
            _kernel.Bind<IRequestHandler<Ping, string>>().To<Pong>();
            _kernel.Bind<IRequestHandler<PingALing, string>>().To<Pong>();
            _kernel.Bind<IRequestHandler<MyQuestion<string>, string>>().To<MyQuestionHandler>();
            _kernel.Bind<IRequestHandler<MyQuestion<MyComplexeResponse>, MyComplexeResponse>>().To<MyComplexeQuestionHandler>();



            _kernel.Bind<IRequestHandler<DoublePing, string>, IRequestHandler<TriplePing, string>>().To<MultiPong>();

            var resolver = _kernel.Get<IDependencyResolver>();
            ShortBus.DependencyResolver.SetResolver(resolver);
        }

        [Test]
        public void ResponseToRequest()
        {
            var query = new Ping();

            var mediator = _kernel.Get<IMediator>();

            var pong = mediator.Request(query);

            Assert.That(pong.Data, Is.EqualTo("PONG!"));
            Assert.That(pong.HasException(), Is.False);
        }

        [Test]
        public void MyResponseToMyQuestion()
        {
            var myQuestion = new MyQuestion<string>();

            var mediator = _kernel.Get<IMediator>();

            var myResponse = mediator.Request(myQuestion);

            Assert.That(myResponse.Data, Is.EqualTo("This the response corresponding to MyQuestion"));
            Assert.That(myResponse.HasException(), Is.False);
        }
        [Test]
        public void MyComplexeResponseToMyQuestion()
        {
            var myQuestion = new MyQuestion<MyComplexeResponse>();

            var mediator = _kernel.Get<IMediator>();

            var myResponse = mediator.Request(myQuestion);

            Assert.That(myResponse.Data.Info1, Is.EqualTo("info1"));
            Assert.That(myResponse.Data.Info2, Is.EqualTo("info2"));
            Assert.That(myResponse.HasException(), Is.False);
        }


        [Test]
        public void RequestResponseImplementationWithMultipleHandler()
        {
            var query = new TriplePing();

            var mediator = _kernel.Get<IMediator>();

            var pong = mediator.Request(query);

            Assert.That(pong.Data, Is.EqualTo("PONG! PONG! PONG!"));
            Assert.That(pong.HasException(), Is.False);
        }

        [Test]
        public void RequestResponse_variant()
        {
            var query = new PingALing();

            var mediator = _kernel.Get<IMediator>();

            var pong = mediator.Request(query);

            Assert.That(pong.Data, Is.EqualTo("PONG!"));
            Assert.That(pong.HasException(), Is.False);
        }

        //[Test]
        //public void RequestResponseImplementationWithMultipleHandler_variant()
        //{
        //    var query = new DoublePingALing();

        //    var mediator = ObjectFactory.GetInstance<IMediator>();

        //    var pong = mediator.Request(query);

        //    Assert.That(pong.Data, Is.EqualTo("PONG! PONG!"));
        //    Assert.That(pong.HasException(), Is.False);
        //}

        //[Test]
        //public void Send_void()
        //{
        //    var command = new PrintText
        //    {
        //        Format = "This is a {0} message",
        //        Args = new object[] { "text" }
        //    };

        //    var mediator = ObjectFactory.GetInstance<IMediator>();

        //    var response = mediator.Request(command);

        //    Assert.That(response.HasException(), Is.False, response.Exception == null ? string.Empty : response.Exception.ToString());
        //}

        //[Test]
        //public void Send_void_variant()
        //{
        //    var command = new PrintTextSpecial
        //    {
        //        Format = "This is a {0} message",
        //        Args = new object[] { "text" }
        //    };

        //    var mediator = ObjectFactory.GetInstance<IMediator>();

        //    var response = mediator.Request(command);

        //    Assert.That(response.HasException(), Is.False);
        //}

        //[Test]
        //public void Send_result()
        //{
        //    var command = new CommandWithResult();

        //    var mediator = new Mediator(DependencyResolver.Current);

        //    var response = mediator.Request(command);

        //    Assert.That(response.Data, Is.EqualTo("foo"));
        //}

        //[Test, Explicit]
        //public void Perf()
        //{
        //    var mediator = ObjectFactory.GetInstance<IMediator>();
        //    var query = new Ping();

        //    var watch = Stopwatch.StartNew();

        //    for (int i = 0; i < 10000; i++)
        //        mediator.Request(query);

        //    watch.Stop();

        //    Console.WriteLine(watch.Elapsed);
        //}
    }
}