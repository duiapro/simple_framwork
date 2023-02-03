namespace Simple.Framework.Tools.Province
{
    public class ProvinceModel
    {
        /// <summary>
        /// Lable
        /// </summary>
        public string Label { get; set; } = null!;

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; } = null!;

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<ProvinceModel> Children { get; set; } = new();
    }
}
