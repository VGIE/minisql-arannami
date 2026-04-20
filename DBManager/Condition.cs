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
                if (Operator == "=") 
                    return value == LiteralValue;
                if (Operator == ">") 
                    return value.CompareTo(LiteralValue) > 0;
                if (Operator == "<") 
                    return value.CompareTo(LiteralValue) < 0;
            }
            if (type == ColumnDefinition.DataType.Int)
            {
                int a = int.Parse(value);
                int b = int.Parse(LiteralValue);

                if (Operator == "=") 
                    return a == b;
                if (Operator == ">") 
                    return a > b;
                if (Operator == "<") 
                    return a < b;
            }
            if (type == ColumnDefinition.DataType.Double)
            {
                double a = double.Parse(value);
                double b = double.Parse(LiteralValue);

                if (Operator == "=") 
                    return a == b;
                if (Operator == ">") 
                    return a > b;
                if (Operator == "<") 
                    return a < b;
            }
            return false;
            
        }

        private bool ComparaString(string a, string b)
        {
            int cmp = string.Compare(a, b, StringComparison.Ordinal);

            return Operator switch
            {
                "="  => cmp == 0,
                "!=" => cmp != 0,
                "<"  => cmp < 0,
                "<=" => cmp <= 0,
                ">"  => cmp > 0,
                ">=" => cmp >= 0,
                _ => false
            };
        }

        private bool ComparaNumero<T>(T a, T b) where T : IComparable<T>
        {
            int cmp = a.CompareTo(b);

            return Operator switch
            {
                "="  => cmp == 0,
                "!=" => cmp != 0,
                "<"  => cmp < 0,
                "<=" => cmp <= 0,
                ">"  => cmp > 0,
                ">=" => cmp >= 0,
                _ => false
            };
        }
    }
}