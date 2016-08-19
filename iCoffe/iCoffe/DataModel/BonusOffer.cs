using System;

namespace iCoffe.Shared
{
    /// <summary>
    /// Состояние бонусного предложения
    /// </summary>
    public enum BonusOfferStatus
    {
        /// <summary>
        /// Доступно для использования
        /// </summary>
        Available = 1,

        /// <summary>
        /// Зарезервировано, но не одобрено
        /// </summary>
        Reserved = 2,

        /// <summary>
        /// Одобрено
        /// </summary>
        Approved = 3
    }

    /// <summary>
    /// Бонусное предложение
    /// </summary>
    public class BonusOffer
    {
        /// <summary>
        /// Идентификатор предложения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ключ, сгенерированный по полям
        /// </summary>
        public string AggregatedKey { get; set; }

        /// <summary>
        /// Обозначение, метка
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Заголовок, название позиции предложения
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание позиции предложения
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор кафе <see cref="Cafe.Id"/>
        /// </summary>
        public int CafeId { get; set; }

        /// <summary>
        /// Состояние бонусного предложения
        /// </summary>
        public BonusOfferStatus BonusOfferStatus { get; set; }

        /// <summary>
        /// Группа бонусного предложения
        /// </summary>
        public int BonusOfferGroupId { get; set; }

        /// <summary>
        /// Стоимость бонусного предложения
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Дата и время начала действия предложения. Если не определена, то без ограничения
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Дата и время завершения действия предложения. Если не определена, то без ограничения
        /// </summary>
        public DateTime? ValidUntil { get; set; }

        /// <summary>
        /// Признак активности
        /// </summary>
        public bool IsActive { get; set; }
    }
}
