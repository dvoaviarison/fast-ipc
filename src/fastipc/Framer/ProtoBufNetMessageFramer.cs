using ProtoBuf;
using System;
using System.IO;

namespace fastipc.Framer
{
	public class ProtoBufNetMessageFramer : IMessageFramer
	{
		public FramedMessage Frame<T>(T message) where T : Message.Message
		{
			var type = typeof(T);
			using (var stream = new MemoryStream())
			{
				Serializer.Serialize(stream, message);
				var framedMessage = new FramedMessage
				{
					Type = type.AssemblyQualifiedName,
					Data = stream.ToArray()
				};

				return framedMessage;
			}
		}

		public Message.Message UnFrame(FramedMessage framedMessage)
		{
			using (var stream = new MemoryStream(framedMessage.Data))
			{
				var type = Type.GetType(framedMessage.Type);
				var msg = (Message.Message)Serializer.Deserialize(type, stream);
				return msg;
			}
		}
	}
}