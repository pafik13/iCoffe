using System;

namespace iCoffe.Shared
{
    /// <summary>
    /// ��������� ��������� �����������
    /// </summary>
    public enum BonusOfferStatus
    {
        /// <summary>
        /// �������� ��� �������������
        /// </summary>
        Available = 1,

        /// <summary>
        /// ���������������, �� �� ��������
        /// </summary>
        Reserved = 2,

        /// <summary>
        /// ��������
        /// </summary>
        Approved = 3
    }

    /// <summary>
    /// �������� �����������
    /// </summary>
    public class BonusOffer
    {
        /// <summary>
        /// ������������� �����������
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ����, ��������������� �� �����
        /// </summary>
        public string AggregatedKey { get; set; }

        /// <summary>
        /// �����������, �����
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ���������, �������� ������� �����������
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// �������� ������� �����������
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ������������� ���� <see cref="Cafe.Id"/>
        /// </summary>
        public int CafeId { get; set; }

        /// <summary>
        /// ��������� ��������� �����������
        /// </summary>
        public BonusOfferStatus BonusOfferStatus { get; set; }

        /// <summary>
        /// ������ ��������� �����������
        /// </summary>
        public int BonusOfferGroupId { get; set; }

        /// <summary>
        /// ��������� ��������� �����������
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// ���� � ����� ������ �������� �����������. ���� �� ����������, �� ��� �����������
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// ���� � ����� ���������� �������� �����������. ���� �� ����������, �� ��� �����������
        /// </summary>
        public DateTime? ValidUntil { get; set; }

        /// <summary>
        /// ������� ����������
        /// </summary>
        public bool IsActive { get; set; }
    }
}
