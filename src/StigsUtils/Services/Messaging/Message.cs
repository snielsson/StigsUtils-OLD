// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Services.Messaging;

public interface IMessage : IEquatable<IMessage> { }

public interface IMessage<TId> : IMessage, IEquatable<IMessage<TId>> where TId : IEquatable<TId>  {
	TId Id { get; }
}

public abstract class MessageBase<TId> : IMessage<TId> where TId : IEquatable<TId> {
	protected MessageBase(TId id) => Id = id;
	public TId Id { get; }
	public bool Equals(IMessage<TId>? other) => other != null && Id.Equals(other.Id);
	public abstract bool Equals(IMessage? other);
	public override string ToString() => $"Message: {Id}";
}

public interface IMessage<TId, out TPayload> : IMessage<TId> where TId : IEquatable<TId> {
	public TPayload Payload { get; }
}

public class Message<TId, TPayload> : MessageBase<TId>, IMessage<TId, TPayload> where TId : IEquatable<TId> {
	public Message(TId id, TPayload payload) : base(id) {
		Payload = payload;
	}
	public TPayload Payload { get; }
	public override bool Equals(IMessage? other) => other is IMessage<TId> x && Id.Equals(x.Id);
	public override string ToString() => $"Message: {Id}\nPayload:\n{Payload}";
}

public class Message<TPayload> : Message<Guid, TPayload> {
	public Message(TPayload payload) : base(Guid.NewGuid(), payload) { }
}

