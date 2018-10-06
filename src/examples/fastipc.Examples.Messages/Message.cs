using fastipc.Message;
using ProtoBuf;

namespace fastIpc.Examples.Messages {
	[ProtoContract]
	public class Ping : Message { }

	[ProtoContract]
	public class Pong : Message { }
}
