using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace DomainModel;

public class Student : Entity
{
    public Email Email { get; }
    public string Name { get; private set; }
    public Address[] Addresses { get; private set; }

    private readonly List<StudentEnrollment> _enrollments = new List<StudentEnrollment>();
    public virtual IReadOnlyList<StudentEnrollment> Enrollments => _enrollments.ToList();

    protected Student()
    {
    }

    public Student(Email email, string name, Address[] addresses)
        : this()
    {
        Email = email;
        EditPersonalInfo(name, addresses);
    }

    public void EditPersonalInfo(string name, Address[] addresses)
    {
        Name = name;
        Addresses = addresses;
    }

    public virtual Result<object, Error> Enroll(Enrollment[] enrollments)
    {
        if (_enrollments.Count + enrollments.Length > 2)
            return Errors.Student.TooManyEnrollments();

        StudentEnrollment existingEnrollment = _enrollments
            .FirstOrDefault(x => enrollments.Any(e => x.Enrollment == e));

        if (existingEnrollment != null)
            return Errors.Student.AlreadyEnrolled(existingEnrollment.Enrollment.Course.Name);

        foreach (Enrollment enrollment in enrollments)
        {
            _enrollments.Add(new StudentEnrollment(this, enrollment));
        }

        return new object(); // Unit class
    }
}

public class Address : Entity
{
    public string Street { get; }
    public string City { get; }
    public State State { get; }
    public string ZipCode { get; }

    private Address(string street, string city, State state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public static Result<Address, Error> Create(
        string street, string city, string state, string zipCode, string[] allStates)
    {
        State stateObject = State.Create(state, allStates).Value;

        street = (street ?? "").Trim();
        city = (city ?? "").Trim();
        zipCode = (zipCode ?? "").Trim();

        if (street.Length < 1 || street.Length > 100)
            return Errors.General.InvalidLength("street");

        if (city.Length < 1 || city.Length > 40)
            return Errors.General.InvalidLength("city");

        if (zipCode.Length < 1 || zipCode.Length > 5)
            return Errors.General.InvalidLength("zip code");

        return new Address(street, city, stateObject, zipCode);
    }
}