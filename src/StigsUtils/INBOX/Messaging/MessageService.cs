// // Copyright © 2020 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.
//
// namespace StigsUtils.Messaging {
//     public interface IMessageService 
//     {
//         Task<T> Handle<T>(T msg);
//     }
//     public interface IMessageValidator<in T> {
//         Task<object?> Validate(T msg);
//     }
//     public interface IMessageHandler<in T> {
//         Task Handle(T msg);
//     }
//
//     internal sealed class MessageService : IMessageService
//     {
//         private readonly IServiceProvider _serviceProvider;
//         private readonly ILogger<MessageService> _logger;
//         public MessageService(IServiceProvider serviceProvider, ILogger<MessageService> log)
//         {
//             _serviceProvider = serviceProvider;
//             _logger = log;
//         }
//
//         public async Task<T> Handle<T>(T msg){
//             var handler = _serviceProvider.GetService<IMessageHandler<T>>() ?? throw new Error($"No message handler registered for message of type {typeof(T)}.", _logger, LogLevel.Critical);
//             await Validate(msg);
//             await Task.Run(async () => {
//                 try {
//                     _logger.LogDebug($"Handling {msg}");
//                     await handler.Handle(msg);
//                     _logger.LogDebug($"Handled {msg}");
//                 }
//                 catch (Exception ex) {
//                     _logger.LogError($"Error handling {msg}:\n{new {Exception = ex.ToString()}.ToPrettyJson()}");
//                 }
//             });
//             return msg;
//         }
//         private async Task Validate<T>(T msg) {
//             var validator = _serviceProvider.GetService<IMessageValidator<T>>();
//             if (validator == null) {
//                 _logger.LogDebug($"No validator registered for {msg}.");
//                 return;
//             }
//             _logger.LogDebug($"Validating {msg}");
//             var validationError = await validator.Validate(msg);
//             if (validationError != null) {
//                 _logger.LogDebug($"Validation of {msg} failed: {validationError}");
//                 throw new ValidationError(validationError);
//             } 
//             _logger.LogDebug($"Validated {msg} successfully.");
//         }
//     }
// }