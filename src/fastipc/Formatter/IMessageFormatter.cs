using System.IO;

namespace fastipc.Formatter
{
	public interface IMessageFormatter
	{

		void Serialize<T>(Stream stream, T message) where T : Message.Message;

		Message.Message Deserialize(Stream stream);
	}
}