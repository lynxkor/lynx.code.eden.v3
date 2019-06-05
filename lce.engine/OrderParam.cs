// action：
// file name： OrderParam.cs
// author：lynx <lynx.kor@163.com> @ 2019/6/5 11:09
// copyright (c) 2019 lynxce.com
// desc：
// > add description for OrderParam
// revision：
//
namespace lce.engine
{
    /// <summary>
    /// Order parameter.
    /// </summary>
    public class OrderParam
    {
        public OrderParam() { }

        public OrderParam(string field) : this()
        {
            this.Field = field;
        }

        public OrderParam(string field, bool isAsc) : this(field)
        {
            this.IsAsc = isAsc;
        }

        public string Field { get; set; } = "Id";

        public bool IsAsc { get; set; } = false;
    }
}
