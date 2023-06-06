//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DriveBuddyWpfApp.MVVM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Lesson
    {
        public int LessonID { get; set; }

        public System.DateTime LessonDate { get; set; }

        public string Title { get; set; }

        public Nullable<byte> HoursNumber { get; set; }

        public int CourseCategoryID { get; set; }

        public int InstructorID { get; set; }

        public int CarID { get; set; }

        public int StudentID1 { get; set; }

        public Nullable<int> StudentID2 { get; set; }

        public Nullable<int> StudentID3 { get; set; }
    
        public virtual Car Car { get; set; }

        public virtual Category Category { get; set; }

        public virtual Instructor Instructor { get; set; }

        public virtual Student Student { get; set; }

        public virtual Student Student1 { get; set; }

        public virtual Student Student2 { get; set; }

        public string StudentsFullName => GetStudentsFullName();

        private string GetStudentsFullName()
        { 
            var students = new List<Student>();

            if (StudentID1 != 0 && Student != null)
                students.Add(Student);

            if (StudentID2.HasValue && Student1 != null)
                students.Add(Student1);

            if (StudentID3.HasValue && Student2 != null)
                students.Add(Student2);

            return string.Join(", ", students.Select(s => s.FullName));
        }
    }
}
