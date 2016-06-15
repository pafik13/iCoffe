using System;

namespace iCoffe.Shared
{
    /// <summary>
    /// Бонусное предложение
    /// </summary>
    public class BonusOffer
    {
        /// <summary>
        /// Идентификатор предложения
        /// </summary>
        //[Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор схемы <see cref="GeoLocDataModel.Direct.BonusScheme.Id"/>
        /// </summary>
        //[Required]
        public int Scheme_Id { get; set; }

        /// <summary>
        /// Идентификатор объекта <see cref="GeoLocDataModel.Direct.Obj.Id"/>
        /// </summary>
        //[Required]
        public Int64 Obj_Id { get; set; }


        /// <summary>
        /// Идентификатор бонусной позиции <see cref="GeoLocDataModel.Direct.BonusItem.Id"/>
        /// </summary>
        //[Required]
        public int Item_Id { get; set; }


        /// <summary>
        /// Заголовок, название позиции предложения
        /// </summary>
        //[Required]
        //[StringLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// Описание позиции предложения
        /// </summary>
        //[Required]
        public string Descr { get; set; }


        /// <summary>
        /// Дата и время начала действия предложения. Если не определена, то без ограничения
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Дата и время завершения действия предложения. Если не определена, то без ограничения
        /// </summary>
        public DateTime? ValidUntil { get; set; }


        /// <summary>
        /// Единица измерения = 11 (шт)
        /// </summary>
        //[Required]
        public short Meas_Id { get; set; }


        /// <summary>
        /// Количество позиции, в единицах измерения
        /// </summary>
        //[Required]
        public float Qnt { get; set; }


        /// <summary>
        /// Цена позиции, в бонусных баллах
        /// </summary>
        //[Required]
        public float Price { get; set; }



        /// <summary>
        /// Признак активности
        /// </summary>
        public bool IsActive { get; set; }
    }
}
