using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
    public class Condition
    {
        public string ColumnName { get; private set; }
        public string Operator { get; private set; }
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables
            ColumnName = column;
            Operator = op;
            LiteralValue = literalValue;  
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)
            if (type == ColumnDefinition.DataType.String)
            {
                switch (Operator)
                {
                    case "=":
                        return value == LiteralValue;
                    case ">":
                        return value.CompareTo(LiteralValue) > 0;
                    case "<":
                        return value.CompareTo(LiteralValue) < 0;
                    default:
                        return false;
                }

            }
            if (type == ColumnDefinition.DataType.Int)
            {
                int a = int.Parse(value);
                int b = int.Parse(LiteralValue);

                switch (Operator)
                {
                    case "=":
                        return a == b;
                    case ">":
                        return a > b;
                    case "<":
                        return a < b;
                    default:
                        return false;

                }
            }
            if (type == ColumnDefinition.DataType.Double)
            {
                double a = double.Parse(value);
                double b = double.Parse(LiteralValue);

                switch (Operator)
                {
                    case "=":
                        return a == b;
                    case ">":
                        return a > b;
                    case "<":
                        return a < b;
                    default:
                        return false;
                }
            }
            return false;
            
        }

    }
}