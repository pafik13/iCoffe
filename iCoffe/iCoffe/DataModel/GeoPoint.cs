namespace iCoffe.Shared
{
	/// <summary>
	/// Структура определеяет точку на поверхности Земли
	/// </summary>
	public struct GeoPoint
	{
		/// <summary>
		/// Широта
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// Долгота
		/// </summary>
		public double Longitude { get; set; }
	}
}
