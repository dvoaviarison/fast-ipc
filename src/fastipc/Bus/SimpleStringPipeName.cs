namespace fastipc.Bus
{
	public class SimpleStringPipeName : IPipeName {
		private readonly Side _side;
		private readonly string _name;

		public SimpleStringPipeName(
			string name,
			Side side = Side.In) {
			_side = side;
			_name = name;
		}

		public string Read => _side == Side.In ? $"In-{_name}" : $"Out-{_name}";

		public string Write => _side == Side.In ? $"Out-{_name}" : $"In-{_name}";
	}
}
