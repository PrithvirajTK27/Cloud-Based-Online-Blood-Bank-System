using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetAppSqlDb.Models
{
    public class Todo
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Donated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DonatedDate { get; set; }

        [Display(Name = "Gender")]
        public Gender DonorGender { get; set; }

        [MinAge(18)] // 18 is the parameter of constructor. 
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UserBirthDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Blood Group")]
        public Groups Blood_Group { get; set; }

        [Display(Name = "Polarity")]
        public pol Pola { get; set; }

        [Display(Name = "Storage Area")]
        public string Storage_Area { get; set; }
        public string SUBJECTID { get; set; }
        public string RSUBJECTID { get; set; }
        [Display(Name = "Request")]
        public bool requst { get; set; }
        [Display(Name = "Reply")]
        public bool rply { get; set; }
        [Display(Name = "Type")]
        public typ type { get; set; }
    }
}
public enum Gender
{
    Select,
    Male,
    Female
}

public enum Groups
{
    Select,
    O,
    A,
    B,
    AB
}

public enum pol
{
    Select,
    POSITIVE,
    NEGATIVE
}

public enum typ
{
    DONATION,
    REQUEST
}

public class MinAge : ValidationAttribute
{
    private int _Limit;
    public MinAge(int Limit)
    { // The constructor which we use in modal.
        this._Limit = Limit;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime bday = DateTime.Parse(value.ToString());
        DateTime today = DateTime.Today;
        int age = today.Year - bday.Year;
        if (bday > today.AddYears(-age))
        {
            age--;
        }
        if (age < _Limit)
        {
            var result = new ValidationResult("Sorry you are not old enough");
            return result;
        }


        return null;

    }
}