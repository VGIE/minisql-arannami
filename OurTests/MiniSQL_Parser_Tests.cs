using DbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests
{
    [TestFixture]
    internal class MiniSQL_Parser_Tests
    {
        [Test]
        public void Update_WithCommasInValues_ParsesCorrectly()
        {
            // Escenario: Un valor contiene una coma, lo cual rompería un Split(',') simple
            // Arrange
            string query = "UPDATE Users SET Address='Calle 1, Bloque 2', City='Madrid' WHERE ID=1";

            // Act
            var result = MiniSQLParser.Parse(query) as Update;

            // Assert
            Assert.IsNotNull(result, "El parser devolvió null para una sintaxis válida.");
            Assert.AreEqual("Users", result.Table);

            // Verificamos el primer SetValue (el que tiene la coma)
            Assert.AreEqual("Address", result.Columns[0].Column);
            Assert.AreEqual("Calle 1, Bloque 2", result.Columns[0].Value);

            // Verificamos el segundo SetValue
            Assert.AreEqual("City", result.Columns[1].Column);
            Assert.AreEqual("Madrid", result.Columns[1].Value);
        }

        [Test]
        public void Update_WithoutWhere_ParsesCorrectly()
        {
            // Arrange
            string query = "UPDATE Inventory SET Stock=10, Status='Available'";

            // Act
            var result = MiniSQLParser.Parse(query) as Update;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Where, "El objeto Condition debería ser null si no hay WHERE.");
            Assert.AreEqual(2, result.Columns.Count);
        }

        [Test]
        public void DropTable_BasicCase_ParsesCorrectly()
        {
            // Arrange
            string query = "DROP TABLE Students";

            // Act
            var result = MiniSQLParser.Parse(query) as DropTable;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Students", result.Table);
        }

        [Test]
        public void CreateTable_EmptyColumns_ReturnsInstance()
        {
            // Según tus requerimientos: "parsing of CREATE TABLE should accept empty columns ()"
            // Arrange
            string query = "CREATE TABLE EmptyTable ()";

            // Act
            var result = MiniSQLParser.Parse(query);

            // Assert
            Assert.IsNotNull(result, "Debería aceptar paréntesis vacíos aunque luego falle la ejecución.");
        }

        [Test]
        public void Update_WithComplexCondition_ParsesConditionParts()
        {
            // Arrange
            string query = "UPDATE Employees SET Salary=2000 WHERE Age >= 30";

            // Act
            var result = MiniSQLParser.Parse(query) as Update;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Where);
            Assert.AreEqual("Age", result.Where.Column);
            Assert.AreEqual(">=", result.Where.Operator);
            Assert.AreEqual("30", result.Where.Value);
        }

        [Test]
        public void InvalidQuery_ReturnsNull()
        {
            // Arrange
            string query = "ESTO NO ES SQL";

            // Act
            var result = MiniSQLParser.Parse(query);

            // Assert
            Assert.IsNull(result, "El parser debe devolver null ante una sintaxis irreconocible.");
        }
    }


}
