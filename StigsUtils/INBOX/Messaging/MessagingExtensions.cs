// // Copyright © 2020 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.
//
// namespace StigsUtils.Messaging {
// 	public static class MessagingExtensions {
// 		/// <summary>
// 		/// Register a message service for handling of messages and scan calling assembly and  register all found  message handlers and message validators. 
// 		/// </summary>
// 		/// <param name="this"></param>
// 		/// <returns></returns>
// 		public static IServiceCollection AddMessaging(this IServiceCollection @this) => @this.AddMessaging(Assembly.GetCallingAssembly());
// 		public static IServiceCollection AddMessaging(this IServiceCollection @this, params Assembly[] assemblies) => @this
// 			//Register the service provider itself, so that it will be injected as a dependency into the MessageService.
// 			.AddSingleton<IServiceProvider>(serviceProvider => serviceProvider)
// 			.AddSingleton<IMessageService, MessageService>()
// 			.AddInterfaceImplementationsAsSingletons(typeof(IMessageValidator<>))
// 			.AddInterfaceImplementationsAsSingletons(typeof(IMessageHandler<>))
// 		;
// 	}
// }