using System;
using fastipc.Message;
using ProtoBuf;

namespace fastIpc.Example.Messages {
	[ProtoContract]
	public class Ping : Message { }

	[ProtoContract]
	public class Pong : Message { }
}
