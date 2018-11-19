//Copyright(c) 2014-2016 tynor88

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using MassTransit;
using SimpleInjector;

namespace SpringIt.ServiceBus.SimpleInjector
{
    public static class SimpleInjectorEndpointConfigurator
    {
        public static EndpointConfigurator UseSimpleInjector(this EndpointConfigurator endpointConfigurator,
            Container container, Func<IFactory, IBusControl> busFactory)
        {
            IBusControl BusInstanceFactory()
            {
                var factory = container.GetInstance<IFactory>();
                var bus = busFactory.Invoke(factory);
                return bus;
            }

            var registration = Lifestyle.Singleton.CreateRegistration(BusInstanceFactory, container);
            container.AddRegistration(typeof(IBus), registration);
            container.AddRegistration(typeof(IBusControl), registration);


            return endpointConfigurator;
        }

        public static EndpointConfigurator Run(this EndpointConfigurator endpointConfigurator, Container container,
            Func<IFactory, IBusControl> busFactory)
        {
            return endpointConfigurator
                .UseSimpleInjector(container, busFactory)
                .RunTopshelf<IService>(hostConfigurator => hostConfigurator.AddConfigurator(new SimpleInjectorHostBuilderConfigurator(container)),
                    serviceConfigurator => serviceConfigurator.ConstructUsingSimpleInjector());
        }


    }
}