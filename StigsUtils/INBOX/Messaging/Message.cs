// // Copyright © 2020 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.
//
// namespace StigsUtils.Messaging {
//     public interface IMessage {
//         long Id { get; }
//         Task Completion { get; }
//         ValidationError? ValidationError { get; }
//         void SetException(Exception ex);
//         void SetCancelled();
//     }
//
//     public interface IMessage<TResult> : IMessage {
//         Task<TResult> ResultAsync { get; }
//         void SetResult(TResult result);
//     }
//
//     public abstract class MessageBase {
//         private static long LastMessageId;
//         protected object? _validationResult;
//
//         protected internal MessageBase() {
//             Id = GetNextMessageId();
//         }
//
//         public long Id { get; }
//         public ValidationError? ValidationError { get; protected set; }
//
//         public virtual object? ValidationResult {
//             get => _validationResult;
//             set {
//                 _validationResult = value;
//                 if (_validationResult != null) {
//                     ValidationError = new ValidationError(_validationResult);
//                 }
//             }
//         }
//
//         internal static long GetNextMessageId() => Interlocked.Increment(ref LastMessageId);
//         public override string ToString() => $"{GetType()} message with Id {Id}.";
//     }
//
//     public abstract class MessageBase<T> : MessageBase {
//         protected readonly TaskCompletionSource<T> _taskCompletionSource = new TaskCompletionSource<T>();
//         public override object? ValidationResult {
//             set {
//                 base.ValidationResult = value;
//                 if (ValidationError != null) SetException(ValidationError);
//             }
//         }
//         public Task Completion => _taskCompletionSource.Task;
//         public Task<T> ResultAsync => _taskCompletionSource.Task;
//         public void SetException(Exception ex) => _taskCompletionSource.SetException(ex);
//         public void SetCancelled() => _taskCompletionSource.SetCanceled();
//     }
//
//     public abstract class Message : MessageBase<bool?>, IMessage {
//         protected Task SetResult(bool? result) {
//             _taskCompletionSource.SetResult(result);
//             return _taskCompletionSource.Task;
//         }
//     }
//
//     public abstract class Message<T> : MessageBase<T> {
//         public Task<T> SetResult(T result) {
//             _taskCompletionSource.SetResult(result);
//             return _taskCompletionSource.Task;
//         }
//     }}