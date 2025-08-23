namespace DomainModel;

public class StudentEnrollment : Entity
{
    public Student Student { get; }
    public Enrollment Enrollment { get; }

    public StudentEnrollment(Student student, Enrollment enrollment)
    {
        Student = student;
        Enrollment = enrollment;
    }
}