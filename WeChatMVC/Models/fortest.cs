namespace WeChatMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("fortest")]
    public partial class fortest
    {
        [Key]
        [StringLength(50)]
        public string wechatid { get; set; }

        [StringLength(10)]
        public string studentnum { get; set; }

        [StringLength(50)]
        public string ty_password { get; set; }
    }
}
