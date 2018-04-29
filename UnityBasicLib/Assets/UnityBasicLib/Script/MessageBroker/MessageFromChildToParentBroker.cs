using System;
using UniRx;
using UnityEngine;

namespace UnityBasicLib
{
    public interface IMessageToParentPublisher
    {
        void Publish<T>(Component child, T message);
    }

    public interface IMessageFromChildReceiver
    {
        IObservable<T> Receive<T>(Component parent);
    }

    public interface IMessageFromChildToParentBroker : IMessageToParentPublisher, IMessageFromChildReceiver
    {
    }

    public class MessageFromChildToParentBroker : IMessageFromChildToParentBroker, IDisposable
    {
        readonly MessageBroker messageBroker = new MessageBroker();

        public void Publish<T>(Component child, T message)
        {
            messageBroker.Publish(new MessageWithChild<T>(child, message));
        }

        public IObservable<T> Receive<T>(Component parent)
        {
            return messageBroker.Receive<MessageWithChild<T>>()
                .Where(m => m.Child.transform.IsChildOf(parent.transform))
                .Select(m => m.Message);
        }

        public void Dispose()
        {
            messageBroker.Dispose();
        }

        struct MessageWithChild<T>
        {
            public Component Child { get; }
            public T Message { get; }

            public MessageWithChild(Component child, T message)
            {
                Child = child;
                Message = message;
            }
        }
    }
}