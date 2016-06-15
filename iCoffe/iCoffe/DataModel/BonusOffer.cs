using System;

namespace iCoffe.Shared
{
    /// <summary>
    /// �������� �����������
    /// </summary>
    public class BonusOffer
    {
        /// <summary>
        /// ������������� �����������
        /// </summary>
        //[Key]
        public Guid Id { get; set; }

        /// <summary>
        /// ������������� ����� <see cref="GeoLocDataModel.Direct.BonusScheme.Id"/>
        /// </summary>
        //[Required]
        public int Scheme_Id { get; set; }

        /// <summary>
        /// ������������� ������� <see cref="GeoLocDataModel.Direct.Obj.Id"/>
        /// </summary>
        //[Required]
        public Int64 Obj_Id { get; set; }


        /// <summary>
        /// ������������� �������� ������� <see cref="GeoLocDataModel.Direct.BonusItem.Id"/>
        /// </summary>
        //[Required]
        public int Item_Id { get; set; }


        /// <summary>
        /// ���������, �������� ������� �����������
        /// </summary>
        //[Required]
        //[StringLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// �������� ������� �����������
        /// </summary>
        //[Required]
        public string Descr { get; set; }


        /// <summary>
        /// ���� � ����� ������ �������� �����������. ���� �� ����������, �� ��� �����������
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// ���� � ����� ���������� �������� �����������. ���� �� ����������, �� ��� �����������
        /// </summary>
        public DateTime? ValidUntil { get; set; }


        /// <summary>
        /// ������� ��������� = 11 (��)
        /// </summary>
        //[Required]
        public short Meas_Id { get; set; }


        /// <summary>
        /// ���������� �������, � �������� ���������
        /// </summary>
        //[Required]
        public float Qnt { get; set; }


        /// <summary>
        /// ���� �������, � �������� ������
        /// </summary>
        //[Required]
        public float Price { get; set; }



        /// <summary>
        /// ������� ����������
        /// </summary>
        public bool IsActive { get; set; }
    }
}
