namespace QwirkleLib {
	public enum Color {
		Red, Orange, Yellow, Green, Blue, Purple
	}
	public enum Shape {
		Circle, Square, Diamond, Starburst, Clover, X
	}

	/// <summary>
	/// A piece that represents a Qwirkle tile.
	/// </summary>
	public class Tile {
		public Color Color {
			get;
			private set;
		}
		public Shape Shape {
			get;
			private set;
		}

		public Tile(Color color, Shape shape) {
			this.Color = color;
			this.Shape = shape;
		}

		public bool IsCompatibleWith(Tile tile) {
			// Same color different shape or
			// different color same shape
			return (tile != null &&
					(((this.Color == tile.Color) && (this.Shape != tile.Shape)) ||
					 ((this.Color != tile.Color) && (this.Shape == tile.Shape))));
		}

		public override bool Equals(object obj) {
			if(obj == null) {
				return false;
			}

			var tile = obj as Tile;
			if((object)tile == null) {
				return false;
			}

			return this.Color == tile.Color && this.Shape == tile.Shape;
		}

		public bool Equals(Tile tile) {
			if((object)tile == null) {
				return false;
			}

			return this.Color == tile.Color && this.Shape == tile.Shape;
		}

		public static bool operator ==(Tile a, Tile b) {
			if(ReferenceEquals(a, b)) {
				return true;
			}

			if((object)a == null || (object)b == null) {
				return false;
			}

			return ((a.Color == b.Color) && (a.Shape == b.Shape));
		}

		public static bool operator !=(Tile a, Tile b) {
			return !(a == b);
		}

		public override int GetHashCode() {
			int hash = 13;

			hash = (hash * 7) + this.Color.GetHashCode();
			hash = (hash * 7) + this.Shape.GetHashCode();

			return hash;
		}
	}
}
