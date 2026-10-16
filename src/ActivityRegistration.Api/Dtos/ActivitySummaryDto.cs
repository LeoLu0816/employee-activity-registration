namespace ActivityRegistration.Api.Dtos
{
    public class ActivitySummaryDto
    {
        /// <summary>
        /// 編號
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public required string Category { get; set; }

        /// <summary>
        /// 人數
        /// </summary>
        public int Capacity { get; set; }
    }
}
