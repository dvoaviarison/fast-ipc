using System.Diagnostics;

namespace fastipc.Bus
{
	public class ParentChildProcessPipeName : IPipeName {
		private readonly Side _side;
		private readonly int _parentProcessId;

		public ParentChildProcessPipeName(Side side = Side.In) {
			_side = side;
			_parentProcessId = _side == Side.In ? Process.GetCurrentProcess().Parent().Id : Process.GetCurrentProcess().Id;
		}

		public string Read => _side == Side.In ? $"In-{_parentProcessId}" : $"Out-{_parentProcessId}";

		public string Write => _side == Side.In ? $"Out-{_parentProcessId}" : $"In-{_parentProcessId}";
	}
}