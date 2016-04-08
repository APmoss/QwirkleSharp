using System;

namespace QwirkleLib.Exceptions {
	class InvalidGroupException : Exception {
		public InvalidGroupException()
			: base() { }
		public InvalidGroupException(string message)
			: base(message) { }
		public InvalidGroupException(string message, Exception inner)
			: base(message, inner) { }
	}
}
