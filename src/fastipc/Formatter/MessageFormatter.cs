using System;
using System.IO;
using fastipc.Framer;
using ProtoBuf;

namespace fastipc.Formatter
{
	public class ProtobufNetFormatter : IMessageFormatter
	{
		readonly IMessageFramer _framer;
		public ProtobufNetFormatter()
		{
			_framer = new ProtoBufNetMessageFramer();
		}

		public void Serialize<T>(Stream stream, T message) where T : Message.Message
		{
			var framedMsg = _framer.Frame(message);
			Serializer.Serialize(stream, framedMsg);
		}

		public Message.Message Deserialize(Stream stream)
		{
			try
			{
				var framedMsg = Serializer.Deserialize<FramedMessage>(stream);
				var msg = _framer.UnFrame(framedMsg);
				return msg;

			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
