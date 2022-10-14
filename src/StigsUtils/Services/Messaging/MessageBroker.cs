// // Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
// using Microsoft.Extensions.Logging;
// namespace StigsUtils.Services.Messaging;
//
// public interface IMessageBroker {
// 	Task Publish<T>(IMessage<Guid,T> message);
//
// 	IMessageBroker Subscribe<T>(IMessageHandler<T> messageHandler);
// }
//
// internal sealed class MessageBroker : IMessageBroker {
// 	public MessageBroker(ILoggerFactory loggerFactory, IServiceProvider serviceProvider) {
// 		_loggerFactory = loggerFactory;
// 		_serviceProvider = serviceProvider;
// 	}
//
// 	private readonly ILoggerFactory _loggerFactory;
// 	private readonly IServiceProvider _serviceProvider;
// 	private readonly Dictionary<Type, IMessageHandler> _subscribers = new();
//
// 	public Task Publish<T>(IMessage<T> message) where T : IEquatable<T> => Task.CompletedTask;
//
// 	public IMessageBroker Subscribe<T>(IMessageHandler<T> messageHandler) {
// 		lock (_subscribers) _subscribers.Add(typeof(IMessageHandler<T>), messageHandler);
// 		return this;
// 	}
// }