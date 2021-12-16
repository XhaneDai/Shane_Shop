using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

[TestFixture]
   public class TestClass
    {

    [Test]
    public void checkIfMinOfItemsWorks()
    {
        AdoCommand scd = new AdoCommand();
        int minOfItems = scd.GetMinOfItems();
        int expectedResult = 45;
        Assert.AreEqual(minOfItems, expectedResult);
    }

    [Test]
    public void checkIfMaxOfItemsWorks()
    {
        AdoCommand scd = new AdoCommand();
        int maxOfItems = scd.GetMaxOfItems();
        int expectedResult = 100;
        Assert.AreEqual(maxOfItems, expectedResult);
    }




    [Test]
    public void checkIfCountRecordsWorks()
    {
        AdoCommand scd = new AdoCommand();
        int numberOfRecords = scd.GetNumberOfRecords();
        int expectedResult = 11;
        Assert.AreEqual(numberOfRecords, expectedResult);
    }

    [Test]
        public void checkIfMeanOfItemsWorks()
        {
        AdoCommand scd = new AdoCommand();
        int meanOfItems = scd.GetmeanOfItems();
        int expectedResult = 80;
        Assert.AreEqual(meanOfItems, expectedResult);
        }

    [Test]
    public void checkInsertRecordsWorks()
    {
        AdoCommand scd = new AdoCommand();
        {
            SqlConnection conn;
            conn =
                new SqlConnection(
                    "Data Source = lugh4.it.nuigalway.ie; Initial Catalog = msdb2782; Persist Security Info = True; UID=msdb2782; Password = msdb2782JO");

            try
            {
                // Open the connection
                conn.Open();

                // prepare command string
                string insertString = @"
                 insert into Customers
                 (ID,Name, PhoneNo)
                 values (123234345,'John', 034535382)";

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand(insertString, conn);

                // 2. Call ExecuteNonQuery to send command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                    Assert.Pass();
                }
            }
        }
    }

    [Test]
    public void checkReadRecordsWorks()
    {
        AdoCommand scd = new AdoCommand();
        {
            SqlDataReader rdr = null;
            SqlConnection conn;
            conn = new SqlConnection("Data Source = lugh4.it.nuigalway.ie; Initial Catalog = msdb2782; Persist Security Info = True; UID=msdb2782; Password = msdb2782JO");

            try
            {
                // Open the connection
                conn.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand("select * from Customers", conn);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                    Assert.Pass();
                }
            }
        }
    }




}

