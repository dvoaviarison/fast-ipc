namespace fastipc.Bus {
	public interface IHandleMessage {
		void Handle(Message.Message msg);
	}
}
