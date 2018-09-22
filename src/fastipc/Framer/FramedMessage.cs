using ProtoBuf;

namespace fastipc.Framer
{
	[ProtoContract]
	public class FramedMessage
	{
		[ProtoMember(1)]
		public byte[] Data { get; set; }

		[ProtoMember(2)]
		public string Type { get; set; }
	}
}
