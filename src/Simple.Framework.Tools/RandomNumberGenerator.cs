using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Framework.Tools
{
    public class RandomNumberGenerator
    {
        /// <summary>
        /// 随机数
        /// </summary>
        private readonly System.Random _random;

        /// <summary>
        /// 初始化随机数字生成器
        /// </summary>
        public RandomNumberGenerator()
        {
            _random = new System.Random(Guid.NewGuid().GetHashCode());
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public int Generate(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
