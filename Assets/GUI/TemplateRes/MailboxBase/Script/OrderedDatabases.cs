using UnityEngine;
using System.Collections;

// struct Record
// In this question we will work with a database of data records. Each record holds
// an integer value. These values will be random, and not necessarily unique. 
// The records also contains a reference m_next which will be used to link records together.
///////////////////////////////////////////////////////////////////////////////////////////////////
[System.Serializable]
class Record
{
    public Record(int val, Record next)
    {
        m_value = val;
        m_next = next;
    }

    public int m_value;     // Value stored in the record.
    public Record m_next;   // Reference used to link the records.
};

// class Database
// The records are stored in a database. The database consists of a variable 
// number of arrays of records. The number of arrays is given by m_arrayCount.
// Each array must have the same number of records in it, given by m_arraySize.
//
// For example, this code initializes a database with 3 arrays that each contain 5 records.
// The m_value fields are just random.
//
// Record array0[5]  = {{  4, 0}, { 89, 0}, {189, 0}, { 76, 0}, {200, 0}};
// Record array1[5]  = {{205, 0}, { 37, 0}, { 66, 0}, {105, 0}, { 13, 0}};
// Record array2[5]  = {{ 13, 0}, {241, 0}, {158, 0}, { 22, 0}, {140, 0}};
// Record* arrays[3] = {array0, array1, array2};
// Database database(3, 5, arrays);
///////////////////////////////////////////////////////////////////////////////////////////////////
class Database
{
    public Database(int arrayCount, int arraySize, Record[][] arrays)
    {
        m_arrayCount = arrayCount;
        m_arraySize = arraySize;
        m_arrays = arrays;
    }

    protected Record[][] m_arrays;
    protected int m_arrayCount;
    protected int m_arraySize;
};

// class OrderedDatabase
// This subclass of Database keeps all of the records in order by their m_value variable, 
// from smallest to largest. The records stay in their original location in memory, but are 
// are linked together in order using their m_next pointer. The first record in the ordering,
// i.e. the one with the smallest m_value, is pointed to by m_firstRecord.
//
// The function LinkInOrder initializes the links between the records.
// The function SwapRecordValue replaces one value in the database with another.
///////////////////////////////////////////////////////////////////////////////////////////////////
class OrderedDatabase : Database
{
    public OrderedDatabase(int arrayCount, int arraySize, Record[][] arrays)
        : base(arrayCount, arraySize, arrays)
    {
        LinkInOrder();
    }

    Record m_firstRecord;

    // Part A)
    //
    // Name:   void OrderedDatabase::LinkInOrder();
    //
    // Input:  
    //         The database must be initialized and have m_arrays, m_arrayCount and m_arraySize values set.
    //         Make no assumptions about how the Record m_next pointers are initially set.
    //
    // Output: 
    //         The function should use the m_next pointers of the database records to link them together
    //         in sorted order by m_value, from smallest to largest. It should set m_firstRecord to be a 
    //         pointer to the first record in the chain. The last record in the chain should have
    //         m_next == 0.
    //
    //         Eg:
    //         For the example Database shown above, the OrderedDatabase would be linked like this:
    //         m_firstRecord->4->13->13->22->37->66->76->89->105->140->158->189->200->205->241->NULL
    //
    // Specifications:
    //     This code will run only at initialization time, so it is not called very often. 
    //     Your algorithm should complete in O(n2) time and use O(1) memory.
    //     Your code should not crash as long as the database meets the specifications above.
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    void LinkInOrder()
    {

    }

    // Part B)
    //
    // Name:   void OrderedDatabase::SwapRecordValues(int oldValue, int newValue);
    //
    // Input:  
    //         Assuming that the database is already linked using the LinkInOrder() function.
    //
    // Output: 
    //         The function should find all records that have m_value equal to oldValue and change
    //         their value to newValue. The links in the database should be fixed to maintain the sorted
    //         ordering.
    //
    //         Eg:
    //         If the database is linked up so like this:
    //         m_firstRecord->4->13->13->22->37->66->76->89->105->140->158->189->200->205->241->NULL
    //         
    //         Calling:
    //         database.SwapRecordValue(13, 2);
    //
    //         Will result in the database being like this:
    //         m_firstRecord->2->2->4->22->37->66->76->89->105->140->158->189->200->205->241->NULL
    //
    // Specifications:
    //     This function will be called on a large database several times per frame of the 
    //     videogame, so performance is critical. It needs to be as fast as possible. 
    //     Your algorithm should use O(1) memory.
    //     Your code should not crash as long as the database meets the specifications above.
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    void SwapRecordValue(int oldValue, int newValue)
    {

    }

    // Part C)
    //
    // Name: void TestOrderedDatabase(void);
    //
    //
    // Specifications:
    //     Write test cases for the LinkInOrder and SwapRecordValue functions in OrderedDatabase 
    //     to show that the functions meet the specifications.
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    public void TestOrderedDatabase()
    {

    }
};