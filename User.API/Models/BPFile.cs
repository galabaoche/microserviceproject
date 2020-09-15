using System;

namespace User.API.Models
{
    public class BPFile
    {
        /// <summary>
        /// BP Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }

        /// <summary>
        /// 上传的源文件地址
        /// </summary>
        /// <value></value>
        public string OriginFilePath { get; set; }

        /// <summary>
        /// 格式转换后的文件地址
        /// </summary>
        /// <value></value>
        public string FormatFilePath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime CreatedTime { get; set; }
    }
}