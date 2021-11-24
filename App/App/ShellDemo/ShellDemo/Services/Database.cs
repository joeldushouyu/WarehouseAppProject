using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ShellDemo.Models;
namespace ShellDemo.Services
{
    public class Database
    {

        
        readonly SQLiteAsyncConnection databaseOrder;
        readonly SQLiteAsyncConnection databaseOrderAction;

        public Database()
        {
            
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data.db3");
            databaseOrder = new SQLiteAsyncConnection(dbPath);
            databaseOrder.CreateTableAsync<Order>().Wait();

            databaseOrderAction = new SQLiteAsyncConnection(dbPath);
            
            databaseOrderAction.CreateTableAsync<OrderAction>().Wait();


        }


        public Task<List<Order>> GetOrdersAsync()
        {
            //Get all notes.
            return databaseOrder.Table<Order>().ToListAsync();
        }



        public Task<int> SaveOrderAsync(Order ord)
        {
         
                // Save a new note.
            return databaseOrder.InsertAsync(ord);
      
        }

        public Task<int> DeleteOrderAsync(Order ord)
        {
            // Delete a order
            return databaseOrder.DeleteAsync(ord);
        }






        public Task<List<OrderAction>> GetOrderActionsAsync()
        {
            //Get all notes.
            return databaseOrderAction.Table<OrderAction>().ToListAsync();
        }



        public Task<int> SaveOrderActionAsync(OrderAction ord)
        {
            if (ord.ID != 0)
            {
                // Update an existing note.
                return databaseOrderAction.UpdateAsync(ord);
            }
            else
            {
                // Save a new note.
                return databaseOrderAction.InsertAsync(ord);
            }
        }

        public Task<int> DeleteNoteAsync(OrderAction ord)
        {
            // Delete a note.
            return databaseOrderAction.DeleteAsync(ord);
        }
    }
}
