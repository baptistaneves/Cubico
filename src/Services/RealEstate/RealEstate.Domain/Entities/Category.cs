namespace RealEstate.Domain.Entities;

public class Category : Aggregate<CategoryId>
{
    public string Name { get; private set; }

    public static Category Create(CategoryId id, string  name, string createdBy)
    {
        var category = new Category
        {
            Id = id,
            Name = name,
            CreatedBy = createdBy
        };

        CategoryValidator.ValidateCategory(category);

        return category;
    }

    public void Update(string name, string lastModifiedBy)
    {
        Name = name;
        LastModifiedBy = lastModifiedBy;

        CategoryValidator.ValidateCategory(this);
    }
}