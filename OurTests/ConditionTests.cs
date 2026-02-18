using DbManager;

namespace OurTests
{
    public class ConditionTests
    {
        //TODO DEADLINE 1A : Create your own tests for Condition
        /*
        [Fact]
        public void Test1()
        {

        }
        */
    [Fact]
    public void IsTrue_EqualString()
    {
        var condition = new Condition("Name", "=", "Pepe");
        bool result = condition.IsTrue("Pepe", ColumnDefinition.DataType.String);
        Assert.True(result);
    }
    [Fact]
    public void IsTrue_MayorIntTrue()
    {
        var condition = new Condition("Age", ">", "50");
        bool result = condition.IsTrue("67", ColumnDefinition.DataType.Int);
        Assert.True(result);
    }

    [Fact]
    public void IsTrue_MayorIntFalse()
    {
        var condition = new Condition("Age", ">", "50");
        bool result = condition.IsTrue("25", ColumnDefinition.DataType.Int);
        Assert.False(result);
    }

    [Fact]
    public void IsTrue_MenorOIgualDoule()
    {
        var condition = new Condition("Height", "<=", "1.70");
        bool result = condition.IsTrue("1.67", ColumnDefinition.DataType.Double);
        Assert.True(result);
    }

    [Fact]
    public void IsTrue_NotEqualString()
    {
        var condition = new Condition("Name", "!=", "Pepe");
        bool result = condition.IsTrue("Juan", ColumnDefinition.DataType.String);
        Assert.True(result);
    }


    [Fact]
    public void IsTrue_MasPequeñoInt()
    {
        var condition = new Condition("Age", "<", "50");
        bool result = condition.IsTrue("25", ColumnDefinition.DataType.Int);
        Assert.True(result);
    }
        
    }
}