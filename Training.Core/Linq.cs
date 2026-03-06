using Bogus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public class Linq
    {
        public void test()
        {
            var students = GenerateStudents();
            
            //不及格的學生
            var failedStudents = from student in students
                                 where student.Score < 60
                                 orderby student.Score
                                 select student;

            var failedStudents_Lambda = students
                                        .Where(student => student.Score < 60)
                                        .OrderBy(student => student.Score);
            Console.WriteLine("=====不及格的學生=====");
            foreach(var s in failedStudents)
            {
                Console.WriteLine($"名字：{s.Name}\t分數：{s.Score}");
            }

            //所有學生名字
            
            var studentNames = (from student in students
                                select student.Name).ToArray();

            var studentNames_Lambda = students
                                      .Select(student => student.Name)
                                      .ToArray();
            Console.WriteLine("=====所有學生的名字=====");
            foreach(string n in studentNames)
            {
                Console.WriteLine($"{n}");
            }
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
            Console.WriteLine("=====各班平均分數=====");
            foreach (var item in gradeScore)
            {
                Console.WriteLine($"班級: {item.grade}cd 平均分數: {item.averageScore:F2}");
            }
        }
        public Dictionary<string, double> CalculateAverageScoreByClass(List<Student> students)
        {
            if (students == null || students.Count == 0)
                throw new ArgumentException("Student list cannot be empty");

            var result = students
                .GroupBy(s => s.Grade)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(s => s.Score)
                );

            return result;
        }

        public List<Student> GenerateStudents()
        {
            Random random = new Random();
            string[] grades = ["A", "B"];
            var faker = new Faker("en"); // Bogus套件 生成假資料

            //建立學生資料
            List<Student> students = new List<Student>();
            for (int i = 0; i < 50; i++)
            {
                Student student = new Student($"{faker.Name.FirstName()}", random.Next(1, 101), grades[random.Next(0, 2)]);
                students.Add(student);
            }
            return students;
        }
    }
    public class Student(string name, int score, string grade)
    {
        public string Name { get; set; } = name;
        public int Score { get; set; } = score;
        public string Grade { get; set; } = grade;
    }
}
