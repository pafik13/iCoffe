namespace iCoffe.Shared
{
    /// <summary>
    /// Объекты размещения / геолокации
    /// Отражается на запись в таблице GeoLocation
    /// </summary>
    public class GeoLocation
	{
		/// <summary>
		/// Идентификатор объекта
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Обозначение, метка
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Название, заголовок
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Текстовое описание, дополнительные заметки
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Идентификатор групп
		/// <see cref="Group"/>
		/// </summary>
		public long GroupId { get; set; }

		/// <summary>
		/// Координаты геолокации
		/// </summary>
		public GeoPoint GeoPoint { get; set; }
	}
}
