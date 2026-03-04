using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public class Linq
    {
        public void test()
        {
            Random random = new Random();
            string[] grades = ["A", "B"];

            //建立學生資料
            List<Student> students = new List<Student>();
            for (int i = 0; i < 50; i++)
            {
                Student student = new Student($"Student{i + 1}", random.Next(1, 101), grades[random.Next(0, 2)]);
                students.Add(student);
            }
            //不及格的學生
            var failedStudents = from student in students
                                 where student.Score < 60
                                 orderby student.Score
                                 select student;

            var failedStudents_Lambda = students
                                        .Where(student => student.Score < 60)
                                        .OrderBy(student => student.Score);

            //所有學生名字
            var studentNames = (from student in students
                                select student.Name).ToArray();

            var studentNames_Lambda = students
                                      .Select(student => student.Name)
                                      .ToArray();
            //AB班各自的平均分數
            var gradeScore = from student in students
                             group student by student.Grade into studentGroup
                             select new
                             {
                                 grade = studentGroup.Key,
                                 averageScore = (from student2 in studentGroup
                                                 select student2.Score).Average()
                             };

            var gradeScore_Lambda = students
                                    .GroupBy(student => student.Grade)
                                    .Select(student => new
                                    {
                                        grade = student.Key,
                                        averageScore = student.Average(s => s.Score)
                                    });
            
        }
    }
    public class Student(string name, int score, string grade)
    {
        public string Name { get; set; } = name;
        public int Score { get; set; } = score;
        public string Grade { get; set; } = grade;
    }
}
