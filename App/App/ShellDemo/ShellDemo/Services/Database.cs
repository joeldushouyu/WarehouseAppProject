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


        public async Task<List<Order>> GetOrdersAsync()
        {
            //Get all notes.
            return await databaseOrder.Table<Order>().ToListAsync();
        }
        public async Task<int> SaveOrderAsync(Order ord)
        {
            return await databaseOrder.InsertAsync(ord);
         }

        public async Task<int> DeleteOrderAsync(Order ord)
        {
            return await  databaseOrder.DeleteAsync(ord);
        }

        public async Task<int> ClearOrderAsync()
        {
            return await databaseOrder.DeleteAllAsync<Order>();
        }

        public async Task<List<Order>> LoadOrderListWithOrderActionAsync()
        {
            List<Order> orders = await this.GetOrdersAsync();
            foreach(Order ord in orders)
            {
                List<OrderAction> actions = await databaseOrderAction.Table<OrderAction>().Where(
                    (ordAction) => ordAction.FromOrderId == ord.IDAtDatabase).ToListAsync();
                ord.OrderActions = actions;
            }
            return orders;
        }



        public async Task<List<OrderAction>> GetOrderActionsAsync()
        {
            return await  databaseOrderAction.Table<OrderAction>().ToListAsync();
        }

        public async Task<int> ClearOrderActionAsync()
        {
            return await databaseOrderAction.DeleteAllAsync<OrderAction>();
        }

        public async Task<int> SaveOrderActionAsync(OrderAction ord)
        {
        
            // Save a new note.
            return await databaseOrderAction.InsertAsync(ord);
            
        }
        public async Task<int> UpdateOrderActionAsync(OrderAction ord)
        {
            return await databaseOrderAction.UpdateAsync(ord);
        }

        public async Task<int> DeleteNoteAsync(OrderAction ord)
        {
            // Delete a note.
            return await databaseOrderAction .DeleteAsync(ord);
        }
    }
}
