namespace OrderMaking.Models
{
    public class Category : Model
    {
        public string Name { get; set; }  

        public Category ParentCategory { get; set; }

        public long ParentId { get; set; }

        public string Description { get; set; }
    }
}
