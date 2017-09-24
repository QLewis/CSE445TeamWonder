using System;
using System.Threading;
using System.Collections.Generic;

//Not sure why this namespace is different from the rest of the project
namespace WeimoPlant
{
    //Multi-cell buffer class to hold and release resouces for the dealers and plants
    public class MultiCellBuffer
    {
        Semaphore resources;            //Semaphore for handling resources
        List<string> buffer;          //List for holding the data cells

        //Constructor
        public MultiCellBuffer(Int32 size)
        {
            this.resources = new Semaphore(size, size);
            this.buffer = new List<string>(size);
        }

        //Set one cell: Allow one dealer to access the buffer, lock it, and add a single cell
        public void setOneCell(string element)
        {
            resources.WaitOne();        //Decrement the semaphore by one
            lock (buffer) {
                //Create a new data cell and add it to the buffer
                buffer.Add(element);
            }
        }

        public string getOneCell()
        {
            string dataCell = null;
            lock (buffer) {
                if (buffer.Count != 0) {
					//Copy a data cell from the buffer, then remove it from the buffer
					dataCell = buffer[0];
                    buffer.RemoveAt(0);
                    resources.Release();        //Increment the semaphore by one
				}
            }


			//Return that data cell
			return dataCell;
        }
    }
}
