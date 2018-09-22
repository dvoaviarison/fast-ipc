namespace fastipc.Framer
{
	public interface IMessageFramer
	{
		FramedMessage Frame<T>(T message) where T : Message.Message;
		Message.Message UnFrame(FramedMessage framedMessage);
	}
}