namespace fastipc.Bus
{
	public interface IPipeName {
		string Read { get; }
		string Write { get; }
	}
}