using System;
using ProtoBuf;

namespace fastipc.Message {
	[ProtoContract]
	public abstract class Message
	{
		[ProtoMember(1)]
		public Guid MsgId { get; }

		protected Message()
		{
			MsgId = Guid.NewGuid();
		}
	}
}
