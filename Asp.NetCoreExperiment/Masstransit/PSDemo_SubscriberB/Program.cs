﻿using MassTransit;
using System;
using System.Threading.Tasks;

namespace PSDemo_SubscriberB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SubscriberB");

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), hst =>
                {
                    hst.Username("guest");
                    hst.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "gswPSB", e =>
                {
                    e.Consumer<GreetingEventConsumerA>();

                });
            });


            bus.Start();

            Console.WriteLine("Listening for Greeting events.. Press enter to exit");
            Console.ReadLine();

            bus.Stop();
        }
    }
    public class GreetingEventConsumerA : IConsumer<PSDemo_Entity.Entity>
    {
        public async Task Consume(ConsumeContext<PSDemo_Entity.Entity> context)
        {
            await Console.Out.WriteLineAsync($"receive PSDemo_SubscriberB:  {context.Message.Name}  {context.Message.Time}");
        }
    }
}
