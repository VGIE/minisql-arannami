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
    public void IsTrue_StringComparison_Works()
    {
        var condition = new Condition("Name", "=", "Pepe");
        bool result = condition.IsTrue("Pepe", ColumnDefinition.DataType.String);
        Assert.True(result);
    }
    [Fact]
    public void IsTrue_IntGreater_Works()
    {
        var condition = new Condition("Age", ">", "50");
        bool result = condition.IsTrue("67", ColumnDefinition.DataType.Int);
        Assert.True(result);
    }

    [Fact]
    public void IsTrue_IntGreater_False()
    {
        var condition = new Condition("Age", ">", "50");
        bool result = condition.IsTrue("25", ColumnDefinition.DataType.Int);
        Assert.False(result);
    }

    [Fact]
    public void IsTrue_DoubleLessOrEqual_Works()
    {
        var condition = new Condition("Height", "<=", "1.70");
        bool result = condition.IsTrue("1.67", ColumnDefinition.DataType.Double);
        Assert.True(result);
    }

    [Fact]
    public void IsTrue_StringNotEqual_Works()
    {
        var condition = new Condition("Name", "!=", "Pepe");
        bool result = condition.IsTrue("Juan", ColumnDefinition.DataType.String);
        Assert.True(result);
    }


    [Fact]
    public void IsTrue_IntLess_Works()
    {
        var condition = new Condition("Age", "<", "50");
        bool result = condition.IsTrue("25", ColumnDefinition.DataType.Int);
        Assert.True(result);
    }
        
    }
}