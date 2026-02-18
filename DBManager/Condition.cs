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
            switch (type)
            {
                case ColumnDefinition.DataType.String:
                    return ComparaStrings(value, LiteralValue);

                case ColumnDefinition.DataType.Int:
                    int intValue = int.Parse(value, CultureInfo.InvariantCulture);
                    int intLiteral = int.Parse(LiteralValue, CultureInfo.InvariantCulture);
                    return ComparaNumeros(intValue, intLiteral);

                case ColumnDefinition.DataType.Double:
                    double doubleValue = double.Parse(value, CultureInfo.InvariantCulture);
                    double doubleLiteral = double.Parse(LiteralValue, CultureInfo.InvariantCulture);
                    return ComparaNumeros(doubleValue, doubleLiteral);

                default:
                    return false;
            }
            
        }

        private bool ComparaStrings(string a, string b)
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

        private bool ComparaNumeros<T>(T a, T b) where T : IComparable<T>
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