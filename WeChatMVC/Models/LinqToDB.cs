namespace WeChatMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LinqToDB : DbContext
    {
        public LinqToDB()
            : base("name=LinqToDB")
        {
        }

        public virtual DbSet<fortest> fortest { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<fortest>()
                .Property(e => e.wechatid)
                .IsUnicode(false);

            modelBuilder.Entity<fortest>()
                .Property(e => e.studentnum)
                .IsUnicode(false);

            modelBuilder.Entity<fortest>()
                .Property(e => e.ty_password)
                .IsUnicode(false);
        }
    }
}
