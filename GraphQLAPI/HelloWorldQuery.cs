using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

public class HelloWorldQuery: ObjectGraphType
{
    public HelloWorldQuery()
    {
        Field<StringGraphType>(
            name: "hello",
            resolve: context => "world"
        );

        Field<StringGraphType>(
            name:"howdy",
            resolve:context => "universe"
        );

        Field<ItemType>(
            "item",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "barcode" }),
            resolve: context =>
            {
                var barcode = context.GetArgument<string>("barcode");
                return new DataSource().GetItemByBarCode(barcode);
            }
        );
    }
}

public class Item
{
    public string Barcode { get; set; }
    public string Title { get; set; }
    public decimal SellingPrice { get; set; }
}

public class ItemType : ObjectGraphType<Item>
{
    public ItemType()
    {
        Field(i => i.Barcode);
        Field(i => i.Title);
        Field(i => i.SellingPrice);
    }
}

public class DataSource
{
    public IList<Item> Items { get; set; }

    public DataSource() => Items = new List<Item>
        {
            new Item{Barcode = "123",Title = "Headphone", SellingPrice = 50},
            new Item{Barcode = "456",Title = "Keyboard",SellingPrice = 50},
            new Item{Barcode = "789",Title = "Monitor",SellingPrice = 100}
        };

    public Item GetItemByBarCode(string barcode) => Items.First(i => i.Barcode.Equals(barcode));
}