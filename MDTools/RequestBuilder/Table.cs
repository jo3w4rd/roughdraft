using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public class Table : StructuralElement
    {
        private List<List<Cell>> Content;
        public int RowCount => Content.Count;
        public int ColCount { get; private set; }

        public Table(int rows, int cols)
        {
            ColCount = cols;
            Content = new List<List<Cell>>(rows);
            for (int row = 0; row < rows; row++)
            {
                Content.Add(new List<Cell>());
                for (int col = 0; col < cols; col++)
                {
                    Content[row].Add(new Cell());
                }
            }
        }

        public Cell GetCell(int row, int col)
        {
            return Content[row][col];
        }
        
        public override (List<Request> requests, int length) RequestList(int insertionPoint, bool useEoS = true)
        {
            var requestChain = new List<Request>();
            int runningLength = 3; // 3 IS A BIT OF A MAGIC NUMBER, theoretically, it should be 4

            var tableReq = new Request();
            tableReq.InsertTable = new InsertTableRequest();
            tableReq.InsertTable.Columns = ColCount;
            tableReq.InsertTable.Rows = RowCount;
            //tableReq.InsertTable.EndOfSegmentLocation = new EndOfSegmentLocation();
            tableReq.InsertTable.Location = new Location();
            tableReq.InsertTable.Location.Index = insertionPoint;
            requestChain.Add(tableReq);
            var index = insertionPoint + 4;
            
            //Console.WriteLine($"Insert Table at {insertionPoint}");
            for (int row = 0; row <RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    var requestInfo = Content[row][col].RequestList(index, false);
                    Console.WriteLine($"Cell[{row},{col}] at {index}");
                    requestChain.AddRange(requestInfo.requests);
                    runningLength += requestInfo.length + 2;
                    index += 2 + requestInfo.length; // next col
                }

                runningLength += 1;
                index += 1; // next row
            }
            return (requestChain, runningLength);
        }
    }
}