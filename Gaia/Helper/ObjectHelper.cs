using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Helper
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 객체가 NULL인지 확인한다.
        /// </summary>
        /// <typeparam name="T">객체 타입</typeparam>
        /// <param name="src"></param>
        /// <returns>NULL 여부</returns>
        /// <example>
        /// <code>
        /// object src = null;
        /// bool isNull = src.IsNull();
        /// </code>
        /// </example>
        public static bool IsNull<T>(this T src)
        {

            return ReferenceEquals(src, null);
        }

        /// <summary>
        /// 객체가 Not NULL인지 확인한다.
        /// </summary>
        /// <typeparam name="T">객체 타입</typeparam>
        /// <param name="src"></param>
        /// <returns>Not NULL여부</returns>
        /// <example>
        /// <code>
        /// object src = null;
        /// bool isNull = src.IsNotNull();
        /// </code>
        /// </example>
        public static bool IsNotNull<T>(this T src)
        {
            return !ReferenceEquals(src, null);
        }
    }
}
