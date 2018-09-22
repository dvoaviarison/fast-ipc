using ProtoBuf;

namespace fastipc.Message
{
	[ProtoContract]
	public class StringContentMessage : Message
	{
		[ProtoMember(1)]
		public string Content { get; }

		// Used by protobuf to reconstruct message
		public StringContentMessage() { }

		public StringContentMessage(string content)
		{
			Content = content;
		}
	}
}
