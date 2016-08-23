namespace iCoffe.Shared
{
    /// <summary>
    /// Кафе
    /// </summary>
    public class Cafe
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Идентификатор гео-локации <see cref="GeoLocation"/>
		/// </summary>
		public long GeoLocationId { get; set; }

        public GeoLocation GeoLocation { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string FullAddress { get; set; }

		/// <summary>
		/// Контактные данные
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// URL логотипа
		/// </summary>
		public string LogoUrl { get; set; }

		/// <summary>
		/// URL картинки
		/// </summary>
		public string ImageUrl { get; set; }
    }
}
