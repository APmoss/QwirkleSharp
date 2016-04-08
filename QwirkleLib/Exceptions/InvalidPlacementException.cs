using System;

namespace QwirkleLib.Exceptions {
	class InvalidPlacementException : Exception {
		public InvalidPlacementException()
			: base() { }
		public InvalidPlacementException(string message)
			: base(message) { }
		public InvalidPlacementException(string message, Exception inner)
			: base(message, inner) { }
	}
}
