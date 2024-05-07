public class CatApiResponse
{
    public List<Cat> AllCats { get; set; }
}

public class Cat
{
    public string PetID { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public string Age { get; set; }
    public string Location { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
