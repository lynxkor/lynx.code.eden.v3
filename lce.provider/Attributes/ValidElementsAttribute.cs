/* file name：leap.crm.app.api.Filters.ValidElementsAttribute.cs
* author：lynx lynx.kor@163.com @ 2020/01/07 13:20:47
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for ValidElementsAttribute
* revision：
*
*/

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace lce.provider.Validation
{
    /// <summary>
    /// action：ValidElementsAttribute
    /// </summary>
	public class ValidElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;

        /// <summary>
        /// 序列最小数量
        /// </summary>
        /// <param name="minElements"></param>
        public ValidElementsAttribute(int minElements = 1)
        {
            _minElements = minElements;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }
}