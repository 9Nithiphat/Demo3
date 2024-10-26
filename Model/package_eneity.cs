using Demo2.Context;
using System.ComponentModel.DataAnnotations;

namespace Demo2.Model
{
    public class tbl_package
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<tbl_package_menu_condition> package_menu_condition { get; set; }
        public virtual ICollection<tbl_user_package> user_package { get; set; }
    }

    public class tbl_menu
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<tbl_menu_feature> menu_features { get; set; }
        public virtual ICollection<tbl_package_menu_condition> package_menu_condition { get; set; }
        public virtual ICollection<tbl_user_package> user_package { get; set; }
    }

    public class tbl_menu_feature
    {
        [Key]
        public int Id { get; set; }
        public int menu_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public virtual tbl_menu menuId { get; set; }
        public virtual ICollection<tbl_package_menu_condition> package_menu_condition { get; set; }
    }

    public class tbl_package_menu_condition
    {
        public int package_id { get; set; }
        public int menu_id { get; set; }
        public int menu_feature_id { get; set; }
        public string condition_value { get; set; }

        public virtual tbl_package packageId { get; set; }
        public virtual tbl_menu menuId { get; set; }
        public virtual tbl_menu_feature menu_features { get; set; }
    }

    public class tbl_user
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }

        public virtual ICollection<tbl_user_package> user_package { get; set; }
    }

    public class tbl_user_package
    {
        public int user_id { get; set; }
        public int package_id { get; set; }
        public int menu_id { get; set; }
        public virtual tbl_user userId { get; set; }
        public virtual tbl_package packageId { get; set; }
        public virtual tbl_menu menuId { get; set; }
    }
    public class AddUserPackageDto
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public int MenuId { get; set; }
    }
}
