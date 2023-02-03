using Newtonsoft.Json;
using System.Text;

namespace Simple.Framework.Tools.Province
{
    public class ProvinceHelper
    {
        /// <summary>
        /// 服务文件常量
        /// </summary>
        private const string PROVINCE_FILE_PATH = "wwwroot/province.json";

        /// <summary>
        /// 省市区属性数据
        /// </summary>
        private static List<ProvinceModel> _province;

        /// <summary>
        /// 省市区树形数据
        /// </summary>
        public static IReadOnlyList<ProvinceModel> Data => _province;

        /// <summary>
        /// 构造函数
        /// </summary>
        static ProvinceHelper()
        {
            _province = GetProvince();
        }

        /// <summary>
        /// 根据区县code码获取省市区详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static (string? province, string? city, string? district) Get(string code)
        {
            var fatherCode = code.Substring(0, 2) + "0000";
            var cityCode = code.Substring(0, 4) + "00";

            var province = _province.Find(x => x.Value == fatherCode);
            var city = province?.Children.Find(x => x.Value == cityCode);
            if (city == null)
            {
                city = province?.Children.Find(x => x.Value == code);
            }

            var district = city?.Children.Find(x => x.Value == code);

            return (province?.Label, city?.Label, district?.Label);
        }

        /// <summary>
        /// 根据省市区地址反推Code
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <returns></returns>
        public static string GetCode(string province, string city, string district)
        {
            var provinceModel = _province.Find(x => x.Label == province);
            var cityModel = provinceModel.Children.Find(x => x.Label == city);
            if (cityModel == null)
            {
                return provinceModel.Value;
            }
            var districtModel = cityModel.Children.Find(x => x.Label == district);
            if (districtModel == null)
            {
                return cityModel.Value;
            }
            return districtModel.Value;
        }

        /// <summary>
        /// 获取省市区文件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static List<ProvinceModel> GetProvince()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var services = System.IO.File.ReadAllText(PROVINCE_FILE_PATH, Encoding.GetEncoding("UTF-8"));

            if (string.IsNullOrWhiteSpace(services))
            {
                throw new Exception("province file is null");
            }

            return JsonConvert.DeserializeObject<List<ProvinceModel>>(services);
        }
    }
}
